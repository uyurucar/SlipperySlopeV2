using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public GameObject env;
    public GameObject cam;
    public float speed;
    public float angle;
    bool coulock;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    { 
        coulock = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(true);
        if(other.tag.Equals("Player") || other.tag.Equals("attachedCube") )
        {
            if(!coulock)
            {
                transform.position = new Vector3(player.transform.position.x-0.08f, player.transform.position.y + 0.3f, transform.position.z);
                Destroy(GetComponent<BoxCollider>());
                transform.parent = player.transform;
                StartCoroutine(Rotate());
                coulock = true;
            }

        }
    }
    IEnumerator Rotate()
    {
        float amount = 0;
        while (amount <180f)
        {
            
            angle += Time.deltaTime * speed;
            env.transform.rotation = Quaternion.Euler(env.transform.rotation.eulerAngles.x, env.transform.rotation.eulerAngles.y, angle);
            cam.transform.rotation = Quaternion.Euler(10.797f, -5.777f, angle);
            amount += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(8f);

        amount = 0;
        while (amount < 180f)
        {

            angle += Time.deltaTime * speed;
            env.transform.rotation = Quaternion.Euler(env.transform.rotation.eulerAngles.x, env.transform.rotation.eulerAngles.y, angle);
            cam.transform.rotation = Quaternion.Euler(10.797f, -5.777f, angle);
            amount += Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        transform.parent = null;
        Destroy(this.gameObject, 3f);
    }
}
