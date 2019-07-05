using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour
{
    public string ipAddress = "192.168.0.5";
    public Connect connect;

    void Start()
    {
        connect.StartConnect(ipAddress);
    }

}
