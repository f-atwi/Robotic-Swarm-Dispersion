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

    List<GameObject> BronkerboshRec(List<GameObject> R, List<GameObject> P, List<GameObject> X)
    {            //To be fixed, not all branches returns values

        if (P.Count == 0 && X.Count == 0)
            return R;

        int v_local_index;
        foreach (GameObject v in P)
        {
            v_local_index = Neighbors.BinarySearch(v);

            List<GameObject> nextR = new List<GameObject>(R);
            if (!nextR.Contains(v))
                nextR.Add(v);

            List<GameObject> nextP = new List<GameObject>();               //Creating P inter N(v) and X inter N(v)
            List<GameObject> nextX = new List<GameObject>();
            foreach (Robot rob in NeighborsOfNeighbors[v_local_index])
            {
                if (P.Contains(rob))
                    nextP.Add(rob);
                if (X.Contains(rob))
                    nextX.Add(rob);
            }
            BronkerboshRec(nextR, nextP, nextX);
            P.Remove(v);
            if (!X.Contains(v))
                X.Add(v);
        }
    }

    List<GameObject> findMaxClique()
    {
        return BronkerboshRec(new List<GameObject>(), new List<GameObject>(Neighbors), new List<GameObject>());
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
