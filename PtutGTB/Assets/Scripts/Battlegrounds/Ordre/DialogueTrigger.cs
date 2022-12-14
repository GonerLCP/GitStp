using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public DialogueALED dialogue;

    public BattleSystem battleSystem;

    public BattleState stater;


    private void Update()
    {
        if (battleSystem.state == stater)
        {
            TriggerDialogue();
            print(stater);
            return;
        }
    }
    public void TriggerDialogue ()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        battleSystem.state = BattleState.START;
    }

}
