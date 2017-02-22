using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PartyHornController : MonoBehaviour
{
    public AudioClip clip;
    public float offsetTime;

    private AudioSource source;
    private float startTime;
    private bool hasStarted;

    void Start()
    {
        source = GetComponent<AudioSource>();
        startTime = Time.time + offsetTime;
        hasStarted = false;
    }
    
    void Update()
    {
        if (!hasStarted && Time.time > startTime)
        {
            hasStarted = true;
            source.PlayOneShot(clip);
        }
    }
}
