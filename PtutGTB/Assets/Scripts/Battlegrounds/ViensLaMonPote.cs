using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViensLaMonPote : MonoBehaviour
{
    bool trigger;
    public int space;
    int spaceTampon;

    public GameObject UnitPote;
    // Start is called before the first frame update
    void Start()
    {
        trigger = false;
        space = 0;
        UnitPote.GetComponent<PoteGridDeplacement>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger == true && Input.GetKeyDown("space"))
        {
            if (spaceTampon == 1)
            {
                space = 2;
                spaceTampon = 2;
            }
            else
            {
                space = 1;
                spaceTampon = 1;
            }
            
            if (UnitPote.GetComponent<PoteGridDeplacement>().enabled == true)
            {
                print("AH");
                UnitPote.GetComponent<PoteGridDeplacement>().enabled = false;
            }
            else
            {
                print("BH");
                UnitPote.GetComponent<PoteGridDeplacement>().enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Ahhhhhhhhhhhhhhh");
        if (collision.gameObject.tag == "Pote")
        {
            //GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = true;
            trigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Hooooooooooooooo");
        if (collision.gameObject.tag == "Pote")
        {
            //GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = false;
            trigger = false;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Pote" && Input.GetKeyDown("space"))
    //    {
    //        print("Camarche !");
    //        if (GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled == true)
    //        {
    //            print("AH");
    //            GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = false;
    //        }
    //        else
    //        {
    //            print("BH");
    //            GameObject.Find("Pote").GetComponent<PoteGridDeplacement>().enabled = true;
    //        }

    //    }
    //}
}
