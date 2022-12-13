using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public GridDeplacement gridDeplacement;

    public int strengthEnnemy;

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
                //Destroy(gameObject);
            }
        }
    }
}
