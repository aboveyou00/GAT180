using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    public Color playbackColor = Color.blue;
    public Color rewindColor = Color.red;

    public int segmentCount = 20;
    public Vector3 offsetPosition = new Vector3(1, 0, 0);
    public GameObject bridgeSegmentPrefab;

    public bool recording = false, reversing = false;

    private BridgeSegmentController[] segmentControllers;
    private List<Dictionary<GameObject, TimeState>> timeStates = new List<Dictionary<GameObject, TimeState>>();

    public float accelerationThreshold = 3;
    
    private void Start()
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.acceleration.y > accelerationThreshold)
        {
            if (!reversing)
            {
                reversing = true;
                recording = false;
                for (int q = 0; q < segmentControllers.Length; q++)
                {
                    var segment = this[q];
                    segment.StopAllCoroutines();
                    var gobj = segment.gameObject;
                    var rigidbody = gobj.GetComponent<Rigidbody>();
                    rigidbody.useGravity = false;
                    rigidbody.velocity = Vector3.zero;
                }
                Camera.main.backgroundColor = rewindColor;
            }
        }

        if (reversing && timeStates.Count == 0)
        {
            reversing = false;
            Camera.main.backgroundColor = playbackColor;
        }
        if (reversing)
        {
            var dict = timeStates[timeStates.Count - 1];
            timeStates.RemoveAt(timeStates.Count - 1);
            foreach (var kvp in dict)
            {
                if (kvp.Key != null)
                {
                    kvp.Key.transform.position = kvp.Value.position;
                    kvp.Key.transform.rotation = kvp.Value.rotation;
                }
            }
        }
        else if (recording)
        {
            var dict = new Dictionary<GameObject, TimeState>();
            for (int q = 0; q < segmentControllers.Length; q++)
            {
                var gobj = this[q].gameObject;
                dict[gobj] = new TimeState(gobj.transform.position, gobj.transform.rotation);
            }
            timeStates.Add(dict);
        }
    }

    public BridgeSegmentController this[int idx]
    {
        get
        {
            return segmentControllers[idx];
        }
    }

    private struct TimeState
    {
        public TimeState(Vector3 pos, Quaternion rotation)
        {
            this.position = pos;
            this.rotation = rotation;
        }

        public readonly Vector3 position;
        public readonly Quaternion rotation;
    }
}
