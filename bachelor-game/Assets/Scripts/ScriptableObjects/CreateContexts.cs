using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

public class CreateContexts : MonoBehaviour
{
    public string contextName;
    public enum Character { Adelaide_Shackleton,Edgar_Evans,Lawrence_Wilson};
    private Character character;

    List<Context.Topic> listOfTopics;
    Context.CM[] listOfConversationalMoves;

    public enum CMType
    {
        CM,
        SCM,
        GREET
    }
    private CMType cmtype;

    public void CreateContext()
    {

        if (contextName != null)
        {

            // set character variable;
            if (contextName.Contains("AK_"))
            {
                character = Character.Adelaide_Shackleton;
            }
            else if (contextName.Contains("EK_"))
            {
                character = Character.Edgar_Evans;
            }
            else if (contextName.Contains("LK_"))
            {
                character = Character.Lawrence_Wilson;
            }


            // load the .CSV
            string fileData = File.ReadAllText("Assets/Resources/Answers/" + contextName + ".csv");

            string[] lineData = fileData.Split("\n"[0]);

            int spaltenAnzahl = lineData[0].Split(","[0]).Length;

            

            //string[] cellData = lineData[0].Trim().Split(","[0]);

            string[,] processedData = new string[spaltenAnzahl, lineData.Length];

            Debug.Log("Spalten Anzahl: " + spaltenAnzahl);
            Debug.Log("Zeilen Anzahl: " + lineData.Length);

            
            for (int y = 0; y < lineData.Length; y++)
            {
                for (int x = 0; x < spaltenAnzahl; x++)
                {
                    string[] temp = lineData[y].Trim().Split(","[0]);
                    processedData[x, y] = temp[x];
                    //Debug.Log("[" + x + "," + y + "]:" + processedData[x, y]);
                }
                //Debug.Log("//////////////////////////////////////LINE COMPLETE");
            }

            // create a Context that needs to be fed the data from the CSV
            Context context = ScriptableObject.CreateInstance<Context>();
            context.name = contextName;
            context.listOfConversationalMoves = new List<Context.CM>();
            context.specialCases = new List<Context.CM>();

            string prevCM = "";

            for (int yy = 0; yy < lineData.Length; yy++)
            {
                if (processedData[1, yy].Contains("SCM_"))
                {
                    cmtype = CMType.SCM;
                }
                else if (processedData[1, yy].Contains("GREET"))
                {
                    cmtype = CMType.GREET;
                }
                else
                {
                    cmtype = CMType.CM;
                }

                if (processedData[1, yy] != prevCM && processedData[1, yy] != "")
                {
                    //To Check if the Concersational Move is still the same
                    prevCM = processedData[1, yy];

                    ConversationalMove newConversationalMove = new ConversationalMove();
                    // initializing all the variables for the CM Object
                    string newCMName = processedData[1, yy];
                    switch (cmtype)
                    {
                        case CMType.CM:
                            newConversationalMove = Resources.Load<ConversationalMove>("Conversational Moves/Standard Moves/" + processedData[1, yy]);
                            //Debug.Log(newConversationalMove);
                            break;

                        case CMType.SCM:
                            newConversationalMove = Resources.Load<ConversationalMove>("Conversational Moves/Special Cases/" + processedData[1, yy]);
                            break;

                        case CMType.GREET:
                            break;

                        default:
                            break;
                    }
                    

                    List<Context.Topic> newListOfTopics = new List<Context.Topic>();

                    // Creating the CM Object
                    Context.CM newCM = new Context.CM(newCMName, newConversationalMove, newListOfTopics);

                    
                    switch (cmtype)
                    {
                        case CMType.CM:
                            Debug.Log("CM added: " + newCMName);
                            context.listOfConversationalMoves.Add(newCM);
                            break;

                        case CMType.SCM:
                            Debug.Log("SCM added: " + newCMName);
                            context.specialCases.Add(newCM);
                            break;

                        case CMType.GREET:
                            break;

                        default:
                            break;
                    }

                }

                if (prevCM != "")
                {
                    // initializing all the variables for the Topic Object
                    string newTopicName = processedData[2, yy];
                    InputTag newTopicObject = Resources.Load<InputTag>("InputTags/Topics/" + processedData[2, yy]);

                    int numberOfAnswers = 0;

                    while (processedData[4 + numberOfAnswers, yy] != "" && numberOfAnswers + 4 < spaltenAnzahl-1)
                    {
                        numberOfAnswers++;
                    }

                    string[] newAnswers = new string[numberOfAnswers];

                    for (int no = 0; no < numberOfAnswers; no++)
                    {
                        newAnswers[no] = processedData[4 + no, yy];
                    }

                    Context.Topic newTopic = new Context.Topic(newTopicName, newTopicObject, newAnswers);
                    //Debug.Log(newTopic.name);

                    switch (cmtype)
                    {
                        case CMType.CM:
                            context.listOfConversationalMoves[context.listOfConversationalMoves.Count - 1].topics.Add(newTopic);
                            break;

                        case CMType.SCM:
                            context.specialCases[context.specialCases.Count - 1].topics.Add(newTopic);
                            break;

                        case CMType.GREET:
                            break;

                        default:
                            break;
                    }

                }

            }

            // create the Context
            AssetDatabase.CreateAsset(context, "Assets/Resources/Contexts/" + character + "/" + contextName + ".asset");

            //Debug.Log(fileData);
            /*
            InputTag[] allTopics = Resources.LoadAll<InputTag>("InputTags/Topics");
            int countOfTopics = allTopics.Length;
            listOfTopics = new List<Context.Topic>();

            ConversationalMove[] allCM = Resources.LoadAll<ConversationalMove>("Conversational Moves/Standard Moves");
            int countOfConversationalMoves = allCM.Length;
            listOfConversationalMoves = new Context.CM[countOfConversationalMoves];

            int i = 0;
            foreach (InputTag top in Resources.LoadAll("InputTags/Topics", typeof(InputTag)))
            {
                //listOfTopics[i] = new Context.Topic(top.name, top);
                i++;
            }

            int j = 0;
            foreach (ConversationalMove cmove in Resources.LoadAll("Conversational Moves/Standard Moves", typeof(ConversationalMove)))
            {
                listOfConversationalMoves[j] = new Context.CM(cmove.name, cmove, listOfTopics);
                j++;
            }
            */

        }

    }

}
