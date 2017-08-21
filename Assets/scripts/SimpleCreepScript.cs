using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleCreepScript : NetworkBehaviour {

    public Transform destinationPoint;
    public selection_circle selectCircle;
    public HealthScript healthScript;
    public int speed;
    [SyncVar(hook = "OnChangeHealth")]
    public int health;
    public int maxHealth;
    public int team;
    public GameObject carpenter;
    public LivesScript livesScript;

    // Update is called once per frame

    void Update ()
    {

    }

    public void SubtractHealth(int damage)
    {
        if (!isServer)
        {
            return;
        }

        health -= damage;

        if (health <= 0)
        {
            carpenter.GetComponent<SpawnScript>().KillCreep(gameObject);
        }
    }

    public void OnChangeHealth(int health)
    {
        carpenter.GetComponent<SpawnScript>().UpdateHealthText(gameObject, health, maxHealth);

        if (health <= 0)
        {
            carpenter.GetComponent<SpawnScript>().UnselectCreep(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (!isServer)
            return;

        if (collisionObject.gameObject.tag == "EndZone")
        {
            livesScript = GameObject.Find("lives text").GetComponent<LivesScript>();
            livesScript.CmdSubtractLives(team);
            carpenter.GetComponent<SpawnScript>().KillCreep(gameObject);
        }
    }
}
