using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public GameController game;
    
    public float x, y;
    public float baseShotsPerSecond = 1;
    public float ShotsPerSecond
    {
        get
        {
            return baseShotsPerSecond * (1 + Mathf.Pow(speedUpgrades, 1.6f));
        }
    }
    public float baseDamage = 30;
    public float Damage
    {
        get
        {
            var upgradeDamageComponent = (1 + Mathf.Pow(damageUpgrades, 1.6f));
            var critComponent = (Random.value < (Mathf.Sqrt(criticalHitUpgrades) / 10) ? Mathf.Pow(2 + criticalHitUpgrades, 2.5f) : 1);
            return baseDamage * upgradeDamageComponent * critComponent;
        }
    }
    public float baseRange = 1.6f;
    public float Range
    {
        get
        {
            return baseRange * (1 + Mathf.Pow(rangeUpgrades, 1.6f));
        }
    }

    public int ChainHit
    {
        get
        {
            int ch = 0;
            for (int q = chainHitUpgrades; q > 0; q++)
            {
                if (Random.value * q > .5) ch++;
                else break;
            }
            return ch;
        }
    }
    public PoisonEffect Poison
    {
        get
        {
            var dmg = Damage;
            return new PoisonEffect(Mathf.Pow(2, poisonUpgrades) * dmg, Mathf.Sqrt(poisonUpgrades));
        }
    }
    
    public int rangeUpgrades, damageUpgrades, speedUpgrades, chainHitUpgrades, criticalHitUpgrades, poisonUpgrades;
    public int UpgradesPurchased
    {
        get
        {
            return rangeUpgrades + damageUpgrades + speedUpgrades + chainHitUpgrades + criticalHitUpgrades + poisonUpgrades;
        }
    }

    public GameObject shotPrefab;

    private float shotsToFire = 0;

    private void Update()
    {
        shotsToFire += ShotsPerSecond * Time.deltaTime;
        while (shotsToFire >= 1 && fireShot())
            shotsToFire--;
        if (shotsToFire > 1) shotsToFire = 1;
    }
    private bool fireShot()
    {
        var center = new Vector2(transform.localPosition.x + 30, transform.localPosition.y + 30);
        var range_squared = Mathf.Pow(Range * 60, 2);
        var fireAt = game.map.enemies
            .Where(enemy => point_distance_squared(center, enemy.transform.position) < range_squared)
            .OrderBy(enemy => enemy.age)
            .FirstOrDefault();
        if (fireAt == null) return false;

        var shot = Instantiate(shotPrefab);
        shot.transform.position = center;
        var shotC = shot.GetComponent<ShotController>();
        shotC.target = fireAt;
        shotC.damage = Damage;
        return true;
    }
    private float point_distance(Vector2 one, Vector2 two)
    {
        return Mathf.Sqrt(point_distance_squared(one, two));
    }
    private float point_distance_squared(Vector2 one, Vector2 two)
    {
        var diffx = one.x - two.x;
        var diffy = one.y - two.y;
        return diffx * diffx + diffy * diffy;
    }

    private void OnDrawGizmos()
    {
        const int max_segments = 32;
        var center = new Vector2(transform.localPosition.x + 30, transform.localPosition.y + 30);
        for (int q = 0; q < max_segments; q++)
        {
            var anglefrom = ((Mathf.PI * 2) / max_segments) * q;
            var angleto = ((Mathf.PI * 2) / max_segments) * (q + 1);
            var range = Range;
            Vector3 frompos = new Vector3(center.x + Mathf.Sin(anglefrom) * range * 60, center.y + Mathf.Cos(anglefrom) * range * 60, 10);
            Vector3 topos = new Vector3(center.x + Mathf.Sin(angleto) * range * 60, center.y + Mathf.Cos(angleto) * range * 60, 10);
            Gizmos.DrawLine(frompos, topos);
        }
    }
}
