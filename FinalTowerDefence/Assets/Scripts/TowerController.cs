using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameController game;
    
    public float x, y;
    public float shotsPerSecond = 1;
    public float damage = 1;
    public float range = 2;

    public GameObject shotPrefab;

    private float shotsToFire = 0;

    private void Update()
    {
        shotsToFire += shotsPerSecond * Time.deltaTime;
        while (shotsToFire >= 1)
            fireShot();
    }
    private void fireShot()
    {
        shotsToFire--;
        //var shot = Instantiate(shotPrefab);
        
    }
}
