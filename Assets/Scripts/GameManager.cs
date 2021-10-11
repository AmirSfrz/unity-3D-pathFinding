using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cubePrefab;
    public static GameManager instance;
    public Dictionary<Vector3,BlockType> blocks;
    public GameObject[] obstacles;
    public GameObject player;
    public GameObject destination;
    public bool obstacleFoundInThisFrame = false;
    public bool askedForPathfinding = false;
    public float collissionUpdateDelay = 1;

    public Text statusTxt;
    // Start is called before the first frame update
    void Start()
    {
        statusTxt.text = "Finding Obstacles";
        instance = this;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        player = GameObject.FindGameObjectWithTag("Player");
        destination = GameObject.FindGameObjectWithTag("Destination");
        blocks = new Dictionary<Vector3, BlockType>();
        GetAllTheObstacles();
    }
    void GetAllTheObstacles() {
        foreach (GameObject obj in obstacles)
        {
            Vector3 pos = new Vector3((int)obj.transform.position.x, (int)obj.transform.position.y, (int)obj.transform.position.z);
            Instantiate(cubePrefab, pos, Quaternion.identity);
            blocks.Add(pos, BlockType.obstacle);
        }
    }

    public IEnumerator IsObstacle(GameObject obj, Vector3 pos)
    {
        obstacleFoundInThisFrame = true;
        yield return null;
        obstacleFoundInThisFrame = true;

        //blocks.Add(pos, BlockType.obstacle);
        blocks[pos] = BlockType.obstacle;
        //forward
        Vector3 measurePos = pos + Vector3.forward;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }

        //back
        measurePos = pos + Vector3.back;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }

        //up
        measurePos = pos + Vector3.up;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }

        //down
        measurePos = pos + Vector3.down;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }

        //right
        measurePos = pos + Vector3.right;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }

        //left
        measurePos = pos + Vector3.left;
        if (!blocks.ContainsKey(measurePos))
        {
            blocks.Add(measurePos, BlockType.empty);
            Instantiate(cubePrefab, measurePos, Quaternion.identity);
        }
        Destroy(obj);
    }

    private void LateUpdate()
    {
        collissionUpdateDelay -= Time.fixedDeltaTime;

        if (!obstacleFoundInThisFrame && !askedForPathfinding)
            statusTxt.text = "Waiting: " + collissionUpdateDelay;

        if (!obstacleFoundInThisFrame && !askedForPathfinding && collissionUpdateDelay < 0)
        {
            askedForPathfinding = true;
            gameObject.AddComponent<PathFinder>();
            statusTxt.text = "Finding Path";
        }
        else
            obstacleFoundInThisFrame = false;
    }

    public enum BlockType
    {
        empty,
        obstacle,
        player,
        destination
    }
}
