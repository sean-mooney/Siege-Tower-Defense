using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionToCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 300);
    }
}
