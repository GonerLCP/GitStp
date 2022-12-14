using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueObject
{
    private const string kStart = "START";
    private const string kEnd = "END";

    public struct Response
    {
        public string displayText;
        public string destinationNode;

        public Response(string display, string destination)
        {
            displayText = display;
            destinationNode = destination;
        }
    }

    public class Node
    {
        public string title;
        public string fullText;
        public string text;
        public List<string> tags;
        public List<Response> responses;

        internal bool IsEndNode()
        {
            return tags.Contains(kEnd);
        }

        // TODO proper override
        public string Print()
        {
            return "";//string.Format( "Node {  Title: '%s',  Tag: '%s',  Text: '%s'}", title, tag, text );
        }

    }

    public class SetVariableTwine // c'est un struct plutot ?
    {
        public string toutLeSet;
        public string nomVariable;
        public string valeur; //ou alors une valeur de chaque type ?
        public bool isSttringUnique = false;

        public SetVariableTwine(string texteSet)
        {
            toutLeSet = texteSet;
            // exrait le nom de la variable
            string avantVar = toutLeSet.Substring(texteSet.IndexOf("$") + 1);
            nomVariable = avantVar.Substring(0, avantVar.IndexOf(" "));
            //Debug.Log(nomVariable);
            // valeur de la variable
            string variableBrute = texteSet.Substring(texteSet.IndexOf("to") + 2); //garde que ce qui est après le to
            variableBrute = variableBrute.Remove(variableBrute.Length - 1); // vire la parenthese à la fin
            variableBrute = variableBrute.Trim();
            if (variableBrute[0] == '"' && variableBrute[variableBrute.Length -1] == '"' && !variableBrute[1..^1].Contains('"')) //TODO enregistrer en string valeur simple que si : y’a que deux guillemets, et ils sont au début et à la fin
            {
                variableBrute = variableBrute.Substring(1, variableBrute.Length - 2);
                isSttringUnique = true;
            }
            valeur = variableBrute;
            //Debug.Log(valeur);
        }
    }

    public class GestionVariables
    {
        public static List<SetVariableTwine> CheckForVar(string nodeText)
        {
            List<SetVariableTwine> listeVar = new List<SetVariableTwine>();
            int debuSet = nodeText.IndexOf("(set:");
            while(debuSet != -1)
            {
                int finSet = nodeText.IndexOf(")") + 1;
                listeVar.Add(new SetVariableTwine(nodeText.Substring(debuSet, finSet - debuSet)));
                nodeText = nodeText.Substring(finSet, nodeText.Length - finSet);
                debuSet = nodeText.IndexOf("(set:");
            }

            return (listeVar);
           
        }

        public static void creeVarInit(List<SetVariableTwine> new_listeVar, Dialogue currentDialogue)
        {
            foreach (SetVariableTwine s in new_listeVar)
            {
                if (!currentDialogue.variablesDic.ContainsKey(s.nomVariable)) // si pas encore dans le dico, l'ajoute
                {
                    currentDialogue.variablesDic.Add(s.nomVariable, "");
                }
            }
          
        }

        public static SetVariableTwine SetVar(string texte)
        {
            int finSet = texte.IndexOf(")") + 1;
            var newSetVar = new SetVariableTwine(texte.Substring(0, finSet));
            return (newSetVar);
        }
    }

    public class Dialogue
    {
        string title;
        Dictionary<string, Node> nodes;
        string titleOfStartNode;
        public Dictionary<string, string> variablesDic = new Dictionary<string, string>();
        public Dialogue(TextAsset twineText)
        {
            nodes = new Dictionary<string, Node>();
            ParseTwineText(twineText);
        }

        public Node GetNode(string nodeTitle)
        {
            return nodes[nodeTitle];
        }

        public Node GetStartNode()
        {
            UnityEngine.Assertions.Assert.IsNotNull(titleOfStartNode);
            return nodes[titleOfStartNode];
        }

        public void ParseTwineText(TextAsset twineTextA)
        {
            
            string twineText = twineTextA.ToString();
            string[] nodeData = twineText.Split(new string[] { "::" }, StringSplitOptions.None);
            bool passedHeader = false;
            //const int kIndexOfContentStart = 4;
            for (int i = 0; i < nodeData.Length; i++)
            {
                // The first node comes after the UserStylesheet node
                if (!passedHeader)
                {
                    if (nodeData[i].StartsWith(" UserStylesheet"))
                        passedHeader = true;
                    continue;
                }
               
                // Note: tags are optional
                // Normal Format: "NodeTitle [Tags, comma, seperated] \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]"
                // No-Tag Format: "NodeTitle \r\n Message Text \r\n [[Response One]] \r\n [[Response Two]]"
                string currLineText = nodeData[i];

                // Remove position data
                int posBegin = currLineText.IndexOf("{\"position");
                if (posBegin != -1)
                {
                    int posEnd = currLineText.IndexOf("}", posBegin);
                    currLineText = currLineText.Substring(0, posBegin) + currLineText.Substring(posEnd + 1);
                }

                bool tagsPresent = false;
                // y'a des tags si y'a un [ avant le premier passage à la ligne
                if (currLineText.IndexOf("[") != -1 && currLineText.IndexOf("[") < currLineText.IndexOf("\r\n"))
                {
                    tagsPresent = true;
                }
                int endOfFirstLine = currLineText.IndexOf("\r\n"); // debut du texte, premier passage à la ligne apres les tags

                // Extract Title
                int titleStart = 0;
                int titleEnd = tagsPresent
                    ? currLineText.IndexOf("[")
                    : endOfFirstLine;
                string title = currLineText.Substring(titleStart, titleEnd); //.Trim(); <- responsable du bug de l'espace !
                title = title.Substring(1, title.Length - 2); //enlève un espace au début et à la fin, ce qui correspond à la syntaxe d'entweedle, comme ça si le nom du passage commence par un espace ça le supprime pas comme la fonction trim qui vire tous les espaces

                // Extract Tags (if any)
                string tags = tagsPresent
                    ? currLineText.Substring(titleEnd + 1, (endOfFirstLine - titleEnd) - 2)
                    : "";

                if (!string.IsNullOrEmpty(tags) && tags[tags.Length - 1] == ']')
                    tags = tags.Substring(0, tags.Length - 1);

                // Extract Message Text 
                string fullText = currLineText.Substring(endOfFirstLine).Trim();
                //string messsageText = currLineText.Substring(endOfFirstLine, startOfResponses - endOfFirstLine).Trim();
                //string responseText = currLineText.Substring(startOfResponses).Trim();

                Node curNode = new Node();
                curNode.title = title;
                curNode.fullText = fullText;
                //curNode.text = messsageText;
                curNode.tags = new List<string>(tags.Split(new string[] { " " }, StringSplitOptions.None));

                if (curNode.tags.Contains(kStart))
                {
                    UnityEngine.Assertions.Assert.IsTrue(null == titleOfStartNode);
                    titleOfStartNode = curNode.title;
                }

                SeparateTextAndLinks(curNode);

                // detecte variable et les cree si exite pas 
                List<SetVariableTwine> vars = GestionVariables.CheckForVar(curNode.fullText);
                GestionVariables.creeVarInit(vars,this);

                nodes[curNode.title] = curNode;
               
            }
        }

        public void SeparateTextAndLinks(Node curNode)
        {
            string currLineText = curNode.fullText;
            int startOfResponses;
            int startOfResponseDestinations = currLineText.IndexOf("[[");
            bool lastNode = (startOfResponseDestinations == -1);
            if (lastNode)
                startOfResponses = currLineText.Length;
            else
            {
                // Last new line before "[["
                startOfResponses = currLineText.Substring(0, startOfResponseDestinations).LastIndexOf("\r\n");
            }

            curNode.text = currLineText.Substring(0, startOfResponses).Trim();
            string responseText = currLineText.Substring(startOfResponses).Trim();

            // Note: response messages are optional (if no message then destination is the message)
            // With Message Format: "\r\n [[Message|Response One]]"
            // Message-less Format: "\r\n [[Response One]]"
            curNode.responses = new List<Response>();
            if (!lastNode)
            {
                List<string> responseData = new List<string>(responseText.Split(new string[] { "\r\n" }, StringSplitOptions.None));
                for (int k = 0; k <= responseData.Count - 1; k++)
                {
                    string curResponseData = responseData[k];

                    // si c'est vide ou qu'il n'y a pas de lien
                    if (string.IsNullOrEmpty(curResponseData) || (curResponseData.IndexOf("[[") == -1) || (curResponseData.IndexOf("]]") == -1) )
                    {
                        responseData.RemoveAt(k);
                        continue;
                    }

                    Response curResponse = new Response();
                    int linkStart = curResponseData.IndexOf("[[");
                    int destinationEnd = curResponseData.IndexOf("]]");
                    int destinationStart = curResponseData.IndexOf("|");
                    string destination;
                    // si c'est un lien avec un | (il y a un | et il est avant le ]]
                    if (destinationStart != -1 && destinationStart < destinationEnd) // la destination est le texte entre le | et le ]]
                    {
                        destination = curResponseData.Substring(destinationStart + 1, (destinationEnd - destinationStart) - 1);
                    }
                    else // sinon la destination est tout ce qui est entre les crochets
                    {
                        //Debug.Log(curResponseData.Substring(linkStart + 2));
                        destination = curResponseData.Substring(linkStart + 2, (destinationEnd - (linkStart + 2)));
                    }
                    //UnityEngine.Assertions.Assert.IsFalse(destinationStart == -1, "No destination around in node titled, '" + curNode.title + "'");
                    UnityEngine.Assertions.Assert.IsFalse(destinationEnd == -1, "No destination around in node titled, '" + curNode.title + "'");

                    curResponse.destinationNode = destination;
                    if (destinationStart == -1 || destinationStart > destinationEnd)
                        curResponse.displayText = destination; // If message-less, then message is the name of the passage
                    else
                        curResponse.displayText = curResponseData.Substring(linkStart + 2, destinationStart - (linkStart + 2));
                    curNode.responses.Add(curResponse);
                }
            }
        }

        
    }
}

