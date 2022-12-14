using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerPote : MonoBehaviour
{
    public DialoguePote dialogue2;

    public ViensLaMonPote rammennetoi;

    public int QuelDialogue;

    private void Update()
    {
        if (rammennetoi.space == QuelDialogue)
        {
            print("ca marche");
            rammennetoi.space = 0;
            TriggerDialogue2();
            return;
        }
    }
    public void TriggerDialogue2()
    {
        FindObjectOfType<DialogueManager2>().StartDialogue2(dialogue2);
    }
}
