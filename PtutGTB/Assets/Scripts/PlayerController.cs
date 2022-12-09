using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        x = x * 1.5f;
        float y = Input.GetAxisRaw("Vertical");
        y = y * 2f;
        transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime;

    }
}
