using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicBuildingMenu : MonoBehaviour {

    public GameObject purchaseButton = null;
    public GameObject tempTemplate = null;

    public GameObject contentPanel = null;

    public int startYPosition = 180;

	// Use this for initialization
	void Start () {
        //if(tempTemplate != null)
          //  CreateMenu(tempTemplate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateMenu(GameObject menuDefinition)
    {
        ClearMenu();
        
        int subtractYPosition = 70;
        GameObject guiCam = GameObject.Find("GUICamera");
        //int buttonHeight = 60;
        Debug.Log("Creating menu");
        for (int i = 0; i < menuDefinition.transform.childCount; i++)
        {
            GameObject go = Instantiate(purchaseButton, transform, false);
            go.transform.parent = contentPanel.transform;
            go.transform.localScale = new Vector3(1, 1, 1);

            //go.GetComponent<RectTransform>().position = new Vector3(9.0f, 180.0f - (70 * i));
            go.GetComponent<RectTransform>().anchoredPosition = new Vector2(9.0f, startYPosition - (subtractYPosition * i));
            int cost = menuDefinition.GetComponentsInChildren<Unit>()[i].price;
            go.GetComponentInChildren<Text>().text = menuDefinition.transform.GetChild(i).name + ": " + cost.ToString() + "G";

            Button tempButton = go.GetComponent<Button>();
            int currPlayerNumber = GameObject.Find("CellGrid").GetComponent<CellGrid>().CurrentPlayerNumber;
            int tempInt = i;
            GameObject playersParent = GameObject.Find("Players Parent");
            for (int j = 0; j < playersParent.transform.childCount; j++)
            {
                Debug.Log("Settings players active");
                playersParent.transform.GetChild(j).gameObject.SetActive(true);
            }

            Player[] players = playersParent.GetComponentsInChildren<Player>();

            int indexMod = -1 ;
            if (menuDefinition.name == "GroundUnits")
                indexMod = players[currPlayerNumber].myFactionGroundIndexMod;
            else if (menuDefinition.name == "AirUnits")
                indexMod = players[currPlayerNumber].myFactionAirIndexMod;


            tempButton.onClick.AddListener(() => guiCam.GetComponent<NewBarracks>().SpawnUnitWithButton(tempInt, indexMod));
            //tempButton.onClick.AddListener(() => ButtonClicked(tempInt));

            //menuDefinition.transform.GetChild(i);
        }
    }

    public void ClearMenu()
    {
        var children = new List<GameObject>();
        foreach (Transform child in contentPanel.transform)
            children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    void ButtonClicked(int buttonNo)
    {
        Debug.Log("Button clicked = " + buttonNo);
    }
}
