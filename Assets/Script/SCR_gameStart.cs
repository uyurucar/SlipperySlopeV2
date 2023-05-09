using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SCR_gameStart : MonoBehaviour
{
    public Text highscoreText;
    void Start()
    {
        float highscore = PlayerPrefs.GetFloat("highscore", 0);
        highscoreText.text = "HIGHSCORE: " + highscore;
    }
    public void gameStart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
