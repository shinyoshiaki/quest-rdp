using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utf8Json;

public class QrdpMonitorManager : MonoBehaviour
{
    public Connect connect;
    QrdpKeyboardManager keyboardManagerContext;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetKeyboardManagerContext(QrdpKeyboardManager _keyboardManagerContext)
    {
        keyboardManagerContext = _keyboardManagerContext;
    }

    public void OnKeyboardButtonClick()
    {
        keyboardManagerContext.SwitchKeyboard();
        keyboardManagerContext.Focus(OnInput, OnEnter);
    }

    class Input
    {
        public string type;
        public string payload;
    }

    void OnInput(string s)
    {
        var data = new Input
        {
            type = "key",
            payload = s
        };
        var json = JsonSerializer.ToJsonString(data);
        connect.Send(json);
    }

    void OnEnter(string _)
    {
        var data = new Input
        {
            type = "key",
            payload = "enter"
        };
        var json = JsonSerializer.ToJsonString(data);
        connect.Send(json);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
