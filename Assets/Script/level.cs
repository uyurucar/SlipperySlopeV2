using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level : MonoBehaviour
{
    public int levl=1;
    public double score=0;
    // Start is called before the first frame update
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        
    }
    public void newLevel(double scor)
    {
        score = scor;
        levl++;
        if (levl >= 4) levl = 4;
    }
    public int getLevel() { return levl; }
    public double getScore()
    {
        return score;
    }
    public void reset()
    {
        levl = 1;
        score = 0;
    }
}
