using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRKeys;

public class OpenKeyboard : MonoBehaviour
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
                keyboard.playerSpace.transform.localPosition = player.position;
                keyboard.playerSpace.transform.localRotation = player.rotation;
                keyboard.Enable();
            }
            else
            {
                keyboard.Disable();
            }
        }
    }
}
