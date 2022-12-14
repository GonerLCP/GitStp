using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ennemy : MonoBehaviour
{
    public GridDeplacement gridDeplacement;

    public int strengthEnnemy;
    public GameObject textEnnemy;

    private void Start()
    {
        textEnnemy.GetComponent<TextMeshPro>().text = strengthEnnemy.ToString();
        GameObject.Find("LifeEnnemi").GetComponent<TextMeshPro>().text = "X";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (gridDeplacement.strength >= strengthEnnemy)
            {
                gridDeplacement.Ennemykilled = true;
                Destroy(gameObject);
            }

            if (gridDeplacement.strength < strengthEnnemy)
            {
                print("mdr c'est pas géré");
                //Destroy(gameObject);
            }
        }
    }
}
