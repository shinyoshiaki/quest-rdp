using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utf8Json;

public class QrdpMouseControll : MonoBehaviour
{
    Connect connect;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        connect = GetComponent<Connect>();
    }

    void Update()
    {
        bool IsButtonXPressed = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);
        if (IsButtonXPressed)
        {
            StartCoroutine(Click());
        }
    }

    IEnumerator Click()
    {
        SendMove(x, y);
        yield return new WaitForSeconds(0.05f);
        SendClick();
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

    public void SendMove(float x, float y)
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
    }

    public void Move(float _x, float _y)
    {
        x = _x;
        y = _y;
    }

    class MouseClick
    {
        public string type;
    }
    void SendClick()
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
