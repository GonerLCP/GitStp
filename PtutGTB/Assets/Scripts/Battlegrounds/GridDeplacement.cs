using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridDeplacement : MonoBehaviour
{
    private Vector3 startPos, endPos;
    public bool isMoving = false;
    public float MoveTime = 0.2f;
    public int strength = 10;
    public int life = 3;

    BattleSystem state;
    public BattleSystem battleSystem; 
    public bool Ennemykilled;

    private void Start()
    {
        Ennemykilled = false;
        GameObject.Find("StrengthPlayer").GetComponent<TextMeshPro>().text = strength.ToString();
        GameObject.Find("LifePlayer").GetComponent<TextMeshPro>().text = life.ToString();
    }
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
            //battleSystem.state = BattleState.IDLE;
        }
        battleSystem.nbTours = battleSystem.nbTours - 1;
        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + battleSystem.nbTours;
        if (battleSystem.nbTours <= 0)
        {
            switch (battleSystem.stateTampon)
            {
                case BattleStateTampon.AVANCE:
                    if (battleSystem.playerBattleStation.position.y < battleSystem.positionOrdreAvance.y)
                    {
                        battleSystem.stateTampon = BattleStateTampon.SCOLD;
                        battleSystem.state = BattleState.SCOLD;
                        life = life - 1;
                        GameObject.Find("LifePlayer").GetComponent<TextMeshPro>().text = life.ToString();
                    }
                    else
                    {
                        battleSystem.state = BattleState.ORDERTURN;
                    }
                    break;

                case BattleStateTampon.RECUL:
                    if (battleSystem.playerBattleStation.position.y > battleSystem.positionOrdreRecul.y)
                    {
                        battleSystem.stateTampon = BattleStateTampon.SCOLD;
                        battleSystem.state = BattleState.SCOLD;
                        life = life - 1;
                        GameObject.Find("LifePlayer").GetComponent<TextMeshPro>().text = life.ToString();
                    }
                    else
                    {
                        battleSystem.state = BattleState.ORDERTURN;
                    }
                    break;

                case BattleStateTampon.ATTAQUE:
                    if (Ennemykilled == false)
                    {
                        battleSystem.stateTampon = BattleStateTampon.SCOLD;
                        battleSystem.state = BattleState.SCOLD;
                        life = life - 1;
                        GameObject.Find("LifePlayer").GetComponent<TextMeshPro>().text = life.ToString();
                    }
                    else
                    {
                        Ennemykilled = false;
                        battleSystem.state = BattleState.ORDERTURN;
                    }
                    break;
            }
        }
        //print("Input");
        transform.position = endPos;
        isMoving = false;
    }
}