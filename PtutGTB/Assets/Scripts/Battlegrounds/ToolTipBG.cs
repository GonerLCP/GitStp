using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipBG : MonoBehaviour
{
    public string message;
    public GameObject Radius;

    private void OnMouseEnter()
    {
        ToolTipManagerBG.instance.SetAndShowToolTip(message);
        Radius.GetComponent<SpriteRenderer>().enabled = true;
        print("enter");
    }

    private void OnMouseExit()
    {
        ToolTipManagerBG.instance.HideToopTip();
        Radius.GetComponent<SpriteRenderer>().enabled = false;
        print("exit");
    }
}
