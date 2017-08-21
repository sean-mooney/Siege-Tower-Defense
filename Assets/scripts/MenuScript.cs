using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public Camera playerCamera;
    private string goToMap;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);
            Debug.Log(hit2D.collider.gameObject);
            if (hit2D)
            {
                if (hit2D.collider.gameObject.name == "Start Game")
                {
                    NewGame();
                }
                else if (hit2D.collider.gameObject.name == "Quit Game")
                {
                    ExitGame();
                }
            }

        }
	}

    public void NewGame()
    {
        SceneManager.LoadScene("1v1-farm");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
