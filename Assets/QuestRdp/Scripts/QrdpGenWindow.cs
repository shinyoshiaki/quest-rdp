using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpGenWindow : MonoBehaviour
{
    public GameObject connect;
    public GameObject player;

    QrdpKeyboardManager keyboardManager;
    public QrdpUiNumpad Numpad;

    // Start is called before the first frame update
    void Start()
    {
        keyboardManager = GetComponent<QrdpKeyboardManager>();
        Focus();
        Numpad.OnEnter = StartConnect;
    }

    public void Focus()
    {
        keyboardManager.Focus(null, StartConnect);
    }

    public void StartConnect(string text)
    {
        var offset = new Vector3(0, 1, 3);
        var position = player.transform.position +
           player.transform.up * offset.y +
           player.transform.right * offset.x +
           player.transform.forward * offset.z;
        var obj = Instantiate(connect, position, player.transform.rotation);

        var peer = obj.GetComponentInChildren<Connect>();
        peer.StartConnect(text);
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Focus();
        }
    }
}
