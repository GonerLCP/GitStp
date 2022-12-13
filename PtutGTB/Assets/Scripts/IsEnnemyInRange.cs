using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsEnnemyInRange : MonoBehaviour
{
    public bool isEnnemyInRange;
    // Start is called before the first frame update
    void Start()
    {
        isEnnemyInRange = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            isEnnemyInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ennemy")
        {
            isEnnemyInRange = false;
        }
    }
}
