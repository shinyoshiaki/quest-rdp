using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpZoom : MonoBehaviour
{
    public GameObject target;
    Vector3 def;
    Vector3 mydef;

    void Start()
    {
        def = target.transform.localScale;
        mydef = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var x = target.transform.localScale.x / def.x;
        var y = target.transform.localScale.x / def.x;

        transform.localScale = new Vector3(mydef.x * x, mydef.y * y, mydef.z);
    }
}
