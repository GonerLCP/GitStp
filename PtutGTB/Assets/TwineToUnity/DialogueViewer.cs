using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueObject;
using UnityEngine.Events;
using System;
using System.Runtime.InteropServices;
using TMPro;

public class DialogueViewer : MonoBehaviour
{
    public bool autoplay = true;
    public GameObject BoutonContainer;
    public List<Button> listeBoutons;
    public Button boutonSuivant;
    public string texteBoutonSuivant = "suivant";
    public Button boutonFin;
    public string texteBoutonFin = "fin";
    public TextMeshProUGUI txtMessage;
    public TextMeshProUGUI txtTitle;
    public GameObject dialogueContainer;
    DialogueController controller;
    DialogueActions actions;

    private void Start()
    {
        controller = GetComponent<DialogueController>();
        actions = GetComponent<DialogueActions>();
        controller.onEnteredNode += OnNodeEntered;
        if (boutonFin != null)
        {
            boutonFin.onClick.AddListener(delegate { OnEnd(); });
            boutonFin.gameObject.SetActive(false);
        }
        if (autoplay)
        {
            if(dialogueContainer!= null)
            {
                dialogueContainer.SetActive(true);
            }
            controller.InitializeDialogue();
        }
       
    }

    public void StartNewDialogue(TextAsset newTwineFile = null)
    {
        if (newTwineFile != null) controller.twineText = newTwineFile;
        if (dialogueContainer != null) dialogueContainer.SetActive(true);
        if (boutonFin != null) boutonFin.gameObject.SetActive(false);
        controller.InitializeDialogue();
    }

    public void ResetButtons()
    {
        for (int childIndex = listeBoutons.Count - 1; childIndex >= 0; childIndex--)
        {
            listeBoutons[childIndex].GetComponent<Button>().onClick.RemoveAllListeners();
            listeBoutons[childIndex].gameObject.SetActive(false);
        }
        if (boutonSuivant != null)
        {
            boutonSuivant.onClick.RemoveAllListeners();
        }
    }

    private void OnNodeSelected(int indexChosen)
    {
        //Debug.Log("Chose: " + indexChosen);
        controller.ChooseResponse(indexChosen);
    }

    private void OnNodeEntered(Node newNode)
    {
        // efface le texte et les choix précédents, 
        txtMessage.text = "";
        ResetButtons(); //enlève les liens des boutons des choix pour pouvoir mettre les nouveaux

        // affiche titre du noeud si on a définit un endroit pour
        //print("node "+ newNode.title +" entered");
        if (txtTitle != null)
        {
            txtTitle.text = newNode.title;
        }
        // affichage du texte et des liens en prenant en compte les conditions et variables

        string fullText = controller.texteAvecVariablesGerees(newNode.fullText,actions); // affiche le texte aevc gestion des conditions et variables
        newNode.fullText = fullText;
        //divise fullTexte en texte ett liens
        controller.SepareTexteEtNoeuds(newNode);
        // affihe le texte
        txtMessage.text = newNode.text;
        // affiche les liens vers d'autres passages
        //s'il y a un seul choix et texte du choix == le contenu de la variable texte bouton suivant : affiche bouton suivant
        if (newNode.responses.Count == 1 && newNode.responses[0].displayText == texteBoutonSuivant)
        {
            if (BoutonContainer != null) BoutonContainer.SetActive(false);
            boutonSuivant.gameObject.SetActive(true);
            boutonSuivant.onClick.AddListener(delegate { OnNodeSelected(0); });
        }
        // si un seul choix et texte du choix = contenu de la variable texte fin : affiche le bouton fin
        else if(newNode.responses.Count == 1 && newNode.responses[0].displayText == texteBoutonFin )
        {
            if (BoutonContainer != null) BoutonContainer.SetActive(false);
            boutonFin.gameObject.SetActive(true);
        }
        else if (newNode.responses.Count == 0) // si y'a pas de choix, cache tout
        {
            if (BoutonContainer != null) BoutonContainer.SetActive(false);
            if (boutonSuivant != null) boutonSuivant.gameObject.SetActive(false);
            if (boutonFin != null) boutonFin.gameObject.SetActive(false);
        }
        else // sinon affiche les choix
        {
            if (BoutonContainer != null) BoutonContainer.SetActive(true);
            if (boutonSuivant != null) boutonSuivant.gameObject.SetActive(false);
            for (int i = 0; i <= newNode.responses.Count - 1; i++)
            {
                int currentChoiceIndex = i;
                var response = newNode.responses[i];
                var responceButton = listeBoutons[i];
                responceButton.gameObject.SetActive(true);
                responceButton.GetComponentInChildren<TextMeshProUGUI>().text = response.displayText;
                responceButton.onClick.AddListener(delegate { OnNodeSelected(currentChoiceIndex); });
            }
        }
    }

    public void OnEnd()
    {
        actions.FinDialogue();
    }

}