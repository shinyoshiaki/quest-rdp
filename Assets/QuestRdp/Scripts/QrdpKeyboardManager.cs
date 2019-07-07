using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using VRKeys;


/// <summary>
/// Example use of VRKeys keyboard.
/// </summary>
public class QrdpKeyboardManager : MonoBehaviour
{
    public Keyboard keyboard;

    public delegate void IOnInput(string s);
    IOnInput OnInput;

    public delegate void IOnEnter(string s);
    IOnEnter OnEnter;

    private void OnEnable()
    {
        keyboard.Enable();
        keyboard.SetPlaceholderMessage("");

        keyboard.OnSubmit.AddListener(HandleSubmit);
        keyboard.OnCancel.AddListener(HandleCancel);
    }

    private void OnDisable()
    {

        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);

        keyboard.Disable();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (keyboard.disabled)
            {
                keyboard.Enable();
            }
            else
            {
                keyboard.Disable();
            }
        }
    }

    public void HandleSubmit(string text)
    {
        Debug.Log("input " + text);
        if (OnEnter != null) OnEnter(text);

        keyboard.DisableInput();
        StartCoroutine(SubmitEmail(text));
    }

    public void Focus(IOnInput _OnInput, IOnEnter _OnEnter)
    {
        OnInput = _OnInput;
        OnEnter = _OnEnter;
    }

    private IEnumerator SubmitEmail(string email)
    {
        keyboard.ShowInfoMessage("");
        yield return new WaitForSeconds(0.1f);
        keyboard.ShowSuccessMessage("");
        yield return new WaitForSeconds(0.1f);
        keyboard.SetText("");
        keyboard.EnableInput();
    }


    public void HandleCancel()
    {
        Debug.Log("Cancelled keyboard input!");
    }

}