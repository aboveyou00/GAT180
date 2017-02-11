using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followObject;
    public float planeDistance = 3;
    public float elevation = 2;
    public float lerpPow = .3f;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        if (followObject == null) return;
        var target = followObject.transform;
        thisCamera.transform.LookAt(target);
        float planeAngle = Mathf.PI / 2;
        if (target.position.x != thisCamera.transform.position.x || target.position.z != thisCamera.transform.position.z)
        {
            planeAngle = Mathf.Atan2(thisCamera.transform.position.z - target.position.z, thisCamera.transform.position.x - target.position.x);
        }
        var endDest = target.position + new Vector3(planeDistance * Mathf.Cos(planeAngle), elevation, planeDistance * Mathf.Sin(planeAngle));
        float mixAmt = 1.0f - Mathf.Pow(this.lerpPow, Time.deltaTime);
        thisCamera.transform.position = MathExt.Mix(thisCamera.transform.position, endDest, mixAmt);
    }

    private Camera thisCamera;
}
