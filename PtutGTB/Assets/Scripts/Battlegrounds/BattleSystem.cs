using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BattleState { START, ORDERTURN, PLAYERTURN, ORDREAVANCE, ORDRERECUL, ORDREATTAQUE, WON, LOST, IDLE, SCOLD }
public enum BattleStateTampon { TORDERTURN, TPLAYERTURN, AVANCE, RECUL, ATTAQUE, SCOLD }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;

    public Vector3 positionQuandOrdre;
    Vector3 positionOrdreAvance;
    Vector3 positionOrdreRecul;

	public BattleState state;
    public BattleStateTampon stateTampon;

    public int rng;
    public int nbTours;

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.ORDERTURN;
	}

    private void Update()
    {
        switch (state)
        {
            case BattleState.ORDERTURN:

                rng = Random.Range(1, 4);
                switch (rng)
                {
                    case 1:
                        state = BattleState.ORDREAVANCE;
                        stateTampon = BattleStateTampon.AVANCE;
                        positionQuandOrdre = playerBattleStation.position;
                        positionOrdreAvance = new Vector3(positionQuandOrdre.x, positionQuandOrdre.y + (2.0f * 2), positionQuandOrdre.z);
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;

                    case 2:
                        state = BattleState.ORDRERECUL;
                        stateTampon = BattleStateTampon.RECUL;
                        positionQuandOrdre = playerBattleStation.position;
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;

                    case 3:
                        state = BattleState.ORDREATTAQUE;
                        stateTampon = BattleStateTampon.ATTAQUE;
                        positionQuandOrdre = playerBattleStation.position;
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;
                }
                break;

            default:
                break;
        }
    }
}