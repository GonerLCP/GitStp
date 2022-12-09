using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        ToolTipManager.instance.SetAndShowToolTip(message);
        print("enter");
    }

    private void OnMouseExit()
    {
        ToolTipManager.instance.HideToopTip();
        print("exit");
    }
}
