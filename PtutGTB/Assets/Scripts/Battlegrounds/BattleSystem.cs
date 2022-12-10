using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum BattleState { START, ORDERTURN, PLAYERTURN, ORDREAVANCE, ORDRERECUL, ORDREATTAQUE, WON, LOST, IDLE }

public class BattleSystem : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public Transform playerBattleStation;
	public Transform enemyBattleStation;

	Unit playerUnit;
	Unit enemyUnit;

	public Text dialogueText;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

	public BattleState state;

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
            case BattleState.START:
                break;

            case BattleState.ORDERTURN:

                rng = Random.Range(1, 4);
                switch (rng)
                {
                    case 1:
                        state = BattleState.ORDREAVANCE;
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;

                    case 2:
                        state = BattleState.ORDRERECUL;
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;

                    case 3:
                        state = BattleState.ORDREATTAQUE;
                        nbTours = Random.Range(4, 6);
                        GameObject.Find("TxtNbTours").GetComponent<TextMeshProUGUI>().text = "Nombre de tours :" + nbTours;
                        break;
                }
                break;

            case BattleState.PLAYERTURN:
                break;

            case BattleState.ORDREAVANCE:
                nbTours = Random.Range(4, 6);

                break;

            case BattleState.ORDRERECUL:
                nbTours = Random.Range(4, 6);

                break;

            case BattleState.ORDREATTAQUE:
                nbTours = Random.Range(4, 6);

                break;

            case BattleState.WON:
                break;
            case BattleState.LOST:
                break;
            default:
                break;
        }

    }
}