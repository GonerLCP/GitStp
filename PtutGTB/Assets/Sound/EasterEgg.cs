using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    bool g, a, n, s, t;
    public GameObject GangstaSound;
    // Start is called before the first frame update
    void Start()
    {
        g = false; a = false; n = false; s = false; t = false; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("g"))
        {
            g=true;
        }

        if (Input.GetKeyDown("q"))
        {
            a = true;
        }
        if (Input.GetKeyDown("n"))
        {
            n = true;
        }
        if (Input.GetKeyDown("s"))
        {
            s = true;
        }
        if (Input.GetKeyDown("t"))
        {
            t = true;
        }

        if (g && a && n && s && t)
        {
            this.gameObject.GetComponent<AudioSource>().enabled = false;
            GangstaSound.GetComponent<AudioSource>().enabled = true;
        }
    }
}
