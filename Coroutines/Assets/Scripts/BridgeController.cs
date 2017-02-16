using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public int segmentCount = 20;
    public Vector3 offsetPosition = new Vector3(1, 0, 0);
    public GameObject bridgeSegmentPrefab;

    private BridgeSegmentController[] segmentControllers;

    void Start()
    {
        segmentControllers = new BridgeSegmentController[segmentCount];
        Vector3 startPosition = transform.position - (segmentCount / 2) * offsetPosition;
        for (int q = 0; q < segmentCount; q++)
        {
            var segment = Instantiate(bridgeSegmentPrefab);
            segment.transform.position = startPosition + q * offsetPosition;
            var segmentController = segment.GetComponent<BridgeSegmentController>();
            segmentController.bridgeController = this;
            segmentController.segmentIndex = q;
            segmentControllers[q] = segmentController;
        }
    }
}
