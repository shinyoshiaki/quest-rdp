using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utf8Json;

public class QrdpMouseControll : MonoBehaviour
{
    Connect connect;
    // Start is called before the first frame update
    void Start()
    {
        connect = GetComponent<Connect>();
    }

    void Update()
    {


    }

    class MouseMovePayload
    {
        public float x;
        public float y;
    }

    class MouseMove
    {
        public string type;
        public MouseMovePayload payload;
    }

    public void Move(float x, float y)
    {
        var data = new MouseMove
        {
            type = "move",
            payload = new MouseMovePayload
            {
                x = x,
                y = y
            }
        };
        var json = JsonSerializer.ToJsonString(data);
        connect.Send(json);

        bool IsButtonXPressed = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        if (IsButtonXPressed)
        {
            Click();
        }
    }

    class MouseClick
    {
        public string type;
    }
    public void Click()
    {
        Debug.Log("clicked");
        var data = new MouseClick
        {
            type = "click"
        };
        var json = JsonSerializer.ToJsonString(data);
        connect.Send(json);
    }
}
