using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior:MonoBehaviour
{
	private float oldIntensity = -1f;
	private bool seekAttempted = false;
	private Robot robot;
	private Graph graph;

	private void Start()
	{
		robot = GetComponent<Robot>();
		graph = this.gameObject.AddComponent<Graph>();
		StartCoroutine(main());
	}
	private void disperse() {
		if (oldIntensity > graph.getSentryIntensity() || oldIntensity == -1) {
			robot.Linear_velocity = 10f;
			robot.Angular_velocity = 0;
		}
		else {
			robot.Linear_velocity = 2f;
			robot.Angular_velocity = 10f;
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
			robot.Angular_velocity = 30f;
			seekAttempted = false;
		}
		else
		{
			seekAttempted = true;
		}
	}

	private void avoidCollision()
	{
		robot.Linear_velocity = 0;
		robot.Angular_velocity = 30;
	}
	
	private void guard() {
		robot.Angular_velocity = 0;
		robot.Linear_velocity = 0;
	}

	IEnumerator main()
	{
		while (true)
		{
			if (robot.wallDetection())
			{
				avoidCollision();
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
				guard();
			}

		}
		yield return null;
	}
	
    
}
