using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]
public class EnemyController : MonoBehaviour
{
    //public float MapController;
    public FollowPath followPath;
    public Wave wave;

    public float hp = 100;

    void Start()
    {
        if (followPath == null) followPath = GetComponent<FollowPath>();
        followPath.speed = wave.speed;
        hp = 100;
    }
    
    void Update()
    {
        
    }
}
