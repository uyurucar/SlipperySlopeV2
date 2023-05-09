using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;
using System.Net.Http.Headers;

public class SCR_player : MonoBehaviour
{
    [Header("var")]
    [SerializeField] private float speed = 1;  //player speed
    [SerializeField] private float sensitivity = 1f; //touch sensitivity
    [SerializeField] private TMP_Text timeText;   
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private Animator anim;
    [SerializeField] private Animator big_anim;
    public Material[] cube_materials;
    private Material cube_mat;
    public TMP_Text messageBox;
    public GameObject building1;
    public GameObject building2;
    private AudioSource source;
    [Header("Audio & Image")]
    public AudioClip newBoxSound;
    public AudioClip finishSound;
    public AudioClip boing;
    public GameObject trail;
    public GameObject ready_text;
    public GameObject go_text;
    private bool isPlayerCompleted = false;  
    private float PLAYER_HEIGHT = 4.0589F; 
    private bool start = false;
    private int levl;
    private bool isGameOver = false;
    private int score = 0;
    private double finalscore;
    private float intime = 0;
    private float realtime = 0;
    private float CUBE_HEIGHT = 0.044f;
    private Touch touch;
    private float lastFingerPos;
    public ParticleSystem particles;
    public Camera cam;
    private int cubeCount = 1;
    private Vector3 beginPosition;
    private float touchXdiff = 0;
    private void OnEnable()
    {
        player_Bodycollider.gameOver += _upperHitNotification;
    }

    private void OnDisable()
    {
        player_Bodycollider.gameOver -= _upperHitNotification;
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.FindGameObjectWithTag("audio").GetComponent<AudioSource>();
        particles.Play();
        finalscore = GameObject.Find("Level").GetComponent<level>().getScore();
        levl = GameObject.Find("Level").GetComponent<level>().getLevel();
        cube_mat = cube_materials[PlayerPrefs.GetInt("ico", 0)];
        GameObject.FindGameObjectWithTag("attachedCube").GetComponent<MeshRenderer>().material = cube_mat;
    }

    // Update is called once per frame
    void Update()
    {
        setTrail();
        if(!start && Time.timeScale != 0)
        {
            intime += Time.deltaTime; 
            timeText.text = "READY: " + System.Math.Round(3f - intime,2);
            distanceText.text = "LEVEL " + levl;
            if (intime > 3f)
            {
                start = true;
                ready_text.SetActive(false);
                go_text.SetActive(true);
                Destroy(go_text, 5f);
            }
        }
        else if (start && !isGameOver && Time.timeScale != 0 && !isPlayerCompleted)
        {
            transform.position += new Vector3(0, 0, Time.deltaTime * speed);  //player going forward
            if (!isGameOver || isPlayerCompleted)
            {
                realtime += Time.deltaTime;
                distanceText.text = "DISTANCE: " + System.Math.Round(transform.position.z - 3f, 2);  
            }
            else realtime -= Time.deltaTime;
            timeText.text = "TIME: " + (int)realtime;
            
            intime += Time.deltaTime;
            handleInput();
            
        }
        if(cubeCount == 0 && isGameOver == false && isPlayerCompleted == false && Time.timeScale != 0)
        {
            realtime = 7f;
            isGameOver = true;
            anim.SetBool("die", true);
            big_anim.SetBool("die", true);
            transform.position -= new Vector3(0, 0, 0.05f);
            StartCoroutine(GameOver());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "cube" && intime >= 0.1f)
        {
            intime = 0;
            other.gameObject.GetComponent<SCR_cube>().attach();
            this.transform.position = new Vector3(transform.position.x, PLAYER_HEIGHT + CUBE_HEIGHT * cubeCount, transform.position.z);
            cubeCount += 1;
            this.transform.position += new Vector3(0, CUBE_HEIGHT, 0);
            other.gameObject.transform.SetParent(this.transform);
            other.gameObject.transform.position = new Vector3(transform.position.x, other.gameObject.transform.position.y, transform.position.z);
            cam.transform.position -= new Vector3(0, 0, 0.07f);
            source.PlayOneShot(newBoxSound);
            Material cub_material = other.gameObject.GetComponent<MeshRenderer>().materials[0];
            other.gameObject.GetComponent<MeshRenderer>().material = cube_mat;
            building1.GetComponent<MeshRenderer>().materials[1].color = cub_material.color;
            building2.GetComponent<MeshRenderer>().materials[1].color = cub_material.color;
        }
        if(other.gameObject.tag == "finish" && isPlayerCompleted==false)
        {
            isPlayerCompleted = true;
            StartCoroutine(PlayerFinished());
        }
        if(other.gameObject.tag == "upperwall")
        {
            realtime = 7f;
            isGameOver = true;
            anim.SetBool("hit", true);
            big_anim.SetBool("hit", true);
            StartCoroutine(GameOver());
            source.PlayOneShot(boing);
        }
        
    }
    void handleInput()
    {
        #if UNITY_EDITOR
        Vector3 mouseLocalPosition = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.position = new Vector3(mouseLocalPosition.x-0.5f, transform.position.y, transform.position.z);
        if (transform.position.x > 0.15f) transform.position = new Vector3(0.15f, transform.position.y, transform.position.z); //platform border lock
        else if (transform.position.x < -0.17f) transform.position = new Vector3(-0.17f, transform.position.y, transform.position.z); //platform border lock
        if (Input.GetMouseButtonUp(0)) dropCube();
        #else

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            //Debug.Log(touch.phase);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touch.position;
                beginPosition = Camera.main.ScreenToViewportPoint(new Vector3(touchPosition.x, touchPosition.y, 0));   //adapts touch position value between [0,1] float
                //first touch position is saved to calculate movement
                lastFingerPos = (beginPosition.x - 0.5f) * sensitivity;   //localposition [0,1] is shifted to [-0.5,0.5] for calculation reasons.
            }
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = touch.position;
                Vector3 localPosition = Camera.main.ScreenToViewportPoint(new Vector3(touchPosition.x, touchPosition.y, 0)); //adapts touch position value between [0,1] float
                float horizontalPos = (localPosition.x - 0.5f) * sensitivity;
                if (horizontalPos > 0.15f) horizontalPos = 0.15f; //platform border lock
                else if (horizontalPos < -0.17f) horizontalPos = -0.17f; //platform border lock
                transform.position = new Vector3(horizontalPos, transform.position.y, transform.position.z);
                if (Mathf.Abs(localPosition.x - beginPosition.x) > touchXdiff) touchXdiff = Mathf.Abs(localPosition.x - beginPosition.x);

            }
            if(touch.phase == TouchPhase.Ended)
            {
                //input for dropping cubes
                Vector2 touchPosition = touch.position;
                Vector3 endPosition = Camera.main.ScreenToViewportPoint(new Vector3(touch.position.x, touch.position.y, 0));
                if (beginPosition.y - endPosition.y > 0.10 && touchXdiff < 0.3) dropCube();
                touchXdiff = 0; 
            }
        }
        #endif
    }
    void setTrail()
    {
        trail.transform.position = new Vector3(transform.position.x, trail.transform.position.y, transform.position.z);
    }
    public void cubeIsDown()
    {
        cubeCount -= 1;
        StartCoroutine(cubeDown()); //recalculate player y value 
        cam.transform.position += new Vector3(0, 0, 0.07f);
    }
    IEnumerator cubeDown()
    {
        float zValue = transform.position.z;

        while (transform.position.z - zValue < 0.05)
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(0.1f);
        }
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //this.transform.position = new Vector3(transform.position.x, PLAYER_HEIGHT + CUBE_HEIGHT * cubeCount, transform.position.z);
    }
    public void cubeFinished()
    {
        score += 5;  //cubes you finish with is added to the score 
        cubeCount -= 1;
        cam.transform.position += new Vector3(0, 0, 0.07f);
    }
    public void gameStart()
    {
        start = true;
    }
    public IEnumerator GameOver()
    {
        finalscore += System.Math.Round(transform.position.z - 3f + score, 2);
        double lastHighscore = PlayerPrefs.GetFloat("highscore", 0);  
        if(lastHighscore < finalscore)
        {
            messageBox.text = "GAME OVER.\nNEW HIGHSCORE: " + finalscore; 
            PlayerPrefs.SetFloat("highscore",System.Convert.ToSingle(finalscore));
        }
        else
        {
            messageBox.text = "GAME OVER.\nSCORE: " + finalscore;
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("FirstScreen");
    }
    public IEnumerator PlayerFinished() //player crossed finish line
    {
        source.PlayOneShot(finishSound);
        anim.SetBool("victory", true);
        big_anim.SetBool("victory", true);
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("attachedCube");
        score += 5* cubes.Length;  //cubes you finish with is added to the score 
        cubeCount -= 1* cubes.Length;
        cam.transform.position += new Vector3(0, 0, 0.07f* cubes.Length);
        for (int i = 0; i < cubes.Length; i++)
        {
            Destroy(cubes[i].gameObject);
        }
        transform.position = new Vector3(transform.position.x, PLAYER_HEIGHT, transform.position.z);
        finalscore += System.Math.Round(transform.position.z - 3f + score, 2);
        GameObject.Find("Level").GetComponent<level>().newLevel(finalscore);
        messageBox.text = "\n SCORE: " + finalscore;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainScene");  //load first screen
    }
    public void dropCube()
    {
        if (cubeCount == 1) return;
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("attachedCube");
        GameObject lowestCube = cubes[0];
        float lowestVal = cubes[0].gameObject.transform.position.y;
        for(int i =1; i<cubes.Length; i++)
        {
            if(lowestVal > cubes[i].gameObject.transform.position.y)
            {
                lowestVal = cubes[i].gameObject.transform.position.y;
                lowestCube = cubes[i];
            }
        }
        lowestCube.GetComponent<SCR_cube>().detach();
        cubeCount -= 1;
        //this.transform.position = new Vector3(transform.position.x, PLAYER_HEIGHT + CUBE_HEIGHT * cubeCount, transform.position.z);
        cam.transform.position += new Vector3(0, 0, 0.07f);
    }
    public void pause_resume()
    {
        if (Time.timeScale == 0)
        {
            messageBox.text = "";
            Time.timeScale = 1;
        }
        else
        {
            messageBox.text = "PAUSED";
            Time.timeScale = 0;
        }
    }
    void _upperHitNotification()
    {
        if (!isGameOver)
        {
            realtime = 7f;
            isGameOver = true;
            anim.SetBool("hit", true);
            big_anim.SetBool("hit", true);
            StartCoroutine(GameOver());
            source.PlayOneShot(boing);
        }
    }
    public void restart()  //button
    {
        GameObject.Find("Level").GetComponent<level>().reset();
        SceneManager.LoadScene("MainScene");
    }
    public void home() //button
    {
        GameObject.Find("Level").GetComponent<level>().reset();
        SceneManager.LoadScene("FirstScreen");
    }
}
