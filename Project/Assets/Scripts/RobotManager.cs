using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour {
    private GameObject robot2Spawn;

    public GameObject[] possibleITargets; //targets for the investigation robot
    public bool iRobotSelected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void SelectRobot(GameObject robot)
    {
        if (GetComponent<EnergyManager>().GetCount() > robot.GetComponent<Robot>().cost) //si tens suficient diners
        {
            robot2Spawn = robot;
            if (robot2Spawn.name == "InvestigationRobot")
            {
                iRobotSelected = true;
            }
        }        
    }
    public void SpawnRobot (Transform t, Vector3 o)
    {
        GameObject newRobot = Instantiate(robot2Spawn, Vector3.zero ,Quaternion.identity);
        if (robot2Spawn.name == "InvestigationRobot")
        {
            newRobot.GetComponent<Robot>().target = t;
            iRobotSelected = false;
        }

        
        
    }
}
