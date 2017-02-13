using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPrefabController : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite[] frontface_sprites;
    public Sprite backface_sprite;
    public int value;

    private AnimationState animationState;
    private float animationStateEndTime = 0;
    public const float SHOW_INITIAL_DURATION = 2;
    public const float FLIP_ANIMATION_DURATION = .2f;
    
    private Sprite frontface_sprite;

    private void Start()
    {
        animationState = AnimationState.ShowInitial;
        animationStateEndTime = Time.time + SHOW_INITIAL_DURATION;

        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (animationState)
        {
        case AnimationState.ShowInitial:
            if (Time.time >= animationStateEndTime)
            {
                animationState = AnimationState.Hiding;
                animationStateEndTime = Time.time + FLIP_ANIMATION_DURATION;
                Console.WriteLine("Beginning flip animation");
            }
            break;
        case AnimationState.Hiding:
            if (Time.time >= animationStateEndTime)
            {
                animationState = AnimationState.Hidden;
                Console.WriteLine("Ending flip animation. Card is now hidden");
            }
            break;
        case AnimationState.Showing:
            if (Time.time >= animationStateEndTime)
            {
                animationState = AnimationState.Visible;
                Console.WriteLine("Ending flip animation. Card is now visible");
            }
            break;
        case AnimationState.Hidden:
        case AnimationState.Visible:
            break;
        default:
            throw new InvalidOperationException();
        }

        if (sprite == null) return;

        float showAmt = (Time.time - animationStateEndTime + FLIP_ANIMATION_DURATION) / FLIP_ANIMATION_DURATION;
        switch (animationState)
        {
        case AnimationState.ShowInitial:
        case AnimationState.Visible:
            sprite.sprite = frontface_sprites[value];
            sprite.transform.localScale = new Vector3(1, 1);
            break;

        case AnimationState.Hidden:
            sprite.sprite = backface_sprite;
            sprite.transform.localScale = new Vector3(1, 1);
            break;

        case AnimationState.Hiding:
            showAmt = 1 - showAmt;
            goto case AnimationState.Showing;
        case AnimationState.Showing:
            if (showAmt < .5) sprite.sprite = backface_sprite;
            else sprite.sprite = frontface_sprites[value];
            sprite.transform.localScale = new Vector3((float)Math.Abs(2 * (showAmt - .5)), 1);
            break;

        default:
            throw new InvalidOperationException();
        }
    }

    private void OnMouseDown()
    {
        if (animationState == AnimationState.Hidden)
        {
            animationState = AnimationState.Showing;
            animationStateEndTime = Time.time + FLIP_ANIMATION_DURATION;
        }
        else if (animationState == AnimationState.Visible)
        {
            animationState = AnimationState.Hiding;
            animationStateEndTime = Time.time + FLIP_ANIMATION_DURATION;
        }
    }

    private enum AnimationState
    {
        ShowInitial,
        Hiding,
        Hidden,
        Showing,
        Visible,
        Accepting
    }
}
