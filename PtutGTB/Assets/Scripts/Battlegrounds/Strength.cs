using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strength : MonoBehaviour
{
    public int strengthUnit = 8;

    public GridDeplacement gridDeplacement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gridDeplacement.strength += strengthUnit;
        print("entrée");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gridDeplacement.strength -= strengthUnit;
        print("sortie");
    }
}
