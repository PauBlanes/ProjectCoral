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
        public Image cooldownIcon;
    }          

    public List<RobotHud> robots = new List<RobotHud>();

    public float iRobotTimer;
    private float iRobotCounter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCooldown();
    }

    public void SelectRobot(Button b)
    {        
        if (b.transform.GetChild(0).GetComponent<Image>().fillAmount == 0) //si tens suficient diners
        {            
            foreach (RobotHud r in robots)
            {
                if (r.robotButton == b)
                {                  
                    Instantiate(r.robot, new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y), Quaternion.identity);           
                    
                    if (r.tagName != "Investigable") //si no es de investigacio restem el cost
                    {
                        Camera.main.GetComponent<EnergyManager>().UpdateCounter(-r.cost);
                    }
                    else //si es de investigació resetegem el temps
                    {
                        iRobotCounter = iRobotTimer;
                    }
                }
            }            
        }
    }
    
    void UpdateCooldown()
    {
        foreach (RobotHud rH in robots)
        {
            if (rH.cost != 0 && rH.tagName != "Investigable")
            {                
                float a = Mathf.Clamp(rH.cost - GetComponent<EnergyManager>().GetCount(), 0, rH.cost);
                float b = a / rH.cost;                
                rH.cooldownIcon.fillAmount = b;
            }
            else if (rH.tagName == "Investigable")
            {
                iRobotCounter -= Time.deltaTime;
                iRobotCounter = Mathf.Clamp(iRobotCounter, 0, iRobotTimer);
                rH.cooldownIcon.fillAmount = iRobotCounter / iRobotTimer;
            }
                
            //si no tens la pasta no es pot fer spawn, que passsa si no hi ha l'amenaça
            /*if (rH.cost > GetComponent<EnergyManager>().GetCount())
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
            }*/
        }
    }   
}
