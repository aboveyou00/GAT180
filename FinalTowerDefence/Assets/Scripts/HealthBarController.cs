using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public EnemyController enemy;

    private void Update()
    {
        var scaleX = enemy.hp / enemy.totalHp;
        scaleX = Mathf.Max(Mathf.Min(scaleX, 1), 0);
        transform.localScale = new Vector3(scaleX, 1, 1);
    }
}
