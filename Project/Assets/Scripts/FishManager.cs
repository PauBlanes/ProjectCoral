using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public int maxFishes;

    public GameObject[] fishes;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        while(GameObject.FindGameObjectsWithTag("FISH").Length < maxFishes) {

            int indexFish = Random.Range(0, fishes.Length);
            Instantiate(fishes[indexFish], Vector3.zero, fishes[indexFish].transform.rotation);

        }
		
	}
}
