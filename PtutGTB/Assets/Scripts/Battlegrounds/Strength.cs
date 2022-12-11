using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strength : MonoBehaviour
{
    public int strengthUnit = 8;

    public GridDeplacement gridDeplacement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            gridDeplacement.strength += strengthUnit;
            print("entrée");
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            gridDeplacement.strength -= strengthUnit;
            print("sortie");
        }
    }
}
