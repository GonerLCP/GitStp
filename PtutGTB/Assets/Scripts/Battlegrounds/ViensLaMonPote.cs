using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViensLaMonPote : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Pote" && Input.GetKeyDown("space"))
    //    {
    //        GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = true;

    //    }
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Pote" && Input.GetKeyDown("space"))
        {
            if (GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled == true)
            {
                GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = false;
            }
            else
            {
                GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = true;
            }

        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Pote" && Input.GetKeyDown("space"))
    //    {
    //        GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = false;
    //    }
    //}
}
