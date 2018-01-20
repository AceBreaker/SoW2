using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FindUIFocus {
    
    public static Transform GetUIObjectFocused()
    {
        //IEnumerable GameObject.Find("Canvas").transform
        foreach (Transform child in GameObject.Find("Canvas").transform)
        {
            if(child.GetComponent<Dropdown>().isActiveAndEnabled == true)
            {
                return child;
            }
        }
        return null;
    }

    public static Transform GetInputFieldFocused()
    {
        //IEnumerable GameObject.Find("Canvas").transform
        foreach (Transform child in GameObject.Find("Canvas").transform)
        {
            if (child.GetComponent<InputField>() as InputField != null 
                && child.GetComponent<InputField>().isFocused == true)
            {
                return child;
            }
        }
        return null;
    }
}
