using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageChart : MonoBehaviour {

    public static Dictionary<string, Dictionary<string, int>> damageValues;
    public TextAsset damageChartFile = null;
	// Use this for initialization
	void Start () {
        LoadFile("damage.txt");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LoadFile(string name)
    {
        damageValues = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, int> rowLine = new Dictionary<string, int>();

        string[] text = damageChartFile.text.Split('\n');
        int col = 0;
        string[] asdf = text[0].Split('\t');
        //foreach(string column in text)

        for (int i = 0; i < asdf.Length-1; i++)
        {
            string[] rowFromFile = text[i + 1].Split('\t');
            for (int j = 0; j < asdf.Length-1; j++)
            {
                rowLine.Add(asdf[j].ToUpper(),int.Parse(rowFromFile[j + 1]));
            }
            damageValues.Add(asdf[i].ToUpper(), new Dictionary<string, int>(rowLine));
            rowLine.Clear();
        }
    }
}
