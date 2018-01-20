using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public static int currTurnMoney = 0;

    void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            Debug.LogError("enabling parents");
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
