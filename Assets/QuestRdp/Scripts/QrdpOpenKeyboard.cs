using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

public class QrdpOpenKeyboard : MonoBehaviour
{
    public Keyboard keyboard;
    public Transform player;
    private bool isKeyboardOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        keyboard.Disable();
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
                Debug.Log("avtive");
                keyboard.Enable();
            }
            else
            {
                keyboard.Disable();
            }
        }
    }
}
