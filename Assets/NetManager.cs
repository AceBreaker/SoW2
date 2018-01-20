using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DarkRift;
using UnityEngine.UI;

public class NetManager : MonoBehaviour
{
    public static int[] buildCosts;
    public string serverIP = "127.0.0.1";

    public static bool fogOn = false;
    
    public GameObject guiCamRef = null;

    public GameObject playerObject = null;

    Transform player;

    public static int playerNumber = 1;
    public static int playerCount = 1;

    public static bool networkConnected = false;

    public static bool possibleDesyncOccurred = false;
    public static int messageIndex = 0;

    public static UnitUpdate currUpdate;
    public static UnitUpdate prevUpdate;

    [SerializeField]
    public static Unit.Faction[] factions = new Unit.Faction[6];

    public static int astarGroundUnitIndex = 1;
    public static int ifactGroundUnitIndex = 14;
    public static int USBGroundUnitIndex = (int)UnitType.ADVERSARY;
    public static int astarAirIndex = 11;
    public static int ifactAirIndex = 23;
    public static int USBAirIndex = (int)UnitType.ORBITALSTRIKER;

    public int[] playersFactions;

    public GameObject[] factionObject;

    // Use this for initialization
    void Start()
    {
        Screen.SetResolution(1280, 768, Screen.fullScreen);
        NetManager.buildCosts = new int[3] { 1000, 2000, 5000 };
        for (int i = 0; i < 6; i++)
        {
            factions[i] = Unit.Faction.BASIC;
        }

        SceneManager.sceneLoaded += OnLevelWasLoaded;

        //DarkRiftAPI.Connect(serverIP, 4296);
        //GameObject go = GameObject.Find("Name1");
        ////go.GetComponent<UnityEngine.UI.InputField>().text = "chiucken";
        //Application.runInBackground = true;
        //DarkRiftAPI.onDataDetailed += ReceiveData;
        //
        //DontDestroyOnLoad(transform.gameObject);
        //
        //if (DarkRiftAPI.isConnected)
        //{
        //    DarkRiftAPI.SendMessageToOthers(TagIndex.Controller, TagIndex.ControllerSubjects.JoinMessage, "hi");
        //    DarkRiftAPI.SendMessageToAll(TagIndex.Controller, TagIndex.ControllerSubjects.SpawnPlayer, new Vector3(0.0f, 0.0f, 0.0f));
        //}
        //else
        //    Debug.Log("Failed to connect to DarkRift Server!");
        //
        //int id = playerNumber = DarkRiftAPI.id;

        astarGroundUnitIndex = (int)UnitType.INFANTRY;
        ifactGroundUnitIndex = (int)UnitType.ANDROID;
        USBGroundUnitIndex = (int)UnitType.ADVERSARY;
        astarAirIndex = (int)UnitType.FIGHTER;
        ifactAirIndex = (int)UnitType.CAPCOPTER;
        USBAirIndex = (int)UnitType.ORBITALSTRIKER;

        Debug.Log("began");
    }

    public void UpdateMyFaction(Dropdown sender)
    {
        if(DarkRift.DarkRiftAPI.isConnected)
        {
            factions[DarkRift.DarkRiftAPI.id-1] = (Unit.Faction)sender.value;
            SendData(TagIndex.Controller, TagIndex.PlayerUpdate, factions[DarkRiftAPI.id-1]);
        }
    }

    public void UpdateNetworkFaction(int otherPlayerNumber, Unit.Faction newFaction)
    {
        factions[otherPlayerNumber] = newFaction;
    }

    // Update is called once per frame
    void Update()
    {
        if (DarkRiftAPI.isConnected)
            DarkRiftAPI.Receive();
    }

    void OnLevelWasLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.LogError("LevelWasLoaded");
        if(scene.name != "CSS")
        {
            Debug.LogError("Setting playernumbers");

            Unit[] grid = GameObject.Find("Units Parent").GetComponentsInChildren<Unit>();
            foreach(Unit u in grid)
            {
                for(int i = 0; i < 6; i++)
                {
                    if(u.PlayerNumber == i)
                    {
                        u.faction = factions[i];
                    }
                }
            }

            GameObject playersObject = GameObject.Find("Players Parent");
            for (int i = 0; i < playersObject.transform.childCount; i++)
            {
                playersObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 0; i < 2; i++)
            {
                playersObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            Debug.LogError("Settings players' factions");

            Player[] players = GameObject.Find("Players Parent").GetComponentsInChildren<Player>();
            Debug.LogError("0");
            foreach(Player p in players)
            {
            Debug.LogError("1");
                p.myFactionIndex = (int)factions[p.PlayerNumber];
            Debug.LogError("2");
                SetPlayerIndexMods(p);
            Debug.LogError("3");
                Instantiate(factionObject[p.myFactionIndex]).transform.parent = p.transform;
                Debug.LogError(p.transform.childCount.ToString());
            }
        }
        //Do something;
    }

    void SetPlayerIndexMods(Player player)
    {
        switch(player.myFactionIndex)
        {
            case 0:
                player.myFactionGroundIndexMod = astarGroundUnitIndex;
                player.myFactionAirIndexMod = astarAirIndex;
                break;
            case 1:
                player.myFactionGroundIndexMod = ifactGroundUnitIndex;
                player.myFactionAirIndexMod = ifactAirIndex;
                break;
            case 2:
                player.myFactionGroundIndexMod = USBGroundUnitIndex;
                player.myFactionAirIndexMod = USBAirIndex;
                break;
            default:
                break;

        }
    }

    void OnApplicationQuit()
    {
        DarkRiftAPI.Disconnect();
    }

    void ReceiveData(ushort senderID, byte tag, ushort subject, object data)
    {
        bool ignoreMessage = false;
        if (ignoreMessage)
            return;

        if (tag == TagIndex.Controller)
        {
            if (subject == TagIndex.ControllerSubjects.JoinMessage)
            {
                networkConnected = true;
                DarkRiftAPI.SendMessageToID(senderID, TagIndex.Controller, TagIndex.ControllerSubjects.SpawnPlayer, null);
                SendData(TagIndex.Controller, TagIndex.PlayerUpdate, factions[DarkRiftAPI.id - 1]);
                if (senderID == 1)
                    playerNumber = 2;
                if (senderID > playerCount)
                    playerCount = senderID;
                //GameObject.Find("Name1").GetComponent<UnityEngine.UI.InputField>().readOnly = true;
                //GameObject.Find("Name2").GetComponent<UnityEngine.UI.InputField>().readOnly = false;

                //Handle Connect message
            }

            if (subject == TagIndex.ControllerSubjects.SpawnPlayer)
            {
                networkConnected = true;
                //Handle spawning the player into our game
            }

            if (subject == TagIndex.PlayerUpdate)
            {
                if (data is string && (data as string) == "ERROR")
                {
                    possibleDesyncOccurred = true;
                    RetrySendData();
                }
                if (data is string[])
                {
                    string[] name = data as string[];
                    if (name[0] == "name")
                    {
                        GameObject go = GameObject.Find("Name" + (senderID).ToString());
                        go.GetComponent<UnityEngine.UI.InputField>().text = name[1];
                    }
                    else if (name[0] == "StartGame")
                    {
                        DontDestroyOnLoad(transform.gameObject);
                        DontDestroyOnLoad(GameObject.Find("GameInitiator"));
                        //if (NetManager.playerCount >= 2)
                        //{
                        //    GameInit.mapName = GameInit.mapNames[NetManager.playerCount - 2];
                        //}
                        //string mapDropDownText = GameObject.Find("mapDropDown").GetComponent<Dropdown>().itemText.text;
                        //GameInit.mapName = mapDropDownText.Split(' ')[1];
                        GameInit.mapName = name[1];
                        SceneManager.LoadScene(GameInit.mapName);
                        //SceneManager.LoadScene("Main");
                    }
                    else if (name[0] == "charUpdate")
                    {
                        GameObject.Find(name[1]).GetComponent<UnityEngine.UI.Dropdown>().value = int.Parse(name[2]);
                    }

                }
                if (data is UnitUpdate /*&& messageIndex + 1 == (data as UnitUpdate).index*/)
                {
                    UnitUpdate update = data as UnitUpdate;
                    GameObject unitsParent = GameObject.Find("Units Parent");
                    Unit[] units = unitsParent.GetComponentsInChildren<Unit>();
                    CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
                    messageIndex = update.index;
                    if (IsEndTurnCommand(update))
                    {
                        grid.EndTurn();
                        //end turn
                    }
                    else if (IsMoveUnitCommand(update))
                    {
                        foreach (Unit u in units)
                        {
                            if (u.unitIndex == update.moving)
                            {
                                //CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();
                                foreach (Cell c in grid.Cells)
                                {
                                    if (c.transform.position.x == update.newLocationX &&
                                        c.transform.position.y == update.newLocationY)
                                    {
                                        u.AdjustCellMovementCosts();
                                        var path = u.FindPath(grid.Cells, c);
                                        u.ResetMovementPoints();
                                        u.Move(c, path);
                                        u.ResetMovementPoints();
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else if (IsAttackCommand(update))
                    {
                        Unit attacker = null;
                        Unit defender = null;
                        foreach (Unit u in units)
                        {
                            if (u.unitIndex == update.moving)
                            {
                                attacker = u;
                                //CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>();

                            }
                            if (u.unitIndex == update.target)
                            {
                                defender = u;
                            }
                            if (attacker != null && defender != null)
                            {
                                attacker.DealDamage(defender, true);
                                //attack

                                break;
                            }
                        }
                    }
                    else if (IsSpawnUnitCommand(update))
                    {
                        foreach (Unit c in grid.Units)
                        {
                            if (c.transform.position.x == update.newLocationX && c.transform.position.y == update.newLocationY)
                            {
                                if (c is BarracksUnit)
                                {
                                    (c as BarracksUnit).InstantiateUnit(update.type);
                                }
                                else if (c is BuildSite )
                                {
                                    if(update.type >= UnitType.INFANTRY && update.type <= UnitType.ANTIAIR)
                                    {
                                        //TODO 1
                                        //guiCamRef.GetComponent<NewBarracks>().SpawnAStarFromNetwork((int)update.type, (int)update.moving);
                                        guiCamRef.GetComponent<NewBarracks>().SpawnUnitFromNetwork((int)update.type, (int)update.moving, 1);
                                        //GameObject.Find("GUICamera").GetComponent<NewBarracks>().SpawnAStarFromNetwork((int)update.type, (int)update.index);
                                    }
                                    else if (update.type >= UnitType.ANDROID && update.type <= UnitType.TURRET)
                                    {
                                        guiCamRef.GetComponent<NewBarracks>().SpawniFactionFromNetwork((int)update.type, (int)update.moving);
                                    }
                                    else if (update.type >= UnitType.ADVERSARY && update.type <= UnitType.STASISGUN)
                                    {
                                        guiCamRef.GetComponent<NewBarracks>().SpawnusbFromNetwork((int)update.type, (int)update.moving);
                                    }
                                    //(c as BuildSite).InstantiateUnit(update.type);
                                }
                                break;
                            }
                        }
                        foreach (Unit c in grid.Units)
                        {
                            if (c.transform.position.x == update.newLocationX && c.transform.position.y == update.newLocationY)
                            {
                                if (c is Airport)
                                {
                                    (c as Airport).InstantiateUnit(update.type);
                                }
                                else if (c is BuildSite)
                                {
                                    if (update.type >= UnitType.FIGHTER && update.type <= UnitType.COPTER)
                                    {
                                        guiCamRef.GetComponent<NewAirport>().SpawnAStarFromNetwork((int)update.type, (int)update.moving);
                                    }
                                    else if (update.type >= UnitType.CAPCOPTER && update.type <= UnitType.BOMB)
                                    {
                                        guiCamRef.GetComponent<NewAirport>().SpawniFactionFromNetwork((int)update.type, (int)update.moving);
                                    }
                                    else if (update.type >= UnitType.ORBITALSTRIKER && update.type <= UnitType.NULLSTAR)
                                    {
                                        guiCamRef.GetComponent<NewAirport>().SpawniFactionFromNetwork((int)update.type, (int)update.moving);
                                    }
                                    //(c as BuildSite).InstantiateUnit(update.type);
                                }
                                break;
                            }
                        }
                        //CellGrid grid = GameObject.Find("CellGrid").GetComponent<CellGrid>()
                    }
                    else if (IsCaptureCommand(update))
                    {

                    }
                    else if (IsIncreaseMoneGenCommand(update))
                    {
                        foreach (Unit c in grid.Units)
                        {
                            if (c.unitIndex == update.moving)
                            {
                                if (c is City)
                                {
                                    (c as City).SelfIncreaseMoneyGenIfAble();
                                }
                                else if (c is BuildSite)
                                {
                                    (c as BuildSite).SelfIncreaseMoneyGenIfAble();
                                }
                            }
                        }
                    }
                    else if(IsUpgradeBuildSiteCommand(update))
                    {
                        BuildSite site = null;

                        foreach (Unit b in GameObject.Find("CellGrid").GetComponent<CellGrid>().Units)
                        {
                            if (b is BuildSite && b.unitIndex == update.moving)
                            {
                                site = b as BuildSite;
                                break;
                            }
                        }

                        if (site == null)
                            return;

                        //handle cost
                        Debug.Log("Site == null: " + (site == null).ToString());
                        Debug.Log("update == null: " + (update == null).ToString());
                        Debug.Log(NetManager.buildCosts[(int)update.buildingType - 1]);

                        if (!site.CanAffordToBuild(NetManager.buildCosts[(int)update.buildingType - 1]))
                        {
                            return;
                        }

                        site.grid.Players[site.PlayerNumber].Money -= NetManager.buildCosts[(int)update.buildingType - 1];
                        PlayerController.currTurnMoney = site.grid.Players[site.PlayerNumber].Money;
                        GameObject.Find("Canvas").GetComponentsInChildren<Text>()[site.PlayerNumber].text = PlayerController.currTurnMoney.ToString() + "G" + "(+" + site.grid.Players[site.PlayerNumber].income.ToString() + "G)";

                        site.buildingType = update.buildingType;
                        site.GetComponent<ChangeMaterial>().SetNewMaterial(update.buildingType);
                    }
                    
                    else if (data is UnitUpdate && messageIndex + 1 != (data as UnitUpdate).index)
                    {
                        possibleDesyncOccurred = true;
                        SendData(TagIndex.Controller, TagIndex.PlayerUpdate, "ERROR");
                        //send error message
                    }
                }
                else if (data is Unit.Faction)
                {
                    factions[senderID - 1] = (Unit.Faction)data;
                }
            }
        }
    }

    private bool IsEndTurnCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.END_TURN;
    }

    private bool IsMoveUnitCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.MOVE;
    }

    private bool IsAttackCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.ATTACK;
    }

    private bool IsSpawnUnitCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.SPAWN;
    }

    private bool IsCaptureCommand(UnitUpdate update)
    {
        return false;
    }

    private bool IsIncreaseMoneGenCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.INCOME;
    }

    private bool IsUpgradeBuildSiteCommand(UnitUpdate update)
    {
        return update.command == UnitUpdateCommand.BUILDSITE;
    }

    public static void SendData(byte tag, ushort subject, object data)
    {
        if (DarkRiftAPI.isConnected)
        {
            if (data is UnitUpdate)
            {
                (data as UnitUpdate).index = ++messageIndex;
                prevUpdate = currUpdate;
                currUpdate = (data as UnitUpdate);
            }
            DarkRiftAPI.SendMessageToOthers(tag, subject, data);
        }
    }

    public static void RetrySendData()
    {
        if (DarkRiftAPI.isConnected)
        {
            DarkRiftAPI.SendMessageToOthers(TagIndex.Controller, TagIndex.PlayerUpdate, prevUpdate);
            DarkRiftAPI.SendMessageToOthers(TagIndex.Controller, TagIndex.PlayerUpdate, currUpdate);
        }
    }

    public void TryToConnect()
    {
        if (DarkRiftAPI.isConnected)
            return;
        string sd = GameObject.Find("InputField").GetComponentsInChildren<Text>()[1].text;

        if (sd == "")
            sd = serverIP;
        if (!DarkRiftAPI.Connect(sd, 4296))
            return;
        GameObject go = GameObject.Find("Name1");
        //go.GetComponent<UnityEngine.UI.InputField>().text = "chiucken";
        Application.runInBackground = true;
        DarkRiftAPI.onDataDetailed += ReceiveData;

        DontDestroyOnLoad(transform.gameObject);

        if (DarkRiftAPI.isConnected)
        {
            DarkRiftAPI.SendMessageToOthers(TagIndex.Controller, TagIndex.ControllerSubjects.JoinMessage, "hi");
            DarkRiftAPI.SendMessageToAll(TagIndex.Controller, TagIndex.ControllerSubjects.SpawnPlayer, new Vector3(0.0f, 0.0f, 0.0f));
            int index = DarkRiftAPI.id - 1;
            GameObject.Find("Canvas").GetComponentsInChildren<Image>()[index].color = Color.white;
            if(DarkRift.DarkRiftAPI.id  != 1)
                GameObject.Find("Button").GetComponentsInChildren<Button>()[index].interactable = false;
        }
        else
            Debug.Log("Failed to connect to DarkRift Server!");

        int id = playerNumber = DarkRiftAPI.id;
        if(id > playerCount)
            playerCount = id;

        SendData(TagIndex.Controller, TagIndex.PlayerUpdate, factions[DarkRiftAPI.id - 1]);
    }

    public static int GetIDIfConnected()
    {
        if(DarkRift.DarkRiftAPI.isConnected)
        {
            return DarkRift.DarkRiftAPI.id;
        }
        return -1;
    }

    public static int songIndex = 0;

    public void SetSong(Dropdown sender)
    {
        songIndex = sender.value;
    }
}
