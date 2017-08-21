using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour {

    public Image menuBackground;
    public Image menuPanel;
    public Image returnToGame;
    public Image exitGame;
    public Image leaveGame;
    public Text returnToGameText;
    public Text exitGameText;
    public Text leaveGameText;
    public GameObject menuButton;
    private bool pause;
    private int i;
    // Use this for initialization
    void Start () {
        menuBackground.enabled = false;
        menuBackground.GetComponent<Collider2D>().enabled = false;
        menuPanel.enabled = false;
        returnToGame.enabled = false;
        returnToGame.GetComponent<Collider2D>().enabled = false;
        returnToGameText.enabled = false;
        exitGame.enabled = false;
        exitGame.GetComponent<Collider2D>().enabled = false;
        exitGameText.enabled = false;
        leaveGame.enabled = false;
        leaveGame.GetComponent<Collider2D>().enabled = false;
        leaveGameText.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.down);
            if (hit2D)
            {
                if (hit2D.collider.gameObject == menuButton)
                {
                    Pause();
                }

                if (hit2D.collider.name == "ReturnToGameButton")
                {
                    Pause();
                }

                if (hit2D.collider.name == "ExitGameButton")
                {
                    Debug.Log("quit pressed");
                    Application.Quit();
                }
            }
        }
    }

    public void Pause()
    {
        if (!pause)
        {
            pause = true;
            menuBackground.enabled = true;
            menuBackground.GetComponent<Collider2D>().enabled = true;
            menuPanel.enabled = true;
            returnToGame.enabled = true;
            returnToGame.GetComponent<Collider2D>().enabled = true;
            returnToGameText.enabled = true;
            exitGame.enabled = true;
            exitGame.GetComponent<Collider2D>().enabled = true;
            exitGameText.enabled = true;
            leaveGame.enabled = true;
            leaveGame.GetComponent<Collider2D>().enabled = true;
            leaveGameText.enabled = true;
            gameObject.GetComponent<CameraMovement>().enabled = false;
        }
        else if (pause)
        {
            pause = false;
            menuBackground.enabled = false;
            menuBackground.GetComponent<Collider2D>().enabled = false;
            menuPanel.enabled = false;
            returnToGame.enabled = false;
            returnToGame.GetComponent<Collider2D>().enabled = false;
            returnToGameText.enabled = false;
            exitGame.enabled = false;
            exitGame.GetComponent<Collider2D>().enabled = false;
            exitGameText.enabled = false;
            leaveGame.enabled = false;
            leaveGame.GetComponent<Collider2D>().enabled = false;
            leaveGameText.enabled = false;
            gameObject.GetComponent<CameraMovement>().enabled = true;
        }
    }
}
