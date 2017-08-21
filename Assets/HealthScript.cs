using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    private Text healthText;

	// Use this for initialization
	void Start () {
        healthText = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetHealthText(int currHealth, int maxHealth)
    {
        healthText.text = currHealth + "/" + maxHealth;
    }

    public void SetHealthColor(int currHealth, int maxHealth)
    {
        double healthRatio = (double)currHealth / (double)maxHealth;

        if(healthRatio >= 0.666)
        {
            healthText.color = Color.green;
        }
        else if (healthRatio < 0.666 && healthRatio >= 0.333)
        {
            healthText.color = Color.yellow;
        }
        else if (healthRatio < 0.333)
        {
            healthText.color = Color.red;
        }
    }
}
