using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using static DialogueObject;

public class DialogueController : MonoBehaviour
{

    public TextAsset twineText;
    Dialogue curDialogue;
    Node curNode;
    public Dictionary<string, string> variablesDict;
    char[] charThatEndVariables = new char[] {' ', '/', ',', '.',�';',�'!',�'?',�':', '-', '_', '@', '(', ')', '=', '+', '-', 
        '*', '$', '�', '�', '&', '#', '>', '<', '~', '�', '^', '�', '[', ']', '{', '}', '�', '�', '"' };

    public delegate void NodeEnteredHandler(Node node);
    public event NodeEnteredHandler onEnteredNode;

    public Node GetCurrentNode()
    {
        return curNode;
    }

    public void InitializeDialogue()
    {
        curDialogue = new Dialogue(twineText);
        variablesDict = curDialogue.variablesDic;
        //printDictionnaire();
        curNode = curDialogue.GetStartNode();
        onEnteredNode(curNode);
    }

    public void printDictionnaire() // pour voir l'�tat des variables
    {
        string toPrint = "variables pr�sentes dans le dictionnaire : \n";
        foreach (KeyValuePair<string, string> kv in variablesDict)
            toPrint += kv.Key + " = " + kv.Value + "  ";
        print(toPrint);
    }

    public string texteAvecVariablesGerees(string texteBase, DialogueActions actions = null)
    {
        string texteClean = "";
        //si y'a pas de variables ni de conditions on change pas le texte
        if (texteBase.IndexOf("$") == -1 && texteBase.IndexOf("(set:") == -1 && texteBase.IndexOf("(if:") == -1)
        { 
            texteClean = texteBase;
        } 
       
        else // si y'en a
        {
            while (texteBase.IndexOf("$") != -1 || texteBase.IndexOf("(set:") != -1 || texteBase.IndexOf("(if:") != -1)
            {
                // d�termine le premier �l�ment � g�rer (set ou get de variable ou condition)
                int firstGet = texteBase.IndexOf("$");
                if(firstGet == -1)firstGet = 100000;
                int firstSet = texteBase.IndexOf("(set:");
                if (firstSet == -1) firstSet = 100000;
                int firstIf = texteBase.IndexOf("(if:");
                if (firstIf == -1) firstIf = 100000;
                int firstIndex = Mathf.Min(firstGet,firstSet,firstIf);
                // ajoute le texte avantt le premier truc � modifier au texte � afficher
                texteClean += texteBase.Substring(0, firstIndex);
                // enl�ve la  partie du texte d'aant le first index du texte surlequel on travail
                texteBase = texteBase.Substring(firstIndex);
                if(firstIndex == firstIf) // gestion conditions
                {
                    texteBase = gereConditions(texteBase);
                }
                else if (firstIndex == firstSet) //si c'est un set variable
                {
                    SetVariableTwine newSetVar = GestionVariables.SetVar(texteBase); // recup�re le nom et la nouvelle valeur de la variable en format string
                    //calcul
                    newSetVar.valeur = CalculeSetVar(newSetVar);
                    // enregistre dans dico
                    variablesDict[newSetVar.nomVariable] = newSetVar.valeur;
                    // check le contenu de la variable qui vient d'�tre modifi�e pour voir si elle d�clenche qqchose
                    if(actions != null)
                    {
                        actions.CheckVariablesOnSet(newSetVar.nomVariable, newSetVar.valeur);
                    }
                    // enleve le set var du texte base pour passer � la suite
                    texteBase = texteBase.Substring(newSetVar.toutLeSet.Length); 
                }
                else if (firstIndex == firstGet) // si c'est un get variable
                {
                    // determine la fin de la varaible : regarde qui est le plus proche entre un � la ligne et un espace
                    /*int indexEspace = texteBase.IndexOf(" ");
                    if (indexEspace == -1) indexEspace = 100000; // evite bug si y'a pas d'espace dans la suite du texte
                    int indexSautLigne = texteBase.IndexOf("\r\n");
                    if (indexSautLigne == -1) indexSautLigne = 100000; // evite bug si y'a pas de retour � la ligne dans la suite du texte
                    int indexCut = Mathf.Min(indexEspace,indexSautLigne);
                    if (indexCut == 100000) indexCut = texteBase.Length; // g�re si la variable est le dernier truc du texte et qu'il n'y a ni espace ni saut de ligne apr�s
                    */
                    int indexCut = 1;
                    while (indexCut <= texteBase.Length-1 && !Array.Exists(charThatEndVariables, element => element == texteBase[indexCut]))
                    {

                        indexCut += 1;
                    }
                    int indexSautLigne = texteBase.IndexOf("\r\n"); // verifie si y'a pas un saut de ligne plus proche
                    if (indexSautLigne == -1) indexSautLigne = 100000; // evite bug si y'a pas de retour � la ligne dans la suite du texte
                    indexCut = Mathf.Min(indexCut, indexSautLigne);

                    // decoupe � la bonne taille (enl�ve $ au debut)
                    string varName = texteBase.Substring(1, indexCut - 1); 
                    string varValue = variablesDict[varName]; // recup�re la valeur de la variable � partir de son nom
                    // l'ajoute au texte
                    texteClean += varValue; 
                    texteBase = texteBase.Substring(varName.Length + 1); //enl�ve la variable du texte restant � afficher(longueu de son nom plus le $)
                }
                else // si c'est une condition
                {
                }
            }
            texteClean += texteBase;
        }

        return (texteClean);
    }

    public void SepareTexteEtNoeuds(Node noeud)
    {
        curDialogue.SeparateTextAndLinks(noeud);
    }

    public string CalculeSetVar(SetVariableTwine SetValeur)
    {
        string valeur = SetValeur.valeur;
        string resultat = valeur; // valeur est trim donc ressemble � "$variable + 5"

        // check si valeur est une string simple : si oui return comme �a elle peut avoir un +/- etc dedans sans probleme
        if (SetValeur.isSttringUnique)
        {
            return (resultat);
        }

        if (valeur.Contains("+") || valeur.Contains("-") || valeur.Contains("*") || valeur.Contains("/"))
        {
            // r�cup�re les deux variables
            string valeur1;
            string typeVar1 = "";
            // si commence par guillemets
            if (valeur[0] == '"') //valeur 1 = limit" par guillemets + type = string
            {
                valeur1 = valeur[1..]; //enl�ve le premier guillemet
                valeur1 = valeur1.Substring(0, valeur1.IndexOf('"')); //va jusqu'au guillemet suivant
                typeVar1 = "string";
            }
            else // si pas de guillemets limit� par espace, et verifie si c'est une variable
            {
                valeur1 = valeur.Substring(0, valeur.IndexOf(" "));
                if (valeur1[0] == '$') valeur1 = GetVar(valeur1);
            }
            string valeur2;
            string typeVar2 = "";
            if (valeur[valeur.Length - 1] == '"') // same, si termine par guillemets, valeur 2 = limit�e par guillemets + type = string
            {
                valeur2 = valeur[0..^1]; //enl�ve le dernier guillemet
                valeur2 = valeur2.Substring(valeur2.LastIndexOf('"') + 1); //va jusqu'au guillemet d'avant
                typeVar2 = "string";
            }
            else
            {
                valeur2 = valeur.Substring(valeur.LastIndexOf(" ")+1);
                if (valeur2[0] == '$') valeur2 = GetVar(valeur2);
            }

            // v�rifie si c'est des variables et si oui get var -> mis plus haut c'est + logique
            /*if (valeur1[0] == '$')
            {
                valeur1 = GetVar(valeur1);
            }
            if (valeur2[0] == '$')
            {
                valeur2 = GetVar(valeur2);
            }*/

            // v�rifie si y'a au moins une string ou que des nombres
            float f1; float f2;
            if (typeVar1 == "string" || typeVar2 == "string" || !float.TryParse(valeur1, out f1) || !float.TryParse(valeur2, out f2))
            {
                // si y'a une string ou plus, concat�ne les strings :
                resultat = valeur1 + valeur2;
            }

            else // si que des nombres : calcul
            {
                if (valeur.Contains("+"))
                {
                    resultat = (float.Parse(valeur1) + float.Parse(valeur2)).ToString();
                }
                else if (valeur.Contains("-"))
                {
                    resultat = (float.Parse(valeur1) - float.Parse(valeur2)).ToString();
                }
                else if (valeur.Contains("*"))
                {
                    resultat = (float.Parse(valeur1) * float.Parse(valeur2)).ToString();
                }
                else if (valeur.Contains("/"))
                {
                    resultat = (float.Parse(valeur1) / float.Parse(valeur2)).ToString();
                }
            }
        }

        else // si y'a pas d'op�ration 
        {
            if (valeur.Contains("$")) // verifie si c'est une variable et si oui la r�cup�re, sinon change rien
            {
                resultat = GetVar(valeur);
            }
        }

        return resultat;
    }

    public string GetVar(string varNameBrut) // dans les calculs
    {
        string valeur;
        string varName = varNameBrut[1..]; //enl�ve le premier caract�re (le $ du d�but)
        if (variablesDict.ContainsKey(varName))
        {
           valeur = variablesDict[varName];
        }
        else
        {
            valeur = varNameBrut;
            print("la variable " + varNameBrut + " n'existe pas");
        }
           
        return valeur;
    }

    public string gereConditions(string texteBase)
    {
        // *** d�limite la condition ***
        string condition;
        string subPrGuillemets = texteBase;
        int finCondition;
        int finToAdd = 0;
        // tant qu'il y a des guillemets et qu'il sont avant la )
        while (subPrGuillemets.IndexOf('"') != -1 && subPrGuillemets.IndexOf(")") > subPrGuillemets.IndexOf('"'))
        {
            //enl�ve ce qui est avant et entre la premi�re paire de guillemets
            finToAdd += subPrGuillemets.IndexOf('"') + 1;
            subPrGuillemets = subPrGuillemets.Substring(subPrGuillemets.IndexOf('"') + 1);
            finToAdd += subPrGuillemets.IndexOf('"') + 1;
            subPrGuillemets = subPrGuillemets.Substring(subPrGuillemets.IndexOf('"') + 1);
        }
        finCondition = subPrGuillemets.IndexOf(")") + 1 + finToAdd;
        condition = texteBase.Substring(0, finCondition);
        texteBase = texteBase.Substring(finCondition); //enl�ve la condition du texte � traiter

        // *** d�limite la zone de texte li�e � la condition
        string texteLie;
        int finTexteLie = TrouveFinZoneLiee(texteBase);
        texteLie = texteBase.Substring(texteBase.IndexOf("[") + 1, finTexteLie - texteBase.IndexOf("[") - 2); //le -2 enl�ve les crochets
                                                                                                              // enl�ve le lien du texte base, pour le remettre sans les crochets seulement si la ocndition est vraie
        texteBase = texteBase.Substring(finTexteLie);

        // **** d�coupe la condition en variable(s) et comparateur
        string subCondition = condition[4..].TrimStart(); // enl�ve le (if: et les espace apr�s s'il y en a
                                                          //TODO si y'a des sauts de ligne dedans les remplacer par des espaces : �a �vite de les prendre en compte dans la fin des variables

        // variable 1
        bool isNot = subCondition.IndexOf("not") == 0;
        if (isNot)
        {
            subCondition = subCondition[3..].TrimStart(); //enl�ve le not s'il y en a un et les espaces apr�s
        }
        string var1;
        int finVar = firstOfString(subCondition, new string[] { " ", "\"", ")" }); //regarde s'il y a un espace avant les guillemets
        if (finVar == subCondition.IndexOf("\""))
        {
            finVar = subCondition.IndexOf("\"", 1) + 1; // deuxi�me "
        }
        var1 = subCondition.Substring(0, finVar);
        if (var1[0] == '"') var1 = var1[1..^1];
        else if (var1[0] == '$')
        {
            var1 = GetVar(var1);
        }
        subCondition = subCondition[finVar..].TrimStart();

        // inverse la var si c'est une bool et qu'il  y a le not
        if (isNot && var1 == "true")
        {
            var1 = "false";
        }
        else if (isNot && var1 == "false")
        {
            var1 = "true";
        }

        // comparateur
        int fin = 0;
        string comparateur = "";
        if (subCondition.IndexOf("is not") != -1) comparateur = "is not";
        else if (subCondition.IndexOf("is") != -1) comparateur = "is";
        else if (subCondition.IndexOf("<=") != -1) comparateur = "<=";
        else if (subCondition.IndexOf(">=") != -1) comparateur = ">=";
        else if (subCondition.IndexOf("<") != -1) comparateur = "<";
        else if (subCondition.IndexOf(">") != -1) comparateur = ">";
        fin = comparateur.Length;
        subCondition = subCondition[fin..].TrimStart();

        // variable 2
        string var2;
        if (comparateur == "")
        {
            var2 = "true";
            comparateur = "is";
        }
        else
        {
            var2 = subCondition.Substring(0, subCondition.Length - 1).Trim();
            if (var2[0] == '"') var2 = var2[1..^1];
            else if (var2[0] == '$')
            {
                var2 = GetVar(var2);
            }
        }
        // gestion else
        bool isElse = false;
        string texteLieElse = "";
        if (texteBase.IndexOf("(else:)") != -1 && (texteBase.IndexOf("(if:") == -1 || texteBase.IndexOf("(else:)") < texteBase.IndexOf("(if:")))
        {
            isElse = true;
            texteBase = texteBase.Substring(texteBase.IndexOf("(else:)") + 7);
            finTexteLie = TrouveFinZoneLiee(texteBase);
            texteLieElse = texteBase.Substring(texteBase.IndexOf("[") + 1, finTexteLie - texteBase.IndexOf("[") - 2); //le -2 enl�ve les crochets
                                                                                                                      // enl�ve le lien du texte base, pour le remettre sans les crochets seulement si la ocndition est vraie
            texteBase = texteBase.Substring(finTexteLie);
        }

        // *** resultat de la comparaison
        bool resultat = false;
        if (comparateur == "is not")
        {
            resultat = (var1 != var2)
                ? true
                : false;
        }
        else if (comparateur == "is")
        {
            resultat = (var1 == var2)
                ? true
                : false;
        }
        else // pour les comparaisons qui ne marchent qu'avec des nombres
        {
            float var1f;
            bool isFloat1 = float.TryParse(var1, out var1f);
            float var2f;
            bool isFloat2 = float.TryParse(var2, out var2f);
            if (!isFloat1 || !isFloat2)
            {
                print("comparaison impossible entre des variables qui ne sont pas des nombres dans la condition " + condition);
            }
            else
            {
                if (comparateur == ">")
                {
                    resultat = (var1f > var2f)
                        ? true
                        : false;
                }
                else if (comparateur == "<")
                {
                    resultat = (var1f < var2f)
                        ? true
                        : false;
                }
                else if (comparateur == "<=")
                {
                    resultat = (var1f <= var2f)
                        ? true
                        : false;
                }
                else if (comparateur == ">=")
                {
                    resultat = (var1f >= var2f)
                        ? true
                        : false;
                }
            }

        }
        // affichage du resultat
        string textResult = "";
        if (resultat)
        {
            textResult += texteLie;
        }
        else
        {
            if (isElse)
            {
                textResult += texteLieElse;
            }
        }
        textResult += texteBase;
        return textResult;
    }

    public int TrouveFinZoneLiee(string texteBase)
    {
        string subPrTexte = texteBase;
        int finTexteLie;
        int finToAdd = 0;
        // s'il y a un lien, qu'il est apr�s le premier crocher fermant, enleve tout le lien de la substring
        while (subPrTexte.IndexOf("]]") != -1 && subPrTexte.IndexOf("]") > subPrTexte.IndexOf("[["))
        {
            finToAdd += subPrTexte.IndexOf("]]") + 2;
            subPrTexte = subPrTexte.Substring(subPrTexte.IndexOf("]]") + 2);
        }
        finTexteLie = subPrTexte.IndexOf("]") + 1 + finToAdd;
        return finTexteLie;
    }

    public int firstOfString(string s, string[] elements)
    {
        int[] index = new int[elements.Length];
        int i = 0;
        foreach (string e in elements)
        {
            index[i] = s.IndexOf(e);
            if (index[i] == -1) index[i] = int.MaxValue;
            i++;
        }
        int resultat = Mathf.Min(index[0], index[1], index[2]); // TODO faire en fonction de la taille du tableau
        if (resultat == int.MaxValue) resultat = -1;
        return resultat;
    }

    public List<Response> GetCurrentResponses()
    {
        return curNode.responses;
    }

    public void ChooseResponse(int responseIndex)
    {
        string nextNodeID = curNode.responses[responseIndex].destinationNode;
        Node nextNode = curDialogue.GetNode(nextNodeID);
        curNode = nextNode;
        onEnteredNode(nextNode);
    }
}