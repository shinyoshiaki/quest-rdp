using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QrdpMousePointer : MonoBehaviour
{
    [SerializeField]
    Transform grip = null;

    [SerializeField]
    float maxGrabDistance = 10f;

    [SerializeField]
    LayerMask layerMask = ~0;

    Vector3 preRayDirection_;

    RaycastHit targetHit_;

    GameObject pos;


    // Start is called before the first frame update
    void Start()
    {
        pos = new GameObject();
    }

    public Transform gripTransform
    {
        get { return grip ? grip : transform; }
    }


    // Update is called once per frame
    void Update()
    {
        var forward = gripTransform.forward;

        var ray = new Ray();
        ray.origin = gripTransform.position;
        ray.direction = Vector3.Lerp(preRayDirection_, forward, 0.25f);

        targetHit_ = new RaycastHit();
        bool hit = Physics.Raycast(ray, out targetHit_, maxGrabDistance, layerMask);

        if (hit)
        {
            var obj = targetHit_.collider.gameObject;
            var mouse = obj.GetComponent<QrdpMouseControll>();
            if (mouse)
            {
                var hitPoint = new Vector2(targetHit_.textureCoord.x, targetHit_.textureCoord.y);
                mouse.Move(hitPoint.x, hitPoint.y);
            }
        }




        preRayDirection_ = hit ? ray.direction : forward;
    }

}
