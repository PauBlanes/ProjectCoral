using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardButton : MonoBehaviour {

    public static int reward = 20;

	// Use this for initialization
	void Start () {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(DoOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DoOnClick ()
    {
        Camera.main.GetComponent<EnergyManager>().InvestigationReward(transform.parent.gameObject, reward);
    }

}
