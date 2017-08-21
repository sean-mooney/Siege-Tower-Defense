using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile1Script : MonoBehaviour {

    public BuildScript build;
    public Image tile1;
    public Material arrowTowerMat;
    public Material sheepCreepMat;
    public GameObject gridObject;
    public string selection;
    public int playerTeam;
    public LivesScript livesScript;

    private int arrowTowerCost = 10;
    private int sheepCost = 5;

    // Use this for initialization
    void Start () {
        tile1.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);

            if (hit2D.collider != null)
            {
                if (hit2D.collider.gameObject.name == "Selection Tile 1" && tile1.enabled == true && !build.buildingTowerCheck && selection == "carpenter")
                {
                    build.BuildTower(arrowTowerCost);
                }
                else if ((hit2D.collider.gameObject.name == "Selection Tile 1" && tile1.enabled == true) && selection == "mercenary hut")
                {
                    if (build.gold >= sheepCost)
                    {
                        livesScript = GameObject.Find("lives text").GetComponent<LivesScript>();
                        build.gameObject.GetComponent<SpawnScript>().CmdSpawnSheep(playerTeam);
                        build.SubtractGold(sheepCost);
                        build.CmdAddIncome(playerTeam, 1);
                    }
                }
            }
        }

        if (selection == "mercenary hut")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (build.gold >= sheepCost)
                {
                    build.gameObject.GetComponent<SpawnScript>().CmdSpawnSheep(playerTeam);
                    build.SubtractGold(sheepCost);
                    livesScript = GameObject.Find("lives text").GetComponent<LivesScript>();
                    build.CmdAddIncome(playerTeam, 1);
                }
            }
        }
        else if (selection == "carpenter")
            {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                build.BuildTower(arrowTowerCost);
            }
        }
    }

    public void SetSelection(string selectionName, int team)
    {
        if (team == playerTeam)
        {
            if (selectionName == "Carpenter")
            {
                tile1.enabled = true;
                tile1.material = arrowTowerMat;
                selection = "carpenter";
            }
            else if (selectionName == "Mercenary Hut")
            {
                tile1.enabled = true;
                tile1.material = sheepCreepMat;
                selection = "mercenary hut";
            }
        }
        else if (team != playerTeam)
        {
            tile1.enabled = false;
            selection = "";
        }


    }


}
