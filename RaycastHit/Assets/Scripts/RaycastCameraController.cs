using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCameraController : MonoBehaviour
{
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo) && Input.GetMouseButtonDown(0))
        {
            GameObject obj = hitInfo.collider.gameObject;

            var material = obj.GetComponent<Renderer>().material;
            material.color = Color.red;

            var rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.AddForceAtPosition(ray.direction * 50, hitInfo.point, ForceMode.Impulse);
        }
    }
}
