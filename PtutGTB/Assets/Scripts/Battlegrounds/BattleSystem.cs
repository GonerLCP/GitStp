using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, ORDERTURN, PLAYERTURN, WON, LOST }

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

	// Start is called before the first frame update
	void Start()
	{
		state = BattleState.ORDERTURN;
		//PlayerTurn();
	}


	IEnumerator EnemyTurn()
	{
		//dialogueText.text = enemyUnit.unitName + " attacks!";

		//yield return new WaitForSeconds(1f);

		//bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

		//playerHUD.SetHP(playerUnit.currentHP);

		yield return new WaitForSeconds(1f);
	}

	void EndBattle()
	{
		if (state == BattleState.WON)
		{
			dialogueText.text = "You won the battle!";
		}
		else if (state == BattleState.LOST)
		{
			dialogueText.text = "You were defeated.";
		}
	}

	void PlayerTurn()
	{
		dialogueText.text = "Choose an action:";
	}


	public void OnAttackButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;
	}

	public void OnHealButton()
	{
		if (state != BattleState.PLAYERTURN)
			return;

	}

}