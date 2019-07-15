using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpActivateObject : MonoBehaviour
{
    public GameObject obj;
    public Transform baseTransform;
    public Vector3 offset = new Vector3(0, 0, 1);

    public void Activate()
    {
        obj.SetActive(true);
        var position = baseTransform.position +
           baseTransform.up * offset.y +
           baseTransform.right * offset.x +
           baseTransform.forward * offset.z;
        obj.transform.position = position;
        obj.transform.rotation = baseTransform.rotation;
    }
}
