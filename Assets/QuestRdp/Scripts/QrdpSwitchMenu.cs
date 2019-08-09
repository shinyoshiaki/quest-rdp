using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpSwitchMenu : MonoBehaviour
{

    public GameObject menu;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            Switch();
        }
    }

    void Switch()
    {
        if (menu.activeSelf)
        {
            menu.SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }
    }
}
