using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Interactor : MonoBehaviour
{
    public Interactable focus;

    [SerializeField]
    public static Sprite interactSprites;
    public static Dictionary<string, Sprite> interactSpriteDict;

    [Range(0f, 100f)]
    public float rayLength = 2.5f;

    [SerializeField]
    private GameObject buttonPrompt;
    private List<Interactable> focusList;
    private RaycastHit hitInfo;
    private bool hitDetected;

    private void Awake()
    {
        if (buttonPrompt == null)
        {
            Debug.LogError("Button prompt not assigned.");
        }

        // Preload input-specific prompts
        Sprite PS4_sprite = Resources.Load<Sprite>("Input/PS4_Cross");
        Sprite XB1_sprite = Resources.Load<Sprite>("Input/XboxOne_A");
        Sprite KB_sprite = Resources.Load<Sprite>("Input/Keyboard_White_Space");

        interactSpriteDict = new Dictionary<string, Sprite>
        {
            { "DualShock4GamepadHID", PS4_sprite },
            { "XInputControllerWindows", XB1_sprite },
            { "Keyboard", KB_sprite }
        };

        Assert.IsNotNull(PS4_sprite);
        Assert.IsNotNull(XB1_sprite);
        Assert.IsNotNull(KB_sprite);
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO (aoyeola): Do we still need to check for many interactables at once?
        focusList = new List<Interactable>();
        buttonPrompt.SetActive(false);
    }

    private void FixedUpdate()
    {
        hitDetected = Physics.BoxCast(transform.position, transform.localScale, transform.forward, out hitInfo, transform.rotation, rayLength);
        if (hitDetected)
        {
            Interactable i = hitInfo.collider.GetComponent<Interactable>();
            if (i != null)
            {
                SetFocus(i);
            }
        }
        else
        {
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
            buttonPrompt.SetActive(false);
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

        buttonPrompt.SetActive(true);
    }

    public void UpdateIconSprite(string deviceName)
    {
        if (interactSpriteDict.TryGetValue(deviceName, out Sprite deviceIcon))
        {
            SpriteRenderer renderer = buttonPrompt.GetComponentInChildren<SpriteRenderer>();
            renderer.sprite = deviceIcon;
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
