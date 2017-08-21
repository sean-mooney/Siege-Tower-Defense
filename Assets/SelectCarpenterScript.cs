using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCarpenterScript : MonoBehaviour {

    public GameObject carpenter;
    public selection_circle selectionCircle;
    public int team;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);

            if (hit2D)
            {
                if (hit2D.collider.gameObject.name == "Select Carpenter")
                {
                    selectionCircle.SelectCarpenter(team);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            selectionCircle.SelectCarpenter(team);
        }
        

    }
}
