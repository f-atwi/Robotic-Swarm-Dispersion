using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    int nbOfRobots;                  //Total number of robots in the experience
    GameObject[] robots;             //Array containing all the robots
    float[] distances;               //Array containing distances from self to every other robot, ordered the same way as robots[] (ie distances[k] is the distance from self to robots[k]
    bool[] areNeighbor;              //Boolean array, areNeighbor[k] is true iff robots[k] is a neighbor of self 
    List<GameObject> Neighbors;      //List of neighbors of self
    List<List<GameObject>> NeighborsOfNeighbors;     //List of list containing the neighbors of every neighbor of self, ordered the same way as Neighbors (the first element of NeighborsOfNeighbors is the list of the neibhbors of the first element of Neighbors
    float neighborDistanceThreshold = 5f;    //Distance required to be considered neighbor
    GameObject localSentry;          //Current sentry of self
    List<List<GameObject>> Cliques;

    // Start is called before the first frame update
    void Start()
    {
        robots = GameObject.FindGameObjectsWithTag("robot");
        if (robots.Length == 0)
            Debug.Log("No robots detected in Graph.Start()");

        nbOfRobots = robots.GetLength(0);
        distances = new float[nbOfRobots];

        areNeighbor = new bool[nbOfRobots];
        for (int k = 0; k < areNeighbor.Length; k++)
            areNeighbor[k] = false;
        Neighbors = new List<GameObject>();
        NeighborsOfNeighbors = new List<List<GameObject>>();
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


    public void updateConnectivityGraph()
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
        NeighborsOfNeighbors = curNeighborsOfNeighbors;

    }


    void BronkerboshRec(List<GameObject> R, List<GameObject> P, List<GameObject> X)
    {            //To be fixed, not all branches returns values

        if (P.Count == 0 && X.Count == 0)
            Cliques.Add(R);

        int v_local_index;
        foreach (GameObject v in P)
        {
            v_local_index = Neighbors.BinarySearch(v);

            List<GameObject> nextR = new List<GameObject>(R);
            if (!nextR.Contains(v))
                nextR.Add(v);

            List<GameObject> nextP = new List<GameObject>();               //Creating P inter N(v) and X inter N(v)
            List<GameObject> nextX = new List<GameObject>();
            foreach (GameObject rob in NeighborsOfNeighbors[v_local_index])
            {
                if (P.Contains((GameObject)rob))
                    nextP.Add((GameObject)rob);
                if (X.Contains((GameObject)rob))
                    nextX.Add((GameObject)rob);
            }
            BronkerboshRec(nextR, nextP, nextX);
            P.Remove(v);
            if (!X.Contains(v))
                X.Add(v);
        }
    }

    List<List<GameObject>> findMaxClique()
    {
        Cliques = new List<List<GameObject>>();
        BronkerboshRec(new List<GameObject>(), new List<GameObject>(Neighbors), new List<GameObject>());
        return Cliques;
    }

    public bool isSentry()
    {
        if(localSentry is null)
        {
            return false;
        }
        if (localSentry.GetInstanceID() == this.transform.parent.gameObject.GetInstanceID())
            return true;
        else
            return false;
    }

    public int getNumberOfNeighbors()
    {
        return Neighbors.Count;
    }

    public float getSentryIntensity()
    {
        if (localSentry is null)
            return 0;
        float dist = getDistance(localSentry);
        return 1 / (dist * dist);
    }
}