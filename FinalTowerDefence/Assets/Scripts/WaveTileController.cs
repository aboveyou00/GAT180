using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveTileController : MonoBehaviour
{
    public Wave wave;
    public Text text;

    void Start()
    {
        text.text = "Wave " + (wave.index + 1) + "\r\n" + wave.count + " Enemies";
    }
}
