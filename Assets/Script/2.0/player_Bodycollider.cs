using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Bodycollider : MonoBehaviour
{
    public delegate void GameOver();
    public static event GameOver gameOver;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("wall"))
        {
            if (gameOver != null)
                gameOver();
        }
    }
}
