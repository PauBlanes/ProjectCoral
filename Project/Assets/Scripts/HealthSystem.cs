using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour {

    private Image lifeBar;

    private int maxHealth = 100;
    public float currentHealth = 100;

    private bool isCoral;

	// Use this for initialization
	void Start () {
        if(SceneManager.GetActiveScene().name == "MAIN")
            lifeBar = transform.GetChild(transform.childCount-1).GetChild(0).GetChild(0).GetComponent<Image>();        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateHealth(int value)
    {
        if (currentHealth > 0)
        {
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            lifeBar.fillAmount = currentHealth / maxHealth;
        }
        else
        {
            Camera.main.GetComponent<EcosystemManager>().HideCoral(gameObject);
            currentHealth = maxHealth;
        }
    }
}
