using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utf8Json;

public class QrdpRemoteScroll : MonoBehaviour
{
    private Connect connect;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetConnectContext(Connect _connect)
    {
        connect = _connect;
    }

    // Update is called once per frame

    class Input
    {
        public string type;
        public string payload;
    }

    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickUp))
        {
            var data = new Input
            {
                type = "key",
                payload = "up"
            };
            var json = JsonSerializer.ToJsonString(data);
            if (connect) connect.Send(json);
        }
        if (OVRInput.Get(OVRInput.RawButton.RThumbstickDown))
        {
            var data = new Input
            {
                type = "key",
                payload = "down"
            };
            var json = JsonSerializer.ToJsonString(data);
            if (connect) connect.Send(json);
        }
    }
}
