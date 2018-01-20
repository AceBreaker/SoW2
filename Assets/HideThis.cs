using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideThis : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HideAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowBarracksMenu(bool active)
    {
        GameObject.Find("BuildingMenu").SetActive(active);
    }

    public void HideAll()
    {
        if(DarkRift.DarkRiftAPI.isConnected)
            GameObject.Find("NetworkManager").GetComponent<NetManager>().guiCamRef = this.gameObject;
        //GameObject.Find("BuildingMenu").SetActive(false);
        //GameObject.Find("AirportMenu").SetActive(false);
        //GameObject.Find("CheapMenu").SetActive(false);
        //GameObject.Find("AirportCheapMenu").SetActive(false);
        //GameObject.Find("CityMenu").SetActive(false);
        //GameObject.Find("BuildSiteMenu").SetActive(false);
        //GameObject.Find("DynamicBuildingMenu").SetActive(false);
        //transform.gameObject.SetActive(false);

        HideGameObject(GameObject.Find("BuildingMenu"));
        HideGameObject(GameObject.Find("AirportMenu"));
        HideGameObject(GameObject.Find("CheapMenu"));
        HideGameObject(GameObject.Find("AirportCheapMenu"));
        HideGameObject(GameObject.Find("CityMenu"));
        HideGameObject(GameObject.Find("BuildSiteMenu"));
        HideGameObject(GameObject.Find("DynamicBuildingMenu"));
        HideGameObject(transform.gameObject);
    }

    public void HideGameObject(GameObject go)
    {
        if (go != null)
            go.SetActive(false);
    }
}
