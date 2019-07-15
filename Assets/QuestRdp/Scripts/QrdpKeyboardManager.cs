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

    bool first = true;

    private void Enable()
    {
        if (first)
        {
            first = false;
            keyboard.SetPlaceholderMessage("");

            keyboard.OnSubmit.AddListener(HandleSubmit);
            keyboard.OnCancel.AddListener(HandleCancel);
            keyboard.OnAddChar.AddListener(HandleAddChar);
            keyboard.OnBackspace.AddListener(HandleBackspace);
        }
    }

    private void OnDisable()
    {

        keyboard.OnSubmit.RemoveListener(HandleSubmit);
        keyboard.OnCancel.RemoveListener(HandleCancel);
        keyboard.OnAddChar.RemoveListener(HandleAddChar);
        keyboard.OnBackspace.RemoveListener(HandleBackspace);

        keyboard.Disable();
    }

    public void SwitchKeyboard()
    {
        if (keyboard.disabled)
        {
            keyboard.Enable();
            Enable();
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
        StartCoroutine(SubmitText());
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

    private IEnumerator SubmitText()
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