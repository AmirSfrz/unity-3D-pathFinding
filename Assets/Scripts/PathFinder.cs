using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    public static Vector3 destination;
    public static Vector3 player;
    public int maxNumberOfCubesPerFrame = 30;
    public Dictionary<Vector3, PathNode> nodes;

    PathNode currentNode = new PathNode();

    bool foundDestination = false;

    //for log
    public List<Vector3> generatedPath;
    public float totalDistance;
    private void Awake()
    {
        destination = GameObject.FindGameObjectWithTag("Destination").transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform.position;

        destination.x = Mathf.Floor(destination.x);
        destination.y = Mathf.Floor(destination.y);
        destination.z = Mathf.Floor(destination.z);

        player.x = Mathf.Floor(player.x);
        player.y = Mathf.Floor(player.y);
        player.z = Mathf.Floor(player.z);

        //Debug.LogError("dest: " + destination.x + "," + destination.y + "," + destination.z);

        StartCoroutine(CalculatePath());
    }

    public IEnumerator CalculatePath()
    {
        nodes = new Dictionary<Vector3, PathNode>();
        int numberOfCalculatedCubes = maxNumberOfCubesPerFrame;

        //add player
        List<PathNode> startPath = new List<PathNode>();
        PathNode startPlayerNode = new PathNode(player, 0, startPath);
        nodes[player] = startPlayerNode;



        while (!foundDestination)
        {
            if (numberOfCalculatedCubes < 0)
            {
                numberOfCalculatedCubes = maxNumberOfCubesPerFrame;
                yield return null;
            }

            float minWeight = float.MaxValue;
            foreach (PathNode node in nodes.Values)
            {
                if (!node.alreadyChecked && node.weight < minWeight)
                {
                    minWeight = node.weight;
                    currentNode = node;
                }
            }
            currentNode.alreadyChecked = true;
            numberOfCalculatedCubes--;
            
            for (int x = -1; x <= 1 && !foundDestination; x++)
            {
                for (int y = -1; y <= 1 && !foundDestination; y++)
                {
                    for (int z = -1; z <= 1 && !foundDestination; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;
                        CheckNodeAvailabality(new Vector3(x, y, z));
                    }
                }
            }


        }

        DestinationFound();
    }

    public void DestinationFound()
    {
        totalDistance = currentNode.pathLenght;
        generatedPath = new List<Vector3>();
        PathNode node = currentNode;
        while (true)
        {
            generatedPath.Add(node.pos);
            if (node.path.Count != 1)
                node = node.path[node.path.Count - 2];
            else
                break;
        }
        //for (int i = generatedPath.Count-2; i >= 0; i--)
        //{
        //GameObject myLine = new GameObject();
        //myLine.transform.position = generatedPath[i+1];
        //myLine.AddComponent<LineRenderer>();
        //LineRenderer lr = myLine.GetComponent<LineRenderer>();
        ////lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        //lr.SetColors(Color.red, Color.red);
        //lr.SetWidth(0.1f, 0.1f);
        //lr.SetPosition(0, generatedPath[i + 1]);
        //lr.SetPosition(1, generatedPath[i]);
        //}

        //draw curved line
        GameObject line = new GameObject();
        line.name = "line";
        line.transform.position = destination;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.SetColors(Color.red, Color.red);
        lr.SetWidth(0.1f, 0.1f);
        line.AddComponent<CurvedLineRenderer>();
        for (int i = 0; i < generatedPath.Count; i++)
        {
            GameObject point = new GameObject();
            point.transform.position = generatedPath[i];
            point.transform.parent = line.transform;
            point.AddComponent<CurvedLinePoint>();
        }
        StartCoroutine(MovePlayer(line.GetComponent<CurvedLineRenderer>()));
        GameManager.instance.statusTxt.text = "path found, moving to target";
    }

    public IEnumerator MovePlayer(CurvedLineRenderer clr)
    {
        yield return null;
        PlayerMove pm = GameObject.FindGameObjectWithTag("Player").AddComponent<PlayerMove>();
        pm.Set(clr.smoothedPoints);
    }

    public void CheckNodeAvailabality(Vector3 offsetPos)
    {
        PathNode newNode = new PathNode();
        Vector3 pos = currentNode.pos + offsetPos;

        //calculating the added path distance
        float addedPathLenghtDistance = 1;
        int changedOffset = 0;
        if (offsetPos.x != 0)
            changedOffset++;
        if (offsetPos.y != 0)
            changedOffset++;
        if (offsetPos.z != 0)
            changedOffset++;

        if (changedOffset == 1)
            addedPathLenghtDistance = 1;
        else if (changedOffset == 2)
            addedPathLenghtDistance = 1.4f;
        else if (changedOffset == 3)
            addedPathLenghtDistance = 1.7f;


        //check if place is eighther existed or is obstacle
        if (GameManager.instance.blocks.ContainsKey(pos) && GameManager.instance.blocks[pos] == GameManager.BlockType.obstacle)
            return;

        newNode = new PathNode(pos, currentNode.pathLenght + addedPathLenghtDistance, currentNode.path);

        if (nodes.ContainsKey(pos) && nodes[pos].weight <= newNode.weight)
            return;
        else
            nodes[pos] = newNode;

        if (newNode.pos == destination)
        {
            foundDestination = true;
            currentNode = newNode;
        }
    }

    public class PathNode
    {
        public Vector3 pos;
        public float dist;
        public float weight;
        public float pathLenght;
        public List<PathNode> path;
        public bool alreadyChecked = false;
        public PathNode(Vector3 pos, float pathLenght, List<PathNode> path)
        {
            dist = Vector3.Distance(pos, destination);
            this.pos = pos;
            this.pathLenght = pathLenght;
            this.path = new List<PathNode>();
            foreach (var node in path)
            {
                this.path.Add(node);
            }
            this.path.Add(this);
            this.weight = (dist/**10*/) + pathLenght;
        }
        public PathNode() { }
    }
}

