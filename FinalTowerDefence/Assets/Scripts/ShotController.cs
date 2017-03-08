using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public GameController game;

    public GameObject chainHitPrefab;

    public EnemyController target;
    public float damage;
    public PoisonEffect poison;
    public int chainHit;

    public float initialSpeed = 4;
    private Vector2 velocity;
    private float age;

    void Start()
    {
        var dir = Random.value * Mathf.PI * 2;
        velocity = new Vector2(Mathf.Sin(dir), Mathf.Cos(dir)) * initialSpeed;
    }
    
    void Update()
    {
        if (target == null || target.gameObject == null)
        {
            var newTarget = game.map.enemies
                .Select(en => new { enemy = en, distance_squared = point_distance_squared(transform.position, en.transform.position) })
                .Where(pair => pair.distance_squared < 80*80)
                .OrderBy(pair => pair.distance_squared)
                .Select(pair => pair.enemy)
                .FirstOrDefault();
            if (newTarget == null)
            {
                Destroy(gameObject);
                return;
            }
            target = newTarget;
        }

        age += Time.deltaTime;

        Vector2 pos = transform.position;
        pos += velocity * Time.deltaTime * 60;
        var newvel = (Vector2)target.transform.position - pos;
        var lerpAmt = Time.deltaTime + (age / 3);
        velocity = (1 - lerpAmt) * velocity + lerpAmt * newvel;
        transform.position = pos;

        if (point_distance_squared(target.transform.position, pos) < 6*6)
        {
            target.takeDamage(damage);
            if (poison != null && poison.damage > 0) target.takePoison(poison);

            if (chainHit != 0)
            {
                var targets = new List<EnemyController>();
                targets.Add(target);
                while (chainHit-- > 0)
                {
                    target = game.map.enemies
                        .Where(enemy => !targets.Contains(enemy))
                        .Select(en => new { enemy = en, distance_squared = point_distance_squared(target.transform.position, en.transform.position) })
                        .Where(pair => pair.distance_squared < 120*120)
                        .OrderBy(pair => pair.distance_squared)
                        .Select(pair => pair.enemy)
                        .FirstOrDefault();
                    if (target == null) break;
                    target.takeDamage(damage);
                    if (poison != null && poison.damage > 0) target.takePoison(poison.Clone());
                    targets.Add(target);
                }

                var chgobj = Instantiate(chainHitPrefab);
                var chcontroller = chgobj.GetComponent<ChainHitController>();
                chcontroller.CreateChainHit(targets);
            }
            
            Destroy(gameObject);
        }
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
}
