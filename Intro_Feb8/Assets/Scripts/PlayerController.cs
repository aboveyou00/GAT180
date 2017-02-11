using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera followingCamera;
    public new Collider collider;
    public new Rigidbody rigidbody;
    public float movementSpeed = 200;
    public float jumpHeight = 200;
    public float maxPlaneVelocity = 8;

    private void Start()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
        if (collider == null) collider = GetComponent<Collider>();
    }

    void Update()
    {
        if (followingCamera == null) return;

        float forwardAngle;
        if (followingCamera.transform.position.x == transform.position.x && followingCamera.transform.position.z == transform.position.z) forwardAngle = Mathf.PI / 2;
        else forwardAngle = Mathf.Atan2(transform.position.z - followingCamera.transform.position.z, transform.position.x - followingCamera.transform.position.x);
        forwardVec = new Vector3(Mathf.Cos(forwardAngle), 0, Mathf.Sin(forwardAngle));
        rightVec = new Vector3(Mathf.Cos(forwardAngle - (Mathf.PI / 2)), 0, Mathf.Sin(forwardAngle - (Mathf.PI / 2)));

        this.transform.forward = forwardVec;
    }

    private void FixedUpdate()
    {
        if (rigidbody == null) return;
        if (collider == null) return;

        Vector3 planeVelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        var horiz = Input.GetAxisRaw("Horizontal");
        var vert = Input.GetAxisRaw("Vertical");
        if (horiz == 0 && vert == 0) planeVelocity *= .8f;
        rigidbody.AddForce(forwardVec * movementSpeed * vert);
        rigidbody.AddForce(rightVec * movementSpeed * horiz);

        bool isOnFloor = Physics.Raycast(transform.position, -Vector3.up, collider.bounds.extents.y + .1f);
        if (isOnFloor)
        {
            bool jump = Input.GetAxisRaw("Jump") != 0;
            if (jump) rigidbody.AddForce(Vector3.up * jumpHeight);
        }

        if (planeVelocity.magnitude > maxPlaneVelocity) planeVelocity = planeVelocity.normalized * maxPlaneVelocity;
        rigidbody.velocity = new Vector3(planeVelocity.x, rigidbody.velocity.y, planeVelocity.z);
    }

    private Vector3 forwardVec, rightVec;
}
