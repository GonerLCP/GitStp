using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

    public BattleSystem battleSystem;


    private void Update()
    {
        if (Input.GetKeyDown("space") || battleSystem.state != BattleState.ORDERTURN)
        {
            return;
        }
        TriggerDialogue();
    }
    public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        battleSystem.state = BattleState.START;
    }

}
