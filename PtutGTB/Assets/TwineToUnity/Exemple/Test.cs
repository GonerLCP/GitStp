using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TextAsset nouveauFichierTwine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // code pour lancer le diaogue avec le texte contenu dans la variable nouveauFichierTwine
        if (Input.GetKeyDown(KeyCode.A)){
            // vous pouvez ajouter des lignes de code pour changer les zones ou le texte sera affiché ici, avant de lancer le diaogue
            GameObject.Find("narrationManager").GetComponent<DialogueViewer>().StartNewDialogue(nouveauFichierTwine);
        }

        // exemple de modification d'une variable nomée $uneVariable dans Twine
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject.Find("narrationManager").GetComponent<DialogueController>().variablesDict["uneVariable"] = "bla bla";
        }

        // exemple d'affichage du contenu d'une variable nomée $uneVariable dans Twine
        if (Input.GetKeyDown(KeyCode.P))
        {
            print(GameObject.Find("narrationManager").GetComponent<DialogueController>().variablesDict["uneVariable"]);
        }

    }
}
