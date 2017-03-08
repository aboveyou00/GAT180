using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HealthBarController : MonoBehaviour
{
    public EnemyController enemy;

    public SpriteRenderer spriteRenderer;

    public Sprite regularHealthbar;
    public Sprite poisonedHealthbar;

    private void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.spriteRenderer.sprite = (enemy.IsPoisoned ? poisonedHealthbar : regularHealthbar);

        var scaleX = enemy.hp / enemy.totalHp;
        scaleX = Mathf.Max(Mathf.Min(scaleX, 1), 0);
        transform.localScale = new Vector3(scaleX, 1, 1);
    }
}
