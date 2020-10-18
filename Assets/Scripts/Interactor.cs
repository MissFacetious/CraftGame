using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    public Interactable focus;
    public Button button;

    [SerializeField]
    public static Sprite interactSprites;
    public static Dictionary<string, Sprite> interactSpriteDict;

    [Range(0f, 100f)]
    public float rayLength = 2.5f;

    private RaycastHit hitInfo;
    private bool hitDetected;
    private float aboveCharacter = 150f;

    private void Awake()
    {
        button = Canvas.FindObjectOfType<Button>();
        if (button == null)
        {
            Debug.LogError("Button not assigned.");
        }

        // Preload input-specific prompts
        Sprite PS4_sprite = Resources.Load<Sprite>("Input/PS4_Cross");
        Sprite XB1_sprite = Resources.Load<Sprite>("Input/XboxOne_A");
        Sprite KB_sprite = Resources.Load<Sprite>("Input/Keyboard_White_Space");
        Sprite Switch_sprite = Resources.Load<Sprite>("Input/Switch_B");

        interactSpriteDict = new Dictionary<string, Sprite>
        {
            { "DualShock4GamepadHID", PS4_sprite },
            { "XInputControllerWindows", XB1_sprite },
            { "SwitchProControllerHID", Switch_sprite},
            { "Keyboard", KB_sprite }
        };

        Assert.IsNotNull(PS4_sprite);
        Assert.IsNotNull(XB1_sprite);
        Assert.IsNotNull(KB_sprite);
        Assert.IsNotNull(Switch_sprite);
    }

    // Start is called before the first frame update
    void Start()
    {
        button.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        hitDetected = Physics.BoxCast(transform.position, transform.localScale, transform.forward, out hitInfo, transform.rotation, rayLength);
        if (hitDetected)
        {
            Interactable i = hitInfo.collider.GetComponent<Interactable>();
            //Debug.Log("HitDetected");
            if (i != null)
            {
                SetFocus(i);
            } else  {
                Debug.Log(hitInfo.transform.name);
            }
        }
        else
        {
            if (hitInfo.collider != null)
            {
                Debug.Log(hitInfo.collider.name);
            }
            RemoveFocus();
        }
    }

    public void PerformInteraction()
    {
        if (focus != null)
        {
            focus.Interact();
        }
    }

    private void RemoveFocus()
    {
        if (focus != null)
        {
            focus.OnUnfocused();
            focus = null;
            button.gameObject.SetActive(false);
        }
    }

    private void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
            {
                focus.OnUnfocused();
            }

            //Debug.Log("Hit: " + hitInfo.collider.name);
            focus = newFocus;
            focus.OnFocused(transform);
        }
        
        button.gameObject.SetActive(true);
        // show the sprite on the canvas in the right position

        Vector3 buttonPosition = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        button.transform.position = new Vector3(buttonPosition.x, buttonPosition.y+aboveCharacter, 0f);
    }

    public void UpdateIconSprite(string deviceName)
    {
        if (interactSpriteDict.TryGetValue(deviceName, out Sprite deviceIcon))
        {
            Image image = button.GetComponent<Image>();
            image.sprite = deviceIcon;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (hitDetected)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hitInfo.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * hitInfo.distance, transform.localScale);
        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * rayLength);
            Gizmos.DrawWireCube(transform.position + transform.forward * rayLength, transform.localScale);
        }
    }
}
