using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager2 : MonoBehaviour
{

    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    private Queue<string> sentences;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue2(DialoguePote dialogue2)
    {
        print("C'est l'animator le relou");
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue2.name;

        sentences.Clear();

        foreach (string sentence in dialogue2.sentences2)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence2();
    }

    public void DisplayNextSentence2()
    {
        if (sentences.Count == 0)
        {
            EndDialogue2();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence2(sentence));
    }

    IEnumerator TypeSentence2(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue2()
    {
        animator.SetBool("IsOpen", false);
    }
}
