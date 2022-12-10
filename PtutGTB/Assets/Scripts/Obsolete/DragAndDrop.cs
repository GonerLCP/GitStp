using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private bool isOverPote = false;
    private GameObject dropZone;
    private Vector2 startposition;
    public int ownStrength = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isOverPote = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverPote= false;
        dropZone = null;
    }
    public void StartDrag()
    {
        startposition= transform.position;
        isDragging = true;
    }

    public void EndDrag()
    {
        isDragging = false;
        if (isOverPote)
        {
           ownStrength += dropZone.GetComponent<Strength>().strengthUnit;
           Destroy(dropZone);
        }
        else
        {
            transform.position = startposition;
        }
    }
}
