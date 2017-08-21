using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class selection_circle : MonoBehaviour {

    public GameObject selection;
    public GameObject carpenter;
    public Camera playerCamera;
    public Text selectionTitle;
    public Text healthText;
    public LayerMask layermask;
    public Tile1Script tile1;
    public BuildScript buildScript;
    private int currentSelectionHealth;
    private int currentSelectionMaxHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && carpenter)
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, layermask);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);
            int team = 0;
            if (hit.collider && !hit2D && !buildScript.buildingTowerCheck)
            {
                if (hit.collider.gameObject.name == "Mercenary Hut")
                {
                    selectionTitle.text = "Mercenary Hut";
                    selection = hit.collider.gameObject;
                    healthText.text = "100/100";
                    healthText.color = Color.green;
                    team = hit.transform.gameObject.GetComponent<TeamScript>().team;
                    tile1.SetSelection(hit.collider.gameObject.name, team);
                }
                else if (hit.collider.gameObject.name == "Carpenter")
                {
                    team = hit.collider.gameObject.GetComponent<CarpenterScript>().team;
                    SelectCarpenter(team);
                }
                else if (hit.collider.gameObject.name == "arrow_tower(Clone)" && hit.collider.gameObject.GetComponent<ArrowTower>().built)
                {

                    selectionTitle.text = "Arrow Tower";
                    selection = hit.collider.gameObject;
                    currentSelectionHealth = hit.collider.gameObject.GetComponent<ArrowTower>().health;
                    currentSelectionMaxHealth = hit.collider.gameObject.GetComponent<ArrowTower>().maxHealth;
                    healthText.text = currentSelectionHealth + "/" + currentSelectionMaxHealth;
                    healthText.gameObject.GetComponent<HealthScript>().SetHealthColor(currentSelectionHealth, currentSelectionMaxHealth);
                    team = hit.transform.gameObject.GetComponent<TeamScript>().team;
                    tile1.SetSelection(hit.collider.gameObject.name, team);
                }
                else if (hit.collider.gameObject.name == "Sheep(Clone)")
                {

                    selectionTitle.text = "Sheep";
                    selection = hit.collider.gameObject;
                    currentSelectionHealth = hit.collider.gameObject.GetComponent<SimpleCreepScript>().health;
                    currentSelectionMaxHealth = hit.collider.gameObject.GetComponent<SimpleCreepScript>().maxHealth;
                    healthText.text = currentSelectionHealth + "/" + currentSelectionMaxHealth;
                    healthText.gameObject.GetComponent<HealthScript>().SetHealthColor(currentSelectionHealth, currentSelectionMaxHealth);
                    team = hit.transform.gameObject.GetComponent<SimpleCreepScript>().team;
                    tile1.GetComponent<Tile1Script>().SetSelection(hit.collider.gameObject.name, team);
                }
                else
                {
                    UnSelect();
                }
            }
        }

        if (selection)
        {
            switch (selection.name) {
                case "Carpenter":
                    gameObject.transform.position = new Vector3(carpenter.transform.position.x, carpenter.transform.position.y - 2, carpenter.transform.position.z);
                    gameObject.transform.localScale = new Vector3(2, 2, 1);
                    break;
                case "Mercenary Hut":
                    gameObject.transform.position = new Vector3(selection.transform.position.x, selection.transform.position.y - 10, selection.transform.position.z);
                    gameObject.transform.localScale = new Vector3(12, 12, 1);
                    break;
                case "Sheep(Clone)":
                    gameObject.transform.position = new Vector3(selection.transform.position.x, selection.transform.position.y - 2, selection.transform.position.z);
                    gameObject.transform.localScale = new Vector3(2, 2, 1);
                    break;
                case "arrow_tower(Clone)":
                    gameObject.transform.position = new Vector3(selection.transform.position.x, selection.transform.position.y - 17, selection.transform.position.z);
                    gameObject.transform.localScale = new Vector3(5, 5, 1);
                    break;
            }                
        }
    }

    public void SelectCarpenter(int team)
    {
        
        selectionTitle.text = "Carpenter";
        selection = carpenter.transform.gameObject;
        healthText.text = "10/10";
        healthText.color = Color.green;
        tile1.GetComponent<Tile1Script>().SetSelection("Carpenter", team);
    }

    public void UnSelect()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        selectionTitle.text = "";
        selection = null;
        healthText.text = "";
        tile1.GetComponent<Tile1Script>().SetSelection("", 0);
    }
}
