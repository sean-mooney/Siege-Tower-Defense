using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuildScript : NetworkBehaviour {
    public GameObject arrowTower;
    public Camera playerCamera;
    public Tile1Script tile1Script;
    public bool buildingTowerCheck = false;
    public LayerMask layermask = 1 << 8;
    public Material red;
    public Material green;
    public Material blue;
    public int gold;
    public int team;
    private GameObject buildTower;
    private Renderer buildTowerStatus;
    private float timeCheck = 0;
    private bool mulitpleBuild = false;
    private Material buildTowerMaterial;
    private int towerCost;
    public Text goldText;

	// Use this for initialization
	public void InitializePlayer() {

        if (!isLocalPlayer)
        {
            return;
        }

        goldText = GameObject.Find("gold text").GetComponent<Text>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        tile1Script = GameObject.Find("Selection Tile 1").GetComponent<Tile1Script>();
        tile1Script.build = gameObject.GetComponent<BuildScript>();
        team = gameObject.GetComponent<CarpenterScript>().team;
        tile1Script.playerTeam = team;
        tile1Script.livesScript = GameObject.Find("lives text").GetComponent<LivesScript>();
        gameObject.GetComponent<SpawnScript>().InitializeSpawnScript();
    }
	
	// Update is called once per frame
	void Update () {

        if (buildingTowerCheck)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity, layermask);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);
            timeCheck += 1;
            if (hit.collider)
            {
                if (hit.collider.gameObject.tag == "GridSpace" && hit2D.collider == null)
                {
                    buildTower.gameObject.transform.position = (new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 18, hit.collider.gameObject.transform.position.z));
                    //gridBuildScript = hit.collider.gameObject.GetComponent("GridBuild");
                    if (!hit.collider.gameObject.GetComponent<GridBuild>().gridInUse)
                    {
                        buildTowerStatus.material = green;
                        if (Input.GetMouseButtonDown(0) && timeCheck >= 3)
                        {
                            if (gold >= towerCost)
                            {
                                hit.collider.gameObject.GetComponent<GridBuild>().gridInUse = true;
                                hit.collider.gameObject.transform.parent = buildTower.gameObject.transform;
                                buildTowerStatus.material = blue;
                                gameObject.GetComponent<CarpenterScript>().addTowerToArrays(new Vector3(buildTower.transform.position.x, buildTower.transform.position.y - 5, buildTower.transform.position.z), buildTower.gameObject);
                                //buildTower.GetComponent<Material>() set transparency and make blue
                                if (!Input.GetKey(KeyCode.LeftShift))
                                {
                                    gold -= towerCost;
                                    ChangeGoldText();
                                    buildingTowerCheck = false;
                                    HideGrid();
                                }
                                else
                                {

                                    gold -= towerCost;
                                    ChangeGoldText();
                                    mulitpleBuild = true;
                                    timeCheck = 0;
                                    BuildTower(towerCost);
                                }
                            }
                            else
                            {
                                Debug.Log("Need More Gold");
                            }
                        }
                    }
                    else
                    {
                        buildTowerStatus.material = red;
                    }
                }
            }
            else
            {

            }

            if (mulitpleBuild && !Input.GetKey(KeyCode.LeftShift))
                {
                    mulitpleBuild = false;
                    CancelBuild();
                }
            }

            if (buildingTowerCheck && Input.GetMouseButtonDown(1))
            {
                CancelBuild();
            }

    }

    public void BuildTower(int cost)
    {
        towerCost = cost;

        if (gold >= towerCost && !mulitpleBuild)
        {
            ShowGrid();
            buildTower = Instantiate(arrowTower, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
            buildTower.transform.eulerAngles = new Vector3(-90, 0, 0);
            buildTowerStatus = buildTower.gameObject.transform.FindChild("grid_status").GetComponent<Renderer>();
            buildingTowerCheck = true;
            timeCheck = 0;
        }

        if (mulitpleBuild)
        {
            ShowGrid();
            buildTower = Instantiate(arrowTower, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0), Quaternion.identity);
            buildTower.transform.eulerAngles = new Vector3(-90, 0, 0);
            buildTowerStatus = buildTower.gameObject.transform.FindChild("grid_status").GetComponent<Renderer>();
            buildingTowerCheck = true;
            timeCheck = 0;
        }
    }

    public void CancelBuild()
    {
        if (buildingTowerCheck)
        {
            Destroy(buildTower);
            HideGrid();
            buildingTowerCheck = false;
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        ChangeGoldText();
    }

    public void SubtractGold(int amount)
    {
        gold -= amount;
        ChangeGoldText();
    }

    public void ChangeGoldText()
    {
        goldText.text = gold.ToString();
    }

    [Command]
    public void CmdAddIncome(int team, int incomeAmt)
    {
        GameObject.Find("lives text").GetComponent<LivesScript>().CmdAddIncome(team, incomeAmt);
    }

    public void HideGrid()
    {
        GameObject[] grid = GameObject.FindGameObjectsWithTag("GridSpace");
        foreach (GameObject gridspace in grid)
        {
            gridspace.GetComponent<Renderer>().enabled = false;
        }
    }

    public void ShowGrid()
    {
        GameObject[] grid = GameObject.FindGameObjectsWithTag("GridSpace");
        foreach (GameObject gridspace in grid)
        {
            gridspace.GetComponent<Renderer>().enabled = true;
        }
    }
}
