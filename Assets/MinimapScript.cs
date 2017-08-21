using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour {

    public Camera playerCamera;
    public Camera minimapCamera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);
            if (hit2D)
            {
                if (hit2D.collider.tag == "Minimap")
                {
                    RaycastHit hit;
                    int moveSpeed = 1000;
                    Vector3 newPosition;
                    Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);
                    Physics.Raycast(ray, out hit);
                    newPosition = new Vector3(hit.point.x, playerCamera.transform.position.y, hit.point.z);
                    Vector3 movDiff = playerCamera.transform.position - newPosition;
                    Vector3 movDir = movDiff.normalized;
                    if (Vector3.Distance(playerCamera.transform.position, newPosition) >= 10)
                    {
                        if (Vector3.Distance(playerCamera.transform.position, newPosition) <= 30)
                        {
                            moveSpeed = 200;
                        } else
                        {
                            moveSpeed = 1000;
                        }
                        playerCamera.gameObject.GetComponent<CharacterController>().Move(-movDir * Time.deltaTime * moveSpeed);
                    }
                }
            }
        }
	}
}
