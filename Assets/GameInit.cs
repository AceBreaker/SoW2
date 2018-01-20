using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameInit : MonoBehaviour {

    bool initialized = false;
    string myName = "";
    public static string mapName = "BeanIslandNotNetwork";

    public static string[] mapNames;

    public GameObject aStarFaction;
    public GameObject iFaction;

	// Use this for initialization
	void Start () {
        mapNames = new string[5];
        mapNames[0] = "BuildSiteBattleOfThermo";
        mapNames[1] = "Isometric";
        mapNames[2] = "FourPlayer";
        mapNames[3] = "SixPlayer";
        mapNames[4] = "SixPlayer";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartMatch()
    {
        if (NetManager.playerCount <= 1)
        {
            Unit.debugoverride = true;
        }
        string mapDropDownText = GameObject.Find("mapDropDown").GetComponentInChildren<Text>().text;
        GameInit.mapName = mapDropDownText.Split(' ')[1];
        DontDestroyOnLoad(transform.gameObject);
        string[] startgame = new string[2];
        startgame[0] = "StartGame";
        startgame[1] = GameInit.mapName;
        NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, startgame);
        //if(NetManager.playerCount >= 2)
        //mapName = mapNames[NetManager.playerCount - 2];
        NetManager.messageIndex = 0;
        SceneManager.LoadScene(mapName);
       


    }

    public void SetName(string newName)
    {
        myName = newName;
    }

    public void SetVolume()
    {
        AudioListener.volume = GameObject.Find("Slider").GetComponent<Slider>().value;
    }
}
