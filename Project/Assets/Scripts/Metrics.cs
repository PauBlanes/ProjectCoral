using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metrics : MonoBehaviour {

    [System.Serializable]
    struct ThreatInfo
    {
        public string name;
        public float spawnTime;
        public float responseTime;
    }

    List<ThreatInfo> threatResponses = new List<ThreatInfo>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void AddThreatInfo (int n, float t)
    {        
        string tName = "";
        switch (n)
        {
            case 0:
                tName = "Net";
                break;
            case 1:
                tName = "Petrol";
                break;
            case 2:
                tName = "Rubbish";
                break;
            case 3:
                tName = "Repair";
                break;
            default:
                break;
        }
        ThreatInfo temp = new ThreatInfo
        {
            name = tName,
            spawnTime = t
        };        
        threatResponses.Add(temp);        
    }

    public void RobotSelected(string robotTag, float t) //ens guardem quan ha trigat a respondre l'usuari
    {

        for (int i = threatResponses.Count-1; i >= 0; i--)
        {           
            if (threatResponses[i].name == robotTag){
                ThreatInfo newT = threatResponses[i];
                newT.responseTime = t - newT.spawnTime;
                threatResponses[i] = newT;
                print(newT.responseTime);
            }
        }
    }


}
