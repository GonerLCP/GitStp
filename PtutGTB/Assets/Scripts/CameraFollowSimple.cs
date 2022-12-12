using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSimple : MonoBehaviour
{
    public Transform Target;
    public Vector2 Decalage;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            transform.position = new Vector3(Target.position.x + Decalage.x, Target.position.y + Decalage.y, transform.position.z);
        }
    }
}
