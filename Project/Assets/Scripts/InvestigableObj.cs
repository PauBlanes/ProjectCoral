using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigableObj : MonoBehaviour {

    public GameObject info;

	// Use this for initialization
	void Start () {        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ShowInfo()
    {
        Time.timeScale = 0;
        GameObject newInfo = Instantiate(info, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        newInfo.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);        
    }    

}
