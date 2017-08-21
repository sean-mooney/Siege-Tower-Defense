using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowTower : NetworkBehaviour {
    public GameObject projectile;
    private Transform bestTarget;
    private double fireRateInterval = 0;
    private GameObject projectileSpawn;

    [SyncVar]
    public string towerType;
    [SyncVar]
    public int health = 30;
    [SyncVar]
    public int maxHealth = 30;
    [SyncVar]
    public bool built = false;
    [SyncVar]
    public int firerate = 2; //Higher the value, longer the interval between shots
    [SyncVar]
    public int range = 20;
    [SyncVar]
    public int damage = 4;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!built)
        {
            return;
        }

        if (fireRateInterval >= firerate)
        {
            fireRateInterval = firerate;
            if (bestTarget) //has target
            {
                if (Vector3.Distance(transform.position, bestTarget.position) <= range && bestTarget.GetComponent<SimpleCreepScript>().health > 0) //target is within range
                {
                    projectileSpawn = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                    projectileSpawn.GetComponent<ProjectileScript>().FindTarget(bestTarget.gameObject, damage);
                    fireRateInterval = 0;
                }
                else
                {
                    bestTarget = null; //if not in range, then no target
                }
            }
            else
            {
                bestTarget = GetClosestEnemy(GameObject.FindGameObjectsWithTag("Creep")); //if no current target, find new one
            }
        }
        else
        {
            fireRateInterval += 0.05;
        }
        
	}

    
    public void CompleteBuild()
    {
        GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
        built = true;
        Destroy(gameObject.transform.FindChild("arrow_tower_build_trigger").gameObject);
        Destroy(gameObject.transform.FindChild("grid_status").gameObject);
    }
    

    public void CancelBuild(int team)
    {
        GetComponentInChildren<GridBuild>().gridInUse = false;
        GetComponentInChildren<GridBuild>().transform.parent = GameObject.Find("Player " + team + " Grid").transform;
    }

    Transform GetClosestEnemy(GameObject[] enemies)
    {
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (GameObject potentialTarget in enemies)
        {
            if (potentialTarget.GetComponent<SimpleCreepScript>().health > 0) //if target is still alive
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestEnemy = potentialTarget.transform;
                }
            }
        }

        return closestEnemy;
    }
}
