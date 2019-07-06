using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpFollow : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = target.transform.rotation;
        transform.position = target.transform.position;
    }
}
