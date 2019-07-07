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
        keyboard.OnAddChar.AddListener(HandleAddChar);
        keyboard.OnBackspace.AddListener(HandleBackspace);
    }

    private void OnDisable()
    {

        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);
        keyboard.OnAddChar.RemoveListener(HandleAddChar);
        keyboard.OnBackspace.RemoveListener(HandleBackspace);

        keyboard.Disable();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            SwitchKeyboard();
        }
    }

    public void SwitchKeyboard()
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

    public void HandleSubmit(string text)
    {
        Debug.Log("input " + text);
        if (OnEnter != null) OnEnter(text);

        keyboard.DisableInput();
        StartCoroutine(SubmitEmail(text));
    }

    public void HandleAddChar(string c)
    {
        if (OnInput != null) OnInput(c);
    }

    public void HandleBackspace()
    {
        if (OnInput != null) OnInput("backspace");
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