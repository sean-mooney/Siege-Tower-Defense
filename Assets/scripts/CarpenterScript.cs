using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CarpenterScript : NetworkBehaviour {

    public GameObject selectCircle;
    public Camera playerCamera;
    public GameObject gotoObject;
    public BuildScript buildScript;
    public GameObject gotoArrows;
    public float moveSpeed = 20;
    public GameObject arrowTowerPrefab;
    [SyncVar (hook = "OnChangeLives")]
    public int lives;
    public int team = 1;
    private ArrayList towerPositionArray = new ArrayList();
    private float positionStartY;
    private ArrayList towerObjectArray = new ArrayList();
    private Quaternion lookPosition;
    private bool moving = false;
    private CharacterController controller;
    private bool movingToTower = false;
    private Vector3 nextTowerPosition;
    private Transform nextGridSpace;
    private GameObject carpenterSpawnPoint;
    public LivesScript livesScript;

    // Use this for initialization
    void Start() {

        if (!isLocalPlayer)
        {
            GetComponent<CarpenterScript>().enabled = false;
            GetComponent<BuildScript>().enabled = false;
            GetComponent<SpawnScript>().enabled = false;
            return;
        }

        InitializePlayer();
    }

    void InitializePlayer()
    {
        livesScript = GameObject.FindGameObjectWithTag("LivesText").GetComponent<LivesScript>();
        team = GetComponent<PlayerScript>().GetPlayers();

        if (team == 0)
        {
            return;
        }
        if (!isLocalPlayer)
        {
            return;
        }



        //set components                //                          //
        controller = GetComponent<CharacterController>();
        Physics.IgnoreLayerCollision(8, 9);
        Physics.IgnoreLayerCollision(8, 10);
        Physics.IgnoreLayerCollision(10, 10);
        Physics.IgnoreLayerCollision(9, 10);
        gameObject.name = "Carpenter";
        gotoObject = GameObject.FindGameObjectWithTag("MoveArrows");
        buildScript = GameObject.FindGameObjectWithTag("Cursor").GetComponent<BuildScript>();
        selectCircle = GameObject.FindGameObjectWithTag("SelectCircle");
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        gotoArrows = gotoObject.transform.GetChild(0).gameObject;
        //                              //                          //

        carpenterSpawnPoint = GameObject.Find("P" + team + " Carpenter Spawn");
        gameObject.transform.position = carpenterSpawnPoint.transform.position;
        positionStartY = gameObject.transform.position.y;

        //set component dependencies of carpenter
        livesScript.localPlayerTeam = team;
        livesScript.carpenter = gameObject.GetComponent<BuildScript>();
        selectCircle.gameObject.GetComponent<selection_circle>().carpenter = gameObject;
        buildScript = gameObject.GetComponent<BuildScript>();
        selectCircle.GetComponent<selection_circle>().buildScript = gameObject.GetComponent<BuildScript>();
        GameObject.Find("Select Carpenter").GetComponent<SelectCarpenterScript>().carpenter = gameObject;
        GameObject.Find("Select Carpenter").GetComponent<SelectCarpenterScript>().team = team;
        gameObject.transform.parent = GameObject.Find("Player " + team).transform;

        switch (team)
        {
            case 1:
                lives = livesScript.player1Lives;
                break;
            case 2:
                lives = livesScript.player2Lives;
                break;
        }

        buildScript.InitializePlayer();
    }

    // Update is called once per frame
    void Update() {
        if (team == 0)
        {
            InitializePlayer();
        }

        if (!isLocalPlayer)
        {
            return;
        }

        if (movingToTower)
        {
            moving = true;
            if (towerPositionArray.Count >= 1 && towerObjectArray.Count == towerPositionArray.Count)
            {
                gotoObject.transform.position = (Vector3)towerPositionArray[0];
            }

        }

        if (moving)
        {
            lookPosition = Quaternion.LookRotation(new Vector3(gotoObject.transform.position.x, positionStartY, gotoObject.transform.position.z) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookPosition, Time.deltaTime * 10);
            controller.Move(transform.forward * Time.deltaTime * moveSpeed);
        }

        if (Input.GetMouseButtonDown(1) && selectCircle.GetComponent<selection_circle>().selection == gameObject)
        {

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);

            if (hit.collider && !hit2D)
            {
                gotoObject.transform.position = (new Vector3(hit.point.x, 34, hit.point.z));
                gotoArrows.transform.localPosition = (new Vector3(0, -1.02f, 0));
                gotoArrows.GetComponent<Animator>().Play("Default Take", -1, 0.3f);
                moving = true;
            }


            if (towerObjectArray.Count > 0)
            {
                CancelTowers();
                movingToTower = false;
            }
        }
    }

    private void OnTriggerEnter(Collider collisionObject)
    {

        if (collisionObject.gameObject.name == "carpenter_goto" && !movingToTower)
        {
            moving = false;
        }
    }

    private void OnTriggerStay(Collider collisionObject)
    {

        if (!isLocalPlayer)
        {
            return;
        }

        if (Vector3.Distance(transform.position, gotoObject.transform.position) < 5)
        {
            moving = false;
        }

        if (collisionObject.gameObject.tag == "BuildTrigger" && movingToTower && (collisionObject.gameObject.transform.parent.gameObject == (GameObject)towerObjectArray[0]))
        {
            Transform parentTower = collisionObject.gameObject.transform.parent;
            nextTowerPosition = parentTower.position;
            nextGridSpace = parentTower.FindChild("GridSpace").transform;
            if (towerPositionArray.Count == 1)
            {
                CmdCompleteTower(parentTower.GetComponent<ArrowTower>().towerType, nextTowerPosition.x, nextTowerPosition.y, nextTowerPosition.z);
                Destroy(parentTower.gameObject, 0.1f);
                removeTowerFromArrays();
                gotoObject.transform.position = new Vector3(0, 0, 0);
                movingToTower = false;
                moving = false;
            }
            else
            {
                removeTowerFromArrays();
                CmdCompleteTower(parentTower.GetComponent<ArrowTower>().towerType, nextTowerPosition.x, nextTowerPosition.y, nextTowerPosition.z);
                Destroy(parentTower.gameObject, 0.1f);
            }
        }
    }

    /// <summary>/////////////////////////////////////////////////////////////////////////////////////////////
    /// Methods
    /// </summary>////////////////////////////////////////////////////////////////////////////////////////////

    public void OnChangeLives(int lives)
    { 
        
    } 

    public void addTowerToArrays(Vector3 newPosition, GameObject newObject) {
        towerPositionArray.Add(newPosition);
        towerObjectArray.Add(newObject);
        if (!movingToTower)
        {
            movingToTower = true;
        }
    }

    public void removeTowerFromArrays()
    {
        towerPositionArray.RemoveAt(0);
        towerObjectArray.RemoveAt(0);
    }

    public void CancelTowers()
    {

        if (!isLocalPlayer)
        {
            return;
        }
        movingToTower = false;
        while (towerObjectArray.Count != 0)
        {
            GameObject arrow;
            arrow = (GameObject)towerObjectArray[0];
            arrow.GetComponent<ArrowTower>().CancelBuild(team);

            Destroy((GameObject)towerObjectArray[0]);
            towerObjectArray.RemoveAt(0);
            towerPositionArray.RemoveAt(0);

        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    private void CmdCompleteTower(string towerType, float x, float y, float z)
    {
        GameObject nextTower = null;

        if (towerType == "arrowTower")
        {
            nextTower = arrowTowerPrefab;
        }

        GameObject completeTower = Instantiate(nextTower);
        completeTower.transform.position = new Vector3(x, y, z);
        completeTower.GetComponent<TeamScript>().team = team;
        NetworkServer.Spawn(completeTower);
        RpcCompleteTower(completeTower);

    }

    [ClientRpc]
    private void RpcCompleteTower (GameObject tower)
    {
        if (isLocalPlayer)
        {
            nextGridSpace.transform.parent = tower.transform;
        }
        tower.GetComponent<ArrowTower>().CompleteBuild();
    }
}
