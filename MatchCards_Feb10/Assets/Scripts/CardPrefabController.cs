using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CardEventHandler(CardPrefabController c);

public class CardPrefabController : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Sprite[] frontface_sprites;
    public Sprite backface_sprite;
    public int value;

    public AnimationState animationState;
    private float animationStateEndTime = 0;
    public const float SHOW_INITIAL_DURATION = 2;
    public const float FLIP_ANIMATION_DURATION = .2f;
    public const float SPIN_ANIMATION_DURATION = .4f;

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
            if (Time.time >= animationStateEndTime) FlipShut();
            break;

        case AnimationState.Hiding:
            if (Time.time >= animationStateEndTime) endAnimation(AnimationState.Hidden);
            break;
        case AnimationState.Showing:
            if (Time.time >= animationStateEndTime) endAnimation(AnimationState.Visible);
            break;

        case AnimationState.Hidden:
        case AnimationState.Visible:
            break;

        case AnimationState.Destroying:
            if (Time.time >= animationStateEndTime)
            {
                endAnimation(AnimationState.Destroyed);
                Destroy(gameObject);
                return;
            }
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

        case AnimationState.Destroying:
            sprite.sprite = frontface_sprites[value];
            showAmt = (animationStateEndTime - Time.time) / SPIN_ANIMATION_DURATION;
            sprite.transform.localScale = new Vector3(1, 1) * showAmt;
            sprite.transform.localRotation = Quaternion.AngleAxis((1 - showAmt) * 180 * 2, Vector3.forward);
            break;

        default:
            throw new InvalidOperationException();
        }
    }

    private void OnMouseDown()
    {
        var handler = Clicked;
        if (handler != null) handler.Invoke(this);
    }
    private void endAnimation(AnimationState state)
    {
        animationState = state;
        var handler = AnimationCompleted;
        if (handler != null) handler.Invoke(this);
    }
    public void SpinDestroy()
    {
        if (animationState != AnimationState.Visible) throw new InvalidOperationException();
        animationState = AnimationState.Destroying;
        animationStateEndTime = Time.time + SPIN_ANIMATION_DURATION;
    }
    public void FlipOpen()
    {
        if (animationState != AnimationState.Hidden) throw new InvalidOperationException();
        animationState = AnimationState.Showing;
        animationStateEndTime = Time.time + FLIP_ANIMATION_DURATION;
    }
    public void FlipShut()
    {
        if (animationState != AnimationState.Visible && animationState != AnimationState.ShowInitial) throw new InvalidOperationException();
        animationState = AnimationState.Hiding;
        animationStateEndTime = Time.time + FLIP_ANIMATION_DURATION;
    }

    public event CardEventHandler Clicked;
    public event CardEventHandler AnimationCompleted;

    public enum AnimationState
    {
        ShowInitial,
        Hiding,
        Hidden,
        Showing,
        Visible,
        Accepting,

        Destroying,
        Destroyed
    }
}
