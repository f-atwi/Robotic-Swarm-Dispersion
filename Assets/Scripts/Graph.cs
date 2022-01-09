using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    int nbOfRobots;
    GameObject[] robots;
    float[] distances;
    bool[] areNeighbor;
    List<GameObject> Neighbors;

    float neighborDistanceThreshold = 5f;

    // Start is called before the first frame update
    void Start()
    {
        robots = GameObject.FindGameObjectsWithTag("robot");
        // TODO : identify self
        if (robots.Length == 0)
            Debug.Log("No robots detected in Graph.Start()");

        nbOfRobots = robots.GetLength(0);
        distances = new float[nbOfRobots];

        areNeighbor = new bool[nbOfRobots];
        for (int k = 0; k < areNeighbor.Length; k++)
            areNeighbor[k] = false;
        updateConnectivityGraph();
    }

    // Update is called once per frame
    void Update()
    {

    }

    float getDistance(GameObject distant_robot)
    {
        Vector3 position_diff = distant_robot.transform.position - this.transform.position;
        return position_diff.sqrMagnitude;
    }


    void updateConnectivityGraph()
    {
        List<GameObject> cur_neighbors = new List<GameObject>();
        List<List<GameObject>> curNeighborsOfNeighbors = new List<List<GameObject>>();
        for (int k = 0; k < nbOfRobots; k++)
        {
            distances[k] = getDistance(robots[k]);
            if (distances[k] < neighborDistanceThreshold)
            {
                areNeighbor[k] = true;
                cur_neighbors.Add(robots[k]);
            }
            else
                areNeighbor[k] = false;
        }

        foreach (GameObject neighbor in cur_neighbors)
        {
            curNeighborsOfNeighbors.Add(neighbor.GetComponent<Graph>().Neighbors);
        }
        Neighbors = cur_neighbors;
    }

    List<GameObject> BronKerbosch(List<GameObject> P_init)
    {
        Stack<List<GameObject>[]> S = new Stack<List<GameObject>[]>();
        List<GameObject>[] cur_layer = new List<GameObject>[3];
        List<GameObject> R = new List<GameObject>(); ;
        List<GameObject> P = new List<GameObject>(P_init);
        List<GameObject> X = new List<GameObject>();
        GameObject v;

        cur_layer[0] = R;
        cur_layer[1] = P;
        cur_layer[2] = X;
        S.Push(cur_layer);
        while (S.Count != 0)
        {
            cur_layer = S.Pop();
            R = cur_layer[0];
            P = cur_layer[1];
            X = cur_layer[2];
            if (P.Count == 0 && X.Count == 0)
                return R;
            if (P.Count != 0)
            {
                //not working
                v = P[0];
                P.RemoveAt(0);
                cur_layer[0] = R;
                cur_layer[1] = P;
                cur_layer[2] = X;
                S.Push(cur_layer);
            }
            //Some remaining code idk T.T
        }
        return R;
    }


    public bool isSentry()
    {
        //TODO
        return true;
    }

    public int getNumberOfNeighbors()
    {
        return Neighbors.Count;
    }

    public float getSentryIntensity()
    {
        //TODO
        return 2f;
    }
}
