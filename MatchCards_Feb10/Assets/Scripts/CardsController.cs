using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extension;
using Random = UnityEngine.Random;

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
            }
        }
    }
}
