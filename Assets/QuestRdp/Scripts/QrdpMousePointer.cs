using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class QrdpMousePointer : MonoBehaviour
{
    public GameObject reticle;

    [SerializeField]
    Transform grip = null;

    [SerializeField]
    float maxGrabDistance = 10f;

    [SerializeField]
    LayerMask layerMask = ~0;

    Vector3 preRayDirection_;

    RaycastHit targetHit_;

    GameObject pos;
    QrdpRemoteInput remoteInput;


    // Start is called before the first frame update
    void Start()
    {
        pos = new GameObject();
        Observable.Interval(TimeSpan.FromMilliseconds(1000 / 30)).Subscribe(l =>
        {
            var pos = DesktopRay();
            if (pos.mouse)
                pos.mouse.Move(pos.x, pos.y);
        }).AddTo(this);
        reticle = Instantiate(reticle);
        remoteInput = GetComponent<QrdpRemoteInput>();
    }

    public Transform gripTransform
    {
        get { return grip ? grip : transform; }
    }


    // Update is called once per frame
    (float x, float y, QrdpMouseControll mouse) DesktopRay()
    {
        float x = 0; float y = 0;
        QrdpMouseControll mouse = null;

        var forward = gripTransform.forward;

        var ray = new Ray();
        ray.origin = gripTransform.position;
        ray.direction = Vector3.Lerp(preRayDirection_, forward, 0.25f);

        targetHit_ = new RaycastHit();
        bool hit = Physics.Raycast(ray, out targetHit_, maxGrabDistance, layerMask);

        if (hit)
        {
            var obj = targetHit_.collider.gameObject;
            mouse = obj.GetComponent<QrdpMouseControll>();
            if (mouse)
            {
                var hitPoint = new Vector2(targetHit_.textureCoord.x, targetHit_.textureCoord.y);
                x = hitPoint.x;
                y = hitPoint.y;

                reticle.transform.rotation = Quaternion.LookRotation(targetHit_.normal);
                reticle.transform.position = targetHit_.point + (-targetHit_.normal * 0.01f);

            }
            var connect = obj.GetComponent<Connect>();
            if (connect)
            {
                remoteInput.SetConnectContext(connect);
            }
        }

        preRayDirection_ = hit ? ray.direction : forward;

        return (x, y, mouse);
    }

}
