using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Strength : MonoBehaviour
{
    public int strengthUnit = 8;

    public GridDeplacement gridDeplacement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            gridDeplacement.strength += strengthUnit;
            GameObject.Find("StrengthPlayer").GetComponent<TextMeshPro>().text = gridDeplacement.strength.ToString();
            print("entrée");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            gridDeplacement.strength -= strengthUnit;
            GameObject.Find("StrengthPlayer").GetComponent<TextMeshPro>().text = gridDeplacement.strength.ToString();
            print("sortie");
        }
    }
}
