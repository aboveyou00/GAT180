using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeSegmentController : MonoBehaviour
{
    public int segmentIndex;
    public BridgeController bridgeController;
    public new Rigidbody rigidbody;

    private void Start()
    {
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.rigidbody.useGravity = true;
    }
}
