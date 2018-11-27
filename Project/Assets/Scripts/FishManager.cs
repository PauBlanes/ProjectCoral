using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

    public int maxFishes;

    public int smallFishesSpawn = 5;
    public int mediumFishesSpawn = 3;

    public int maxFishType = 5;

    public GameObject[] fishes;

    // Use this for initialization
    void Start () {

        /*foreach(GameObject fish in fishes){
            Debug.Log(fish.name + " : " + (fish.GetComponent<SpriteRenderer>().size * fish.transform.localScale).magnitude);
        }*/

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.A)) maxFishType++;

        while(GameObject.FindGameObjectsWithTag("FISH").Length < maxFishes) {

            int indexFish = Random.Range(0, Mathf.Clamp(maxFishType, 0, fishes.Length));
            Instantiate(fishes[indexFish], Vector3.zero, fishes[indexFish].transform.rotation);

            //Crea varios peces de los que son pequeños, con tag diferente a FISH, para contarlos solo como uno
            if((fishes[indexFish].GetComponent<SpriteRenderer>().size * fishes[indexFish].transform.localScale).magnitude < 1) {

                for (int i = 0; i < smallFishesSpawn; i++) {
                    Instantiate(fishes[indexFish], Vector3.zero, fishes[indexFish].transform.rotation).tag = "SMALL_FISH";
                }

            } else if ((fishes[indexFish].GetComponent<SpriteRenderer>().size * fishes[indexFish].transform.localScale).magnitude < 2) {

                for (int i = 0; i < mediumFishesSpawn; i++) {
                    Instantiate(fishes[indexFish], Vector3.zero, fishes[indexFish].transform.rotation).tag = "SMALL_FISH";
                }

            }

        }
		
	}
}
