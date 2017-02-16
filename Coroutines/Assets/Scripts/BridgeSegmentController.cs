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
        Destroy(collision.gameObject);
        rigidbody.velocity = Vector3.zero;
        StartCoroutine(dropBridge().GetEnumerator());
    }

    private void startFall()
    {
        this.rigidbody.useGravity = true;
    }

    private IEnumerable dropBridge()
    {
        int off = 0;
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            if (segmentIndex - off < 0 && segmentIndex + off >= bridgeController.segmentCount) yield break;
            if (segmentIndex - off >= 0) bridgeController[segmentIndex - off].startFall();
            if (segmentIndex + off < bridgeController.segmentCount && off != 0) bridgeController[segmentIndex + off].startFall();
            off++;
        }
    }
}
