using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipScript : MonoBehaviour {
    public GameObject selectCircle;
    public Text tooltipText;
    public Image tooltipBackground;

    string tooltipTitle;
    string tooltipHotKey;
    string tooltipInfo;
    string tooltipFinal = "";
    private selection_circle selectCircleScript;


	// Use this for initialization
	void Start () {
         selectCircleScript = selectCircle.GetComponent<selection_circle>();
    }
	
	// Update is called once per frame
	void Update () {

        if (!selectCircleScript.selection)
        {
            if (tooltipFinal != "")
            {
                HideShit();
            }
            return;
        }

        RaycastHit2D hit2D = Physics2D.Raycast(Input.mousePosition, Vector2.up);

        if (!hit2D)
        {
            if (tooltipFinal != "")
            {
                HideShit();
            }
            return;
        }

        if (tooltipFinal != "")
        {
            return;
        }





        string hitName = hit2D.collider.name;
        string selectionName = selectCircleScript.selection.name;

        if (hitName == "UI_background")
        {
            HideShit();
            Debug.Log(hitName == "UI_background");
            return;
        }

        if (hitName == "Selection Tile 1" && selectCircleScript.selection.name == "Carpenter");
        {
            tooltipTitle = "Arrow Tower";
            tooltipHotKey = "Q";
            tooltipInfo = "Basic arrow tower. Weak and slow. Can be upgraded";
        }

        tooltipFinal = tooltipTitle + " (" + tooltipHotKey + ") " + tooltipInfo;
        tooltipBackground.enabled = true;
        tooltipText.text = tooltipFinal;
    }

    public void HideShit()
    {
        tooltipBackground.enabled = false;
        tooltipFinal = "";
        tooltipText.text = "";
    }
}
