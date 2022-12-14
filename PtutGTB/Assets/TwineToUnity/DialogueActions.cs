using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogueActions : MonoBehaviour
{
    DialogueController controller;
    DialogueViewer viewer;

    public TextMeshProUGUI zone1;
    public TextMeshProUGUI zone2;

    public GameObject zoneNomPerso;
    public GameObject FondPerso;
    public GameObject zoneNomPerso2;
    public GameObject FondPerso2;

    int interlocuteur;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DialogueController>();
        viewer = GetComponent<DialogueViewer>();
        interlocuteur = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckVariablesOnSet(string nom, string valeur)
    {
        // exemple de changement de zone de texte avec un (set: $zoneTexte to "zone1") ou "zone2" dans twine 
        if (nom == "zoneTexte")
        {
            if(valeur == "zone1")
            {
                viewer.txtMessage = zone1;
            }
            else if(valeur == "zone2")
            {
                viewer.txtMessage = zone2;
            }
        }

        // exemples d'affichage d'une valeur dans une zone 
        // (il faut qu'il existe un game object avec un composant Text Mesh pro nommé vie et qu'il soit actif
        if (nom == "vie")
        {
            GameObject.Find("Vie").GetComponent<TextMeshProUGUI>().text = valeur;
        }
        // si la zone d'affichage n'est pas toujours active, il faut l'enregistrer dans une variable publique de type GameObject
        if (nom == "nomPerso")
        {
            if (valeur == "Gedrir")
            {
                FondPerso2.SetActive(false);
                zoneNomPerso2.SetActive(false);
                FondPerso.SetActive(true);
                zoneNomPerso.SetActive(true);
                zoneNomPerso.GetComponent<TextMeshProUGUI>().text = valeur;
            }
            if (valeur == "Sergent" || valeur == "Rupert" || valeur == "Vasvan")
            {
                FondPerso.SetActive(false);
                zoneNomPerso.SetActive(false);
                FondPerso2.SetActive(true);
                zoneNomPerso2.SetActive(true);
                zoneNomPerso2.GetComponent<TextMeshProUGUI>().text = valeur;
            }
        }

        // exemple pour éxecuter du code à chaque fois qu'une variable nomée action est changée à une valeur qui correspnd à une condition
        if (nom == "nomPerso")
        {

//Gedrir
            if (valeur == "Gedrir")
            {
                GameObject.Find("spGedrir").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                if (interlocuteur == 1)
                {
                    GameObject.Find("spSergent").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                }
                if (interlocuteur == 2)
                {
                    GameObject.Find("spRupert").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                }
                if (interlocuteur == 3)
                {
                    GameObject.Find("spVasvan").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
                }
            }
            else
            {
                GameObject.Find("spGedrir").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1f);
            }

//Sergent
            if (valeur == "Sergent")
            {
                GameObject.Find("spSergent").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                GameObject.Find("spRupert").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                GameObject.Find("spVasvan").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                interlocuteur = 1;
            }

//Rupert
            if (valeur == "Rupert")
            {
                GameObject.Find("spRupert").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                GameObject.Find("spSergent").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                GameObject.Find("spVasvan").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                interlocuteur = 2;
            }

//Vasvan
            if (valeur == "Vasvan")
            {
                GameObject.Find("spVasvan").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                GameObject.Find("spSergent").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                GameObject.Find("spRupert").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
                interlocuteur = 3;
            }
        }
    }

    public void CheckVariablesOnNodeDisplayed()
    {
        // comme cette condition sera executée à chaque fois qu'un noeud est affiché, il vaut mieux vérifier que la variable existe avant d'utiliser son contenu
        // si vous avez plusieurs fichiers twine et que les variables ne sont pas présentes dans tous
        // si vous avez un seul fichier twine ce n'est pas la peine la variable existera toujours même si elle est utilisée une seule fois
        if (controller.variablesDict.ContainsKey("nomPersonnage"))
        {
            string nom = controller.variablesDict["nomPersonnage"];
            if(nom == "Dalila")
            {

            }
            else if(nom == "Tom")
            {
                
            }
        }
    }

    public void FinDialogue()
    {
        print("rentrer dans la fin dialogue");
        if (GetComponent<DialogueController>().twineText.name == "FINALFINIPLUSJAMAIS")
        {
            print("rentrer dan la boucle");
            if (viewer.dialogueContainer != null)
            {
                print("changement de scene");
                viewer.dialogueContainer.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
