using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_cube : MonoBehaviour
{
   // bool isGoingRight = true;
    //bool attached = false;
    AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioSource>();
       //  if (transform.parent != null) attached = true;  
       //  else
       //     attached = false;
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if(!attached) //non-attached cubes go left and right 
        { 
            if(isGoingRight)
            {
                transform.position += new Vector3(speed, 0, 0);  
                if(transform.position.x > 0.15f)
                {
                    isGoingRight = false;
                }
            }
            else
            {
                transform.position -= new Vector3(speed, 0, 0);
                if (transform.position.x < -0.17f)
                {
                    isGoingRight = true;
                }
            }
            
        }
        else
        {
            if(transform.position.y < -1.7f)  //bug fix when some cubes go below ground undetacted
            {
                transform.parent.gameObject.GetComponent<SCR_player>().cubeIsDown();
                this.gameObject.tag = "detachedCube"; //ignore detached cubes on trigger
                transform.parent = null;
                Destroy(this, 0.667f);
            }  
        } 
    } */
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            source.PlayOneShot(clip);
            if (transform.parent != null)
            {
                transform.parent.gameObject.GetComponent<SCR_player>().cubeIsDown(); //nofity player that cube is detached
                this.gameObject.tag = "detachedCube"; //ignore detached cubes on trigger
            }
            transform.parent = null;
            this.GetComponent<BoxCollider>().enabled = false;
            Destroy(this.gameObject, 7f);  
        }
        
    }
    public void attach()
    {
        //attached = true;  //called from player, notify cube that it is attached to player
        this.gameObject.tag = "attachedCube";
    }
    public void detach()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        this.gameObject.tag = "detachedCube"; //ignore detached cubes on trigger
        this.GetComponent<BoxCollider>().enabled = false;
        Destroy(this.gameObject, 7f);
    }
}
