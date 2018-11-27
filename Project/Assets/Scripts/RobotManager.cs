using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotManager : MonoBehaviour {

    [System.Serializable]
    public struct RobotHud
    {
        public string tagName;
        public Button robotButton;
        public int cost;
        public GameObject robot;
    }

    private GameObject robot2Spawn;

    public GameObject[] possibleITargets; //targets for the investigation robot
    public bool iRobotSelected;

    public List<RobotHud> robots = new List<RobotHud>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ButtonsOpacity();
    }

    public void SelectRobot(Button b)
    {
        if (b.GetComponent<Image>().color.a == 1) //si tens suficient diners
        {
            /*robot2Spawn = robot;
            if (robot2Spawn.name == "InvestigationRobot")
            {
                iRobotSelected = true;
            }*/
            foreach (RobotHud r in robots)
            {
                if (r.robotButton == b)
                {
                    Instantiate(r.robot, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);
                    Camera.main.GetComponent<EnergyManager>().energyCounter -= r.cost;
                }
            }            
        }
    }
    /*public void SpawnRobot (Transform t, Vector3 o)
    {
        GameObject newRobot = Instantiate(robot2Spawn, Vector3.zero ,Quaternion.identity);
        if (robot2Spawn.name == "InvestigationRobot")
        {
            newRobot.GetComponent<Robot>().target = t;
            iRobotSelected = false;
        }  
        
    }*/

    void ButtonsOpacity()
    {
        foreach (RobotHud rH in robots)
        {
            
            //si no tens la pasta no es pot fer spawn, que passsa si no hi ha l'amenaça
            if (rH.cost > GetComponent<EnergyManager>().GetCount())
            {
                Color color = rH.robotButton.GetComponent<Image>().color;
                color.a = 0.5f;
                rH.robotButton.GetComponent<Image>().color = color;
            }
            else if(rH.robotButton.GetComponent<Image>().color.a < 1)
            {
                Color color = rH.robotButton.GetComponent<Image>().color;
                color.a = 1f;
                rH.robotButton.GetComponent<Image>().color = color;
            }
        }
    }
}
