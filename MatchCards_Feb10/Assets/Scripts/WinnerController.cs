using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerController : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("TitleScene");
    }

    private void OnGUI()
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        int left = (screenWidth / 2) - 100;
        int top = (screenHeight / 2) + 200;
        int width = 200, height = 60;
        if (GUI.Button(new Rect(left, top, width, height), "Tap to Continue"))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}
