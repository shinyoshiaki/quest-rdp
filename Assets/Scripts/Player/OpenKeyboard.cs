using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

public class OpenKeyboard : MonoBehaviour
{
    Keyboard keyboard;
    private bool isKeyboardOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        keyboard = GetComponent<Keyboard>();
    }

    // Update is called once per frame
    void Update()
    {
        bool IsButtonXPressed = OVRInput.GetDown(OVRInput.Button.Three);
        if (IsButtonXPressed)
        {
            isKeyboardOpen = !isKeyboardOpen;
            if (isKeyboardOpen)
            {
                keyboard.Enable();
            }
            else
            {
                keyboard.Disable();
            }
        }
    }
}
