using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigableObj : MonoBehaviour {

    private RobotManager rB;

	// Use this for initialization
	void Start () {
        rB = Camera.main.GetComponent<RobotManager>();	
	}
	
	// Update is called once per frame
	void Update () {
		//pintar aura quan toqui
	}

    private void OnMouseDown()
    {
        print(rB.iRobotSelected);
      if(rB.iRobotSelected)
        {
            rB.SpawnRobot(transform, Vector3.zero);
        }
    }
}
