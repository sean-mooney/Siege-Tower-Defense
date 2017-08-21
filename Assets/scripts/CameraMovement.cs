using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public int scrollDistance = 5;
    public float scrollSpeed = 70;

    // Use this for initialization

    void Start()
    {
        //gameObject.transform.localPosition = new Vector3(0, 2, 2);
    }

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        float mousePosX = Input.mousePosition.x;
        float mousePosY = Input.mousePosition.y;

        if (mousePosX < scrollDistance || Input.GetKey("a"))
        {
            controller.Move(Vector3.right * Time.deltaTime * -scrollSpeed);
        }

        if (mousePosX >= Screen.width - scrollDistance || Input.GetKey("d"))
        {

            controller.Move(Vector3.right * Time.deltaTime * scrollSpeed);
        }

        if (mousePosY < scrollDistance || Input.GetKey("s"))
        {
            controller.Move(Vector3.forward * Time.deltaTime * -scrollSpeed);
        }

        if (mousePosY >= Screen.height - scrollDistance || Input.GetKey("w"))
        {
            controller.Move(Vector3.forward * Time.deltaTime * scrollSpeed);
        }
    }
}
