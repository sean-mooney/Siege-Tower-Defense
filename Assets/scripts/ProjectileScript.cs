using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {

    public int speed = 50;
    private int damage;
    private GameObject target;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (target)
        {
            transform.LookAt(target.transform);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else
        {
            Destroy(gameObject);
        }

	}

    public void FindTarget(GameObject target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target)
        {
            collision.gameObject.GetComponent<SimpleCreepScript>().SubtractHealth(damage);
            Destroy(gameObject);
        }
    }
}
