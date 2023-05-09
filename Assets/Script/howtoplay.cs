using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class howtoplay : MonoBehaviour
{
    public GameObject img;
    public GameObject options;
    public void HowToButton()
    {
        if(img.activeSelf) img.SetActive(false);
        else
            img.SetActive(true);
    }

    public void ico()
    {
        if (options.activeSelf) options.SetActive(false);
        else
            options.SetActive(true);
    }
    public void ico_button0()
    {
        PlayerPrefs.SetInt("ico", 0);
    }
    public void ico_button1()
    {
        PlayerPrefs.SetInt("ico", 1);
    }
    public void ico_button2()
    {
        PlayerPrefs.SetInt("ico", 2);
    }
    public void ico_button3()
    {
        PlayerPrefs.SetInt("ico", 3);
    }

}
