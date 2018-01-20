using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CustomList : MonoBehaviour {

    public List<GameObject> units = new List<GameObject>(1);

    void AddNew(GameObject newUnit)
    {
        units.Add(newUnit);
    }

    void Remove(int index)
    {
        units.RemoveAt(index);
    }
}
