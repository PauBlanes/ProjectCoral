using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public int maxFishes;

    public GameObject[] fishes;

    public GameObject[] actualFishes;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //if(actualFishes.Length < maxFishes) {

            //Instantiate(fishes[0], Vector3.zero, Quaternion.identity).GetComponent<FishMovement>().movementType = FishMovement.Type.HorizontalToLeft;
            
            /*actualFishes.SetValue(Instantiate(fishes[0], Vector3.zero, Quaternion.identity), 1);
            actualFishes[1].GetComponent<FishMovement>().movementType = FishMovement.Type.HorizontalToRight;

            actualFishes.SetValue(Instantiate(fishes[0], Vector3.zero, Quaternion.identity), 2);
            actualFishes[2].GetComponent<FishMovement>().movementType = FishMovement.Type.Vertical;*/

        //}
		
	}
}
