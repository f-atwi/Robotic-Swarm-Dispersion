using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior:MonoBehaviour
{
	private float oldIntensity = -1f;
	private bool seekAttempted = false;
	private Robot robot;
	private Graph graph;

	public bool coroutine_running =false;

	private void Start()
	{
		robot = GetComponent<Robot>();
		graph = GetComponent<Graph>();
	}

    private void Update()
    {
		if (!coroutine_running)
		{
			coroutine_running = true;
			StartCoroutine(main());
		}
	}
	private void disperse() {
		if (oldIntensity > graph.getSentryIntensity() || oldIntensity == -1) {
			robot.Linear_velocity = 10f;
			robot.Angular_velocity = 0;
		}
		else {
			robot.Linear_velocity = 2f;
			robot.Angular_velocity = 10f *robot.sense;
		}
		oldIntensity = graph.getSentryIntensity();
	}
	
	private void seekConnection()
	{
		robot.Linear_velocity = 10f;
		robot.Angular_velocity = 0;

		if (graph.getNumberOfNeighbors() == 0 && seekAttempted)
		{
			robot.Linear_velocity = 2f;
			robot.Angular_velocity = 30f * robot.sense;
			seekAttempted = false;
		}
		else
		{
			seekAttempted = true;
		}
	}

	private void avoidCollision(int side)
	{
		robot.Linear_velocity = 0;
		if (side == 0)
		{
			robot.Angular_velocity = 60 * robot.sense;
		}
		else
		{
			robot.Angular_velocity = 60 * -side;
		}
	}
	
	private void guard() {
		robot.Angular_velocity = 0;
		robot.Linear_velocity = 0;
	}

	IEnumerator main()
	{

		graph.updateConnectivityGraph();

		graph.findMaxClique();
//		graph.chooseSentries();


		int? detection_side = robot.wallDetection();
		if ( !(detection_side is null))
		{
			avoidCollision((int)detection_side);
		}
		else if (graph.getNumberOfNeighbors() == 0)
		{
			seekConnection();
		}
		else if (!graph.isSentry())
		{
			disperse();
		}
		else
		{
			Debug.Log(this.gameObject + " is a sentry");
			guard();
		}

		coroutine_running = false;
		yield return null;
	}
	
    
}
