using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridDeplacement : MonoBehaviour
{
    private Vector3 startPos, endPos;
    public bool isMoving = false;
    public float MoveTime = 0.2f;

    BattleSystem state;
    public BattleSystem battleSystem;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) )
        {

            if (battleSystem.state != BattleState.PLAYERTURN)
            {
                return;
            }
            float x = Input.GetAxisRaw("Horizontal");
            x = x * 1.5f;
            float y = Input.GetAxisRaw("Vertical");
            y = y * 2f;
            if (!isMoving) StartCoroutine(MovePlayer(new Vector3(x, y, 0f)));
        }
       
    }
    IEnumerator MovePlayer(Vector3 dir)
    {
        isMoving = true;
        float nextMove = 0f;
        startPos = transform.position;
        endPos = startPos + dir;
        while (nextMove < MoveTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, nextMove / MoveTime);
            nextMove += Time.deltaTime;
            yield return null;
            battleSystem.state = BattleState.ORDERTURN;
        }
        print("Input");
        transform.position = endPos;
        isMoving = false;
    }
}