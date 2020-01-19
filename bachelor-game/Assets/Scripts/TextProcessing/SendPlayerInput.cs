using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

    //new parsing whole input text for Tags
public class SendPlayerInput : MonoBehaviour
{

    public GameObject PlayerInput;

    public GameObject AnswerText;

    public GameObject PlayerInputOutput;

    public GameObject DialoguePartner;

    public ConversationalMove Catchall; 

    public List<InputTag> listOfAllInputTags;
    public List<InputTag> listOfAllTopics;
    public List<InputTag> listOfAllAuxTags;
    public List<ConversationalMove> listOfAllConversationalMoves;
    public List<ConversationalMove> listOfStandardConversationalMoves;
    public List<ConversationalMove> listOfSpecialCaseConversationalMoves;

    [HideInInspector] public List<InputTag> listOfTagsInInput;

    private GameManager gameManager;

    string input;

    string inputForOutput;

    string output;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        // create Tag List to compare with input
        foreach(InputTag g in Resources.LoadAll("InputTags", typeof(InputTag)))
        {
            Debug.Log("InputTag Loaded: " + g.name);
            listOfAllInputTags.Add(g);
            listOfAllAuxTags.Add(g);
        }

        // create Topic list to compare with input
        foreach (InputTag g in Resources.LoadAll("Topics", typeof(InputTag)))
        {
            Debug.Log("Topic Loaded: " + g.name);
            listOfAllTopics.Add(g);
            listOfAllInputTags.Add(g);
        }

        // create Standard CM list to compare with input
        foreach (ConversationalMove g in Resources.LoadAll("Conversational Moves/Standard Moves", typeof(ConversationalMove)))
        {
            Debug.Log("Conversational Move Loaded: " + g.name);
            listOfAllConversationalMoves.Add(g);
            listOfStandardConversationalMoves.Add(g);
        }

        // create Special Case CM list to compare with input
        foreach (ConversationalMove g in Resources.LoadAll("Conversational Moves/Special Cases", typeof(ConversationalMove)))
        {
            Debug.Log("Conversational Move Loaded: " + g.name);
            listOfAllConversationalMoves.Add(g);
            listOfSpecialCaseConversationalMoves.Add(g);
        }
    }

    // process the player input
    public void SendInputMessage()
    {
        input = PlayerInput.GetComponent<Text>().text;
        input = input.Trim();
        inputForOutput = input;
        input = input.ToLower();

        AddInputTags();
        DecideTopic();


       // Match CM and Topic --> return answer.

        outputAnswer();

        listOfTagsInInput.Clear();
    }

    // map player input to Input Tags
    public void AddInputTags()
    {
        for (int j = 0; j < listOfAllInputTags.Count; j++)
        {
            bool tagFoundForWord = false;

            for (int k = 0; k < listOfAllInputTags[j].associatedStrings.Count && tagFoundForWord == false; k++)
            {   
                Regex matchWords = new Regex(@"\b"+ listOfAllInputTags[j].associatedStrings[k] +@"\b");

                if (matchWords.Match(input).Value == listOfAllInputTags[j].associatedStrings[k])
                {
                        listOfTagsInInput.Add(listOfAllInputTags[j]);
                        Debug.Log(listOfAllInputTags[j].name + " added");
                        tagFoundForWord = true;
                        break;
                }
            }
        }
    }

    public void DecideTopic()
    {
        List<InputTag> listOfTopicsInInput = new List<InputTag>();

        //check which Topic Tags are available in Input and write them in List
        for (int i = 0; i < listOfAllTopics.Count; i++)
        {
            for (int j = 0; j < listOfTagsInInput.Count; j++)
            {
                if (listOfAllTopics[i].name == listOfTagsInInput[j].name)
                {
                    listOfTopicsInInput.Add(listOfTagsInInput[j]);
                }
            }
            
        }

        //decide which Topic is the most relevant (priority?)
        // If current Topic is found in ListOfTopicsInInput --> Topic is used again

        bool continueTopic = false;

        for (int i = 0; i < listOfTopicsInInput.Count; i++)
        {
            if(listOfTopicsInInput[i] == gameManager.currentTopic)
            {
                continueTopic = true;
            }
        }

        if (!continueTopic)
        {
            //check for highest priority Topic
            InputTag currentlyHighestTopic = ScriptableObject.CreateInstance<InputTag>();

            for (int i = 0; i < listOfTopicsInInput.Count; i++)
            {
                if(listOfTopicsInInput[i].priority > currentlyHighestTopic.priority)
                {
                    currentlyHighestTopic = listOfTopicsInInput[i];
                }

                //if two Tags have the same highest priority --> pick one random
                if(listOfTopicsInInput[i].priority == currentlyHighestTopic.priority)
                {
                    bool random = (Random.value > 0.5f);
                    if (random)
                    {
                        currentlyHighestTopic = listOfTopicsInInput[i];
                    }
                }
            }

            if(currentlyHighestTopic != null)
            {
                gameManager.currentTopic = currentlyHighestTopic;
            }
        }
        else
        {
            gameManager.currentTopic = null;
        }

    }

    public List<ConversationalMove> GetStandardConversationalMovesFromInput()
    {
        List<ConversationalMove> conversationalMovesInInput = new List<ConversationalMove>();

        for (int i = 0; i < listOfStandardConversationalMoves.Count; i++)
        {
            for (int j = 0; j < listOfStandardConversationalMoves[i].associatedTagCombinations.Length; j++)
            {
                if(UnorderedEqual(listOfStandardConversationalMoves[i].associatedTagCombinations[j].InputTags, listOfTagsInInput))
                {
                    conversationalMovesInInput.Add(listOfStandardConversationalMoves[i]);
                }
            }
        }

        if (conversationalMovesInInput.Count > 0)
        {
            conversationalMovesInInput.Add(Catchall);
            Debug.Log("Catchall CM added");
        }

        List<ConversationalMove> conversationalMovesInInputOrdered = conversationalMovesInInput.OrderBy(e => e.priority).ToList();

        return conversationalMovesInInputOrdered;

    }

    public List<ConversationalMove> GetSpecialCaseCMFromInput()
    {
        List<ConversationalMove> conversationalMovesInInput = new List<ConversationalMove>();

        for (int i = 0; i < listOfSpecialCaseConversationalMoves.Count; i++)
        {
            for (int j = 0; j < listOfSpecialCaseConversationalMoves[i].associatedTagCombinations.Length; j++)
            {
                if (UnorderedEqual(listOfSpecialCaseConversationalMoves[i].associatedTagCombinations[j].InputTags, listOfTagsInInput))
                {
                    conversationalMovesInInput.Add(listOfSpecialCaseConversationalMoves[i]);
                }
            }
        }

        List<ConversationalMove> conversationalMovesInInputOrdered = conversationalMovesInInput.OrderBy(e => e.priority).ToList();

        return conversationalMovesInInputOrdered;
    }

    public void MatchTopicAndCM()
    {

    }

    // return a answer based on current contexts and Input Tags
    public string[] FindAnswerOfHighestPriority()
    {
        List<Context> contextsToSearch = new List<Context>();

        contextsToSearch.AddRange(gameManager.currentContexts);

        //find highest priority context

        for (int h = 0; h < gameManager.currentContexts.Count; h++)
        {
            Context currentlyHighestContext = ScriptableObject.CreateInstance<Context>();
            List<ConversationalMove> CMBuffer = GetStandardConversationalMovesFromInput();
            List<ConversationalMove> SCMBuffer = GetSpecialCaseCMFromInput();

            //check which of the current contexts has the highest priority and put the one into currentlyHighestContext
            for (int i = 0; i < contextsToSearch.Count; i++)
            {
                if (contextsToSearch[i].priority > currentlyHighestContext.priority)
                {
                    currentlyHighestContext = contextsToSearch[i];
                }

                //Check Special Cases first
                if(currentlyHighestContext.specialCases != null)
                {
                    for (int j = 0; j < currentlyHighestContext.specialCases.Length; j++)
                    {
                        for (int k = 0; k < SCMBuffer.Count; k++)
                        {
                            if (currentlyHighestContext.specialCases[j].conversationalMoveObject == SCMBuffer[k])
                            {
                                for (int l = 0; l < currentlyHighestContext.specialCases[j].topics.Length; l++)
                                {
                                    if (currentlyHighestContext.specialCases[j].topics[l].topic == gameManager.currentTopic)
                                    {
                                        if (currentlyHighestContext.specialCases[j].topics[l].answers != null)
                                        {
                                            return currentlyHighestContext.specialCases[j].topics[l].answers;
                                        }
                                    }
                                }
                            }
                        }
                    }  
                }

                //check the tagResponseCombinations of the currentlyHighestContext against the tags in Input
                for (int j = 0; j < currentlyHighestContext.listOfConversationalMoves.Length; j++)
                {
                    for (int k = 0; k < CMBuffer.Count; k++)
                    {
                        if(currentlyHighestContext.listOfConversationalMoves[j].conversationalMoveObject == CMBuffer[k])
                        {
                            for (int l = 0; l < currentlyHighestContext.listOfConversationalMoves[j].topics.Length; l++)
                            {
                                if (currentlyHighestContext.listOfConversationalMoves[j].topics[l].topic == gameManager.currentTopic)
                                {
                                    if(currentlyHighestContext.listOfConversationalMoves[j].topics[l].answers != null)
                                    {
                                        return currentlyHighestContext.listOfConversationalMoves[j].topics[l].answers;
                                    }
                                }
                            }
                        }
                    }
                }
                //Problematic
                contextsToSearch.Remove(currentlyHighestContext);
            }

        }
        contextsToSearch.Clear();
        return null;
    }

    // stem player input
    public void stemInputMessage(List<string> playerInput)
    {
        string temp;
        for (int i = 0; i < playerInput.Count; i++)
        {

            if (GetComponent<StemDictionary>().stemDictionary.TryGetValue(playerInput[i], out temp))
            {
                playerInput[i] = GetComponent<StemDictionary>().stemDictionary[playerInput[i]];
                Debug.Log("stemmed word: " + playerInput[i]);
            }
        }
    }

    // outputs the answer 
    public void outputAnswer()
    {
        string[] answerBuffer = null;

        answerBuffer = FindAnswerOfHighestPriority();

        if (answerBuffer != null)
        {
            output = answerBuffer[Random.Range(0, answerBuffer.Length)];
        }
        //write the output

        DialoguePartner.GetComponent<CharacterDialogueInfos>().playerInputHistory += inputForOutput + "\n\n\n";
        WriteNPCLine(output);
        //AnswerText.GetComponent<Text>().text = output;

        // clear the output buffer and empty the list of recieved words
        output = "";
        answerBuffer = null;
    }

    // sends the NPC answer dialogue line to the UI
    public void WriteNPCLine(string output)
    {
        DialoguePartner = gameManager.DialoguePartner;

        //Write and Call new Message Function (so that text box size matches)
        DialoguePartner.GetComponent<CharacterDialogueInfos>().responseHistory += "\n" + output + "\n\n";
    }

    // displays greeting Message if conversation with NPC is started
    public void Greeting()
    {
        if (GreetHighestPriorityContext() != null)
        {
            string[] answerBuffer = GreetHighestPriorityContext();

            output = answerBuffer[Random.Range(0, answerBuffer.Length)];

            WriteNPCLine(output);
            DialoguePartner.GetComponent<CharacterDialogueInfos>().playerInputHistory += "\n\n\n";
        }
        else { Debug.Log("no greeting available");}

    }

    // check which greeting Message to display
    public string[] GreetHighestPriorityContext()
    {
        List<Context> contextsToSearch = new List<Context>();

        contextsToSearch.AddRange(gameManager.currentContexts);

        //find highest priority context

        for (int h = 0; h < gameManager.currentContexts.Count; h++)
        {
            Context currentlyHighestContext = ScriptableObject.CreateInstance<Context>();

            //check which of the current contexts has the highest priority and put the one into currentlyHighestContext
            for (int i = 0; i < contextsToSearch.Count; i++)
            {
                if (contextsToSearch[i].priority > currentlyHighestContext.priority)
                {
                    currentlyHighestContext = contextsToSearch[i];

                    if (currentlyHighestContext.greetings != null)
                    {
                        return currentlyHighestContext.greetings;
                    }
                }
            }

        }
        contextsToSearch.Clear();
        Debug.Log("No answer found");
        return null;

        //gameManager.currentContexts[0].tagResponseCombinations[0].inputTags[0]
    }

    //https://www.dotnetperls.com/list-equals
    static bool UnorderedEqual<T>(ICollection<T> a, ICollection<T> b)
    {
        // 1
        // Require that the counts are equal
        if (a.Count != b.Count)
        {
            return false;
        }
        // 2
        // Initialize new Dictionary of the type
        Dictionary<T, int> d = new Dictionary<T, int>();
        // 3
        // Add each key's frequency from collection A to the Dictionary
        foreach (T item in a)
        {
            int c;
            if (d.TryGetValue(item, out c))
            {
                d[item] = c + 1;
            }
            else
            {
                d.Add(item, 1);
            }
        }
        // 4
        // Add each key's frequency from collection B to the Dictionary
        // Return early if we detect a mismatch
        foreach (T item in b)
        {
            int c;
            if (d.TryGetValue(item, out c))
            {
                if (c == 0)
                {
                    return false;
                }
                else
                {
                    d[item] = c - 1;
                }
            }
            else
            {
                // Not in dictionary
                return false;
            }
        }
        // 5
        // Verify that all frequencies are zero
        foreach (int v in d.Values)
        {
            if (v != 0)
            {
                return false;
            }
        }
        // 6
        // We know the collections are equal
        return true;
    }



}