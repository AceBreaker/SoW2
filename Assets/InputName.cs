using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetNameFromTextBox(string name)
    {
        Transform go = FindUIFocus.GetInputFieldFocused();
        if (go != null && go.name == "Name" + DarkRift.DarkRiftAPI.id && go.GetComponent<UnityEngine.UI.InputField>().text == name)
        {
            string[] myName = new string[2];
            myName[0] = "name";
            myName[1] = name;
        
            NetManager.SendData(TagIndex.Controller, TagIndex.PlayerUpdate, myName);
        }
    }

    public void SetName(int textBoxIndex, string name)
    {
        string textBox = "Name" + textBoxIndex.ToString();
        GameObject go = GameObject.Find(textBox);
        go.GetComponent<UnityEngine.UI.InputField>().text = name; 
    }
}
