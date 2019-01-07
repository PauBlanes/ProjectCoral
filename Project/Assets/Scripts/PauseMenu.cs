using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;

	// Use this for initialization
	void Start () {
        pauseMenu.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Unpause();
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
            }            
        }

	}

    void Unpause()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void Resume()
    {
        Unpause();
    }
    public void Restart()
    {
        SceneManager.LoadScene("MAIN");
    }
    public void GoMenu()
    {
        Unpause();
        SceneManager.LoadScene("MainMenu");
    }
}
