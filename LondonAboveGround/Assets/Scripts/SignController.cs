using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignController : MonoBehaviour
{
    public float gustInterval = 1;
    public float minimumGustStrength = 1;
    public float maximumGustStrength = 3;
    public Vector3 gustDirection = new Vector3(0, 0, 1);

    private float timeUntilGust = 0;
    private new Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        timeUntilGust = gustInterval;
    }
    
    void Update()
    {
        timeUntilGust -= Time.deltaTime;
        if (timeUntilGust < 0) addGust();
    }

    private void OnMouseDown()
    {
        addGust();
    }

    private void addGust()
    {
        timeUntilGust = gustInterval;
        float gustStrength = minimumGustStrength + Random.value * (maximumGustStrength - minimumGustStrength);
        rigidbody.AddForce(gustDirection * gustStrength, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + gustDirection);
    }
}
