using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;

    public BattleSystem battleSystem;

    public BattleState stater;


    private void Update()
    {
        if (battleSystem.state == stater)
        {
            TriggerDialogue();
            return;
        }
    }
    public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        battleSystem.state = BattleState.START;
    }

}
