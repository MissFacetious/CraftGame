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
    public static Dictionary<string, Sprite[]> interactSpriteDict;

    [Range(0f, 100f)]
    public float rayLength = 2.5f;

    private RaycastHit hitInfo;
    private bool hitDetected;
    private float aboveCharacter = 150f;

    public enum buttons
    {
        okay,
        cancel,
        jump,
        run,
        menu,
    }

    private void Awake()
    {
        GameObject PlayerButton = GameObject.FindWithTag("PlayerButton");
        if (PlayerButton != null)
        {
            button = PlayerButton.GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError("Button not assigned.");
            }
        }


        // Preload input-specific prompts
        Sprite PS4_okay = Resources.Load<Sprite>("Input/PS4_Cross");
        Sprite PS4_cancel = Resources.Load<Sprite>("Input/PS4_Circle");
        Sprite PS4_jump = Resources.Load<Sprite>("Input/PS4_Triangle");
        Sprite PS4_run = Resources.Load<Sprite>("Input/PS4_Square");
        Sprite PS4_menu = Resources.Load<Sprite>("Input/PS4_Start");

        Sprite XB1_okay = Resources.Load<Sprite>("Input/XboxOne_A");
        Sprite XB1_cancel = Resources.Load<Sprite>("Input/XboxOne_B");
        Sprite XB1_jump = Resources.Load<Sprite>("Input/XboxOne_Y");
        Sprite XB1_run = Resources.Load<Sprite>("Input/XboxOne_X");
        Sprite XB1_menu = Resources.Load<Sprite>("Input/XboxOne_Menu");

        Sprite KB_okay = Resources.Load<Sprite>("Input/Keyboard_Black_Space");
        Sprite KB_cancel = Resources.Load<Sprite>("Input/Keyboard_Black_Esc");
        Sprite KB_jump = Resources.Load<Sprite>("Input/Keyboard_Black_Ctrl");
        Sprite KB_menu = Resources.Load<Sprite>("Input/Keyboard_Black_Del");
        Sprite KB_run = Resources.Load<Sprite>("Input/Keyboard_Black_Alt");
        //Sprite Switch_sprite = Resources.Load<Sprite>("Input/Switch_B");

        Sprite[] PS4 = new Sprite[] { PS4_okay, PS4_cancel, PS4_jump, PS4_run, PS4_menu };
        Sprite[] XBOX = new Sprite[] { XB1_okay, XB1_cancel, XB1_jump, XB1_run, XB1_menu };
        //Sprite[] SWITCH = new Sprite[] { Switch_sprite, Switch_sprite };
        Sprite[] KEYBOARD = new Sprite[] { KB_okay, KB_cancel, KB_jump, KB_run, KB_menu };

        interactSpriteDict = new Dictionary<string, Sprite[]>
        {
            { PlayerController.inputDevice.DualShock4GamepadHID.ToString(), PS4 },
            { PlayerController.inputDevice.XInputControllerWindows.ToString(), XBOX },
            { PlayerController.inputDevice.Keyboard.ToString(), KEYBOARD }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        if (button != null)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        hitDetected = Physics.BoxCast(transform.position, transform.localScale, transform.forward, out hitInfo, transform.rotation, rayLength);
        if (hitDetected)
        {
            Interactable i = hitInfo.collider.GetComponent<Interactable>();
            //Debug.Log("HitDetected");
            if (i != null && i.isReady)
            {
                SetFocus(i);
            } else  {
            //    Debug.Log(hitInfo.transform.name);
            }
        }
        else
        {
            if (hitInfo.collider != null)
            {
            //    Debug.Log(hitInfo.collider.name);
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

    public PlayerController.inputDevice UpdateIcons(string deviceName)
    {
        if (deviceName == "Touchscreen" || deviceName == "Mouse") deviceName = "Keyboard";

        if (deviceName == "Keyboard")
        {
            return PlayerController.inputDevice.Keyboard;
        }
        if (deviceName == "XInputControllerWindows")
        {
            return PlayerController.inputDevice.XInputControllerWindows;
        }
        if (deviceName == "DualShock4GamepadHID")
        {
            return PlayerController.inputDevice.DualShock4GamepadHID;
        }
        return PlayerController.inputDevice.Keyboard;
    }

    public Sprite UpdateIconSprite(string deviceName, buttons buttonName)
    {
        if (deviceName == "Touchscreen" || deviceName == "Mouse") deviceName = "Keyboard";
        if (interactSpriteDict.TryGetValue(deviceName, out Sprite[] deviceIcon))
        {
            if (buttonName.Equals(buttons.okay) && button != null)
            {
                Image image = button.GetComponent<Image>();
                image.sprite = deviceIcon[(int)buttonName];
            }
            return deviceIcon[(int)buttonName];
        }
        return null;
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
