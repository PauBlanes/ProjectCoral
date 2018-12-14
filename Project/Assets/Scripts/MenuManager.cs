using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject startText;
	// Use this for initialization
	void Start () {
        StartCoroutine(Blink());
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
        {
            if (!Input.GetMouseButtonDown(0))   
                SceneManager.LoadScene("MAIN");
        }
	}

    IEnumerator Blink()
    {
        while (true)
        {
            startText.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            startText.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
