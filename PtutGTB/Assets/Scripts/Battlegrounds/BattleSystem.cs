using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BattleState { START, ORDERTURN, PLAYERTURN, ORDREAVANCE, ORDRERECUL, ORDREATTAQUE, WON, LOST, IDLE }
public enum BattleStateTampon { TORDERTURN, TPLAYERTURN, AVANCE, RECUL, ATTAQUE }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

    Vector3 positionQuandOrdre;
    Vector3 positionOrdreAvance;
    Vector3 positionOrdreRecul;

    int nbAvance = 2;

    Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

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

        switch (stateTampon)
        {
            case BattleStateTampon.TORDERTURN:
                break;
            case BattleStateTampon.TPLAYERTURN:
                break;
            case BattleStateTampon.AVANCE:
                positionOrdreAvance = new Vector3(positionQuandOrdre.x, positionQuandOrdre.y +(2.0f *2), positionQuandOrdre.z);
                break;
            case BattleStateTampon.RECUL:
                break;
            case BattleStateTampon.ATTAQUE:
                break;
            default:
                break;
        }
    }
}