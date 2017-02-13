using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class CardsController : MonoBehaviour
{
    public float cardSize = 1.5f;
    public TimerController timer;
    public GameObject cardPrefab;

    private CardPrefabController[][] cards;

    void Start()
    {
        if (cardPrefab == null) throw new InvalidOperationException();

        List<int> values = new List<int>();
        const int totalCount = (4 * 6);
        for (int q = 0; q < totalCount / 2; q++)
        {
            int value = -1;
            while (value == -1 || values.Contains(value))
                value = Random.Range(0, 24);
            values.Add(value);
        }
        for (int q = 0; q < totalCount / 2; q++)
        {
            values.Add(values[q]);
        }
        values.Shuffle();

        int nextValueIdx = 0;
        cards = new CardPrefabController[4][];
        for (int q = 0; q < cards.Length; q++)
        {
            cards[q] = new CardPrefabController[6];
            for (int w = 0; w < cards[q].Length; w++)
            {
                var instance = Instantiate(cardPrefab);
                if (instance == null) throw new InvalidOperationException();
                instance.transform.position = new Vector3((-1.5f + q) * cardSize, (-2.5f + w) * cardSize, 0);

                var cc = instance.GetComponent<CardPrefabController>();
                if (cc == null) throw new InvalidOperationException();
                cc.value = values[nextValueIdx++];

                cc.Clicked += onCardClicked;
                cc.AnimationCompleted += onCardAnimationCompleted;

                cards[q][w] = cc;
            }
        }

        if (timer != null) timer.TimeRanOut += onTimeRanOut;
    }

    private bool animationPlaying = false;
    private CardPrefabController firstCard;
    private CardPrefabController secondCard;
    private void onCardClicked(CardPrefabController c)
    {
        if (timer.IsOutOfTime) return;
        if (animationPlaying) return;
        if (firstCard == c) return;
        if (firstCard == null) firstCard = c;
        else secondCard = c;
        c.FlipOpen();
        animationPlaying = true;
    }
    private void onCardAnimationCompleted(CardPrefabController c)
    {
        animationPlaying = false;
        if (firstCard != c && secondCard != c)
        {
            for (int q = 0; q < cards.Length; q++)
            {
                for (int w = 0; w < cards[q].Length; w++)
                {
                    if (cards[q][w] != null && cards[q][w].gameObject != null && cards[q][w].animationState != CardPrefabController.AnimationState.Destroyed) return;
                }
            }
            SceneManager.LoadScene("WinnerScene");
        }

        if (firstCard != null && secondCard != null && secondCard == c)
        {
            if (firstCard.value == secondCard.value)
            {
                firstCard.SpinDestroy();
                secondCard.SpinDestroy();
            }
            else
            {
                animationPlaying = true;
                firstCard.FlipShut();
                secondCard.FlipShut();
            }
            firstCard = secondCard = null;
        }
    }

    private void onTimeRanOut(TimerController t)
    {
        SceneManager.LoadScene("LoserScene");
    }
}
