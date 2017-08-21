using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnScript : NetworkBehaviour {

    public GameObject sheepPrefab;
    public GameObject spawnArea;
    public GameObject destinationPoint;
    public CarpenterScript carpenterScript;
    public BuildScript buildScript;
    public Vector3 spawnPosition;
    private HealthScript healthScript;
    private GameObject selectionCircle;
    private LivesScript livesScript;

    // Use this for initialization
    public void InitializeSpawnScript() {

        if (!isLocalPlayer)
        {
            return;
        }
        buildScript = gameObject.GetComponent<BuildScript>();
        carpenterScript = gameObject.GetComponent<CarpenterScript>();
        healthScript = GameObject.FindWithTag("HealthText").GetComponent<HealthScript>();
        selectionCircle = GameObject.FindGameObjectWithTag("SelectCircle");
        livesScript = carpenterScript.livesScript;

    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update() {

    }

    [Command]
    public void CmdSpawnSheep(int team)
    {
        if (team == 1)
        {
            spawnArea = GameObject.Find("P2 Spawn Area");
            destinationPoint = GameObject.Find("P2 Destination Area");
        }
        else if (team == 2)
        {
            spawnArea = GameObject.Find("P1 Spawn Area");
            destinationPoint = GameObject.Find("P1 Destination Area");
        }

        Vector3 spawnPosition = new Vector3(Random.Range(spawnArea.transform.position.x - 20.0f, spawnArea.transform.position.x + 20.0f), spawnArea.transform.position.y, spawnArea.transform.position.z);

        GameObject newSheep = Instantiate(sheepPrefab, spawnPosition, spawnArea.transform.rotation);
        NetworkServer.Spawn(newSheep);

        RpcInitializeCreep(newSheep, destinationPoint, team);

    }

    [ClientRpc]
    public void RpcInitializeCreep(GameObject creep, GameObject destination, int creepTeam)
    {

        SimpleCreepScript creepScript = creep.GetComponent<SimpleCreepScript>();

        creepScript.selectCircle = GameObject.Find("selection_circle").GetComponent<selection_circle>();
        creepScript.healthScript = GameObject.Find("Health").GetComponent<HealthScript>();
        creepScript.transform.localScale = new Vector3(5, 5, 5);

        creepScript.destinationPoint = destination.transform;
        creepScript.transform.GetComponent<UnityEngine.AI.NavMeshAgent>().destination = creepScript.destinationPoint.position;
        creepScript.transform.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = creepScript.speed;
        creep.GetComponent<SimpleCreepScript>().livesScript = livesScript;
        creep.GetComponent<SimpleCreepScript>().team = creepTeam;
        creep.GetComponent<SimpleCreepScript>().carpenter = GameObject.Find("Carpenter");

    }

    public void KillCreep(GameObject creep)
    {
        CmdKillCreep(creep);
    }

    [Command]
    public void CmdKillCreep(GameObject creep)
    {
        
        RpcKillCreep(creep);
    }

    [ClientRpc]
    public void RpcKillCreep(GameObject creep)
    {
        creep.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        creep.GetComponent<Renderer>().material.color = Color.red;
        Destroy(creep, 2);
    }

    public void UnselectCreep (GameObject creep)
    {
        if (isLocalPlayer)
        {
            if (GetComponent<CarpenterScript>().selectCircle.GetComponent<selection_circle>().selection)
            {
                if (GetComponent<CarpenterScript>().selectCircle.GetComponent<selection_circle>().selection == creep)
                {
                    GetComponent<CarpenterScript>().selectCircle.GetComponent<selection_circle>().UnSelect();
                }
            }
        }
    }

    public void UpdateHealthText(GameObject creep, int health, int maxHealth)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (selectionCircle.GetComponent<selection_circle>().selection == creep)
        {

            healthScript.SetHealthText(health, maxHealth);
            healthScript.SetHealthColor(health, maxHealth);
        }
    }
}
