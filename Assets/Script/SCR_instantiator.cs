using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_instantiator : MonoBehaviour
{

    //instantiate position values for wall: -0.06 or 0.04 , 4.0958 , Zvalue
    //instantiate position values for cubes: between -0.17 and 0.15 , 4.080654, Zvalue
    // Zvalue incremential range 0.3 to 1 
    private float zValue = 3f;
    private IEnumerator coroutine;
    private int level;
    private int enumRange; 
    [SerializeField] private GameObject greenCube;
    [SerializeField] private GameObject yellowCube;
    [SerializeField] private GameObject blueCube;
    [SerializeField] private GameObject purpleCube;
    [SerializeField] private GameObject redCube;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject upperwall0;
    [SerializeField] private GameObject upperwall1;
    [SerializeField] private GameObject upperwall2;
    [SerializeField] private float instantiateInterval = 0.05f; //instantiate interval to reduce object load
    enum Type {yellowC,wallC1, greenC, blueC, wallC2, purpleC, redC, wallC3, wallC4, wallC5, wallC6, upper1, upper2, upper3, upper4, upper5, upper6}
    Type type;
    bool isWallRight = true;
    void Start()
    {
        coroutine = buildMap();
        StartCoroutine(coroutine);
        level = GameObject.Find("Level").GetComponent<level>().getLevel();
        enumRange = 12 + level;
    }

    private IEnumerator buildMap()
    {
        while(zValue < 45f)
        {
            type = (Type)Random.Range(0, enumRange);
            zValue += Random.Range(0.3f, 1f);
            if (type == Type.wallC1 || type == Type.wallC2 || type == Type.wallC3 || type == Type.wallC4 || type == Type.wallC5 || type == Type.wallC6)
            {
                //instantiate position values for wall: -0.06 or 0.04 , 4.0958 , Zvalue
                if(isWallRight)
                {
                    Instantiate(wall, new Vector3(0.04f, 4.0958f, zValue), Quaternion.Euler(0,0,0));
                    isWallRight = false;
                }
                else
                {
                    Instantiate(wall, new Vector3(-0.06f, 4.0958f, zValue), Quaternion.Euler(0, 0, 0));
                    isWallRight = true;
                }

            }
            else if(type == Type.upper1 || type == Type.upper2 || type == Type.upper3 || type == Type.upper4 || type == Type.upper5 || type == Type.upper6)
            {

                int height = Random.Range(0, 3);
                float x_coor;
                if(isWallRight) { x_coor = 0.072f; isWallRight = false; } else { x_coor = -0.03f; isWallRight = true; }
                if(height == 0)
                {
                    Instantiate(upperwall0, new Vector3(x_coor, 4.33f, zValue), Quaternion.Euler(0, 0, 0));
                }
                else if (height == 1)
                {
                    Instantiate(upperwall1, new Vector3(x_coor, 4.42f, zValue), Quaternion.Euler(0, 0, 0));
                }
                else if (height == 2)
                {
                    Instantiate(upperwall2, new Vector3(x_coor, 4.46f, zValue), Quaternion.Euler(0, 0, 0));
                }
            } 
            else
            {

                //instantiate position values for cubes: between -0.17 and 0.15 , 4.080654, Zvalue
                if (type == Type.yellowC)
                {
                    Instantiate(yellowCube, new Vector3(Random.Range(-0.17f,0.15f), 4.08f,zValue), Quaternion.Euler(0,0,0));
                }
                else if (type == Type.greenC)
                {
                    Instantiate(greenCube, new Vector3(Random.Range(-0.17f, 0.15f), 4.08f, zValue), Quaternion.Euler(0, 0, 0));
                }
                else if (type == Type.blueC)
                {
                    Instantiate(blueCube, new Vector3(Random.Range(-0.17f, 0.15f), 4.08f, zValue), Quaternion.Euler(0, 0, 0));
                }
                else if (type == Type.purpleC)
                {
                    Instantiate(purpleCube, new Vector3(Random.Range(-0.17f, 0.15f), 4.08f, zValue), Quaternion.Euler(0, 0, 0));
                }
                else if (type == Type.redC)
                {
                    Instantiate(redCube, new Vector3(Random.Range(-0.17f, 0.15f), 4.08f, zValue), Quaternion.Euler(0, 0, 0));
                }
            }
            yield return new WaitForSeconds(instantiateInterval);
        }
    }
}
