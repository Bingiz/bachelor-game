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

    public GameObject PlayerInput, DialoguePartner;

    //public GameObject AnswerText;

    public Scrollbar scrollbar;

    public ConversationalMove Catchall, currentCM;

    public List<InputTag> listOfAllInputTags, listOfAllTopics, listOfAllRegularTags, listOfTopicsInInput;

    public List<ConversationalMove> listOfAllConversationalMoves, listOfStandardConversationalMoves, listOfSpecialCaseConversationalMoves, conversationalMovesInInput;

    public RectTransform windowSize;

    private InputTag[] auxTags;

    [HideInInspector]
    public List<InputTag> listOfTagsInInput;

    private GameManager gameManager;
    [SerializeField]
    private InputTag previousTopic;

    string input;

    string inputForOutput;

    string output;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        // create Tag List to compare with input
        foreach (InputTag g in Resources.LoadAll("InputTags/RegularInputTags", typeof(InputTag)))
        {
            //Debug.Log("InputTag Loaded: " + g.name);
            listOfAllInputTags.Add(g);
            listOfAllRegularTags.Add(g);
        }

        // create Topic list to compare with input
        foreach (InputTag g in Resources.LoadAll("InputTags/Topics", typeof(InputTag)))
        {
            //Debug.Log("Topic Loaded: " + g.name);
            listOfAllTopics.Add(g);
            listOfAllInputTags.Add(g);
        }

        // create Topic list to compare with input
        foreach (InputTag g in Resources.LoadAll("InputTags/AuxInputTags", typeof(InputTag)))
        {
            //Debug.Log("Topic Loaded: " + g.name);
            listOfAllInputTags.Add(g);
        }

        // create Standard CM list to compare with input
        foreach (ConversationalMove g in Resources.LoadAll("Conversational Moves/Standard Moves", typeof(ConversationalMove)))
        {
            //Debug.Log("Conversational Move Loaded: " + g.name);
            listOfAllConversationalMoves.Add(g);
            listOfStandardConversationalMoves.Add(g);
        }

        // create Special Case CM list to compare with input
        foreach (ConversationalMove g in Resources.LoadAll("Conversational Moves/Special Cases", typeof(ConversationalMove)))
        {
            //Debug.Log("Conversational Move Loaded: " + g.name);
            listOfAllConversationalMoves.Add(g);
            listOfSpecialCaseConversationalMoves.Add(g);
        }
        auxTags = new InputTag[2] { Resources.Load<InputTag>("InputTags/AuxInputTags/#Dialogpartner"), Resources.Load<InputTag>("InputTags/AuxInputTags/#Person") };
    }

    // process the player input
    public void SendInputMessage()
    {
        input = PlayerInput.GetComponent<Text>().text;
        input = input.Trim();
        inputForOutput = input;

        DialoguePartner = gameManager.DialoguePartner;

        //send inputForOutput;
        GetComponent<MessageManager>().SendMessageToChat(inputForOutput, Message.MessageType.playerMessage) ;
        input = input.ToLower();

        

        AddInputTags();
        CheckDialoguePartner();
        CheckPerson();

        DecideTopic();

        outputAnswer();

        listOfTagsInInput.Clear();
    }

    // map player input to Input Tags
    public void AddInputTags()
    {
        for (int j = 0; j < listOfAllInputTags.Count; j++)
        {
            for (int k = 0; k < listOfAllInputTags[j].associatedStrings.Count; k++)
            {
                Regex matchWords = new Regex(@"\b" + listOfAllInputTags[j].associatedStrings[k] + @"\b");

                if (matchWords.Match(input).Value == listOfAllInputTags[j].associatedStrings[k])
                {
                    if (! listOfTagsInInput.Contains(listOfAllInputTags[j]))
                    {
                        listOfTagsInInput.Add(listOfAllInputTags[j]);
                        Debug.Log("Input Tag " + listOfAllInputTags[j].name + " added");
                        break;
                    }
                }
            }
        }
    }

    public void CheckPerson()
    {
        //check Person Input Tag
        //check DialoguePartner Input Tag

        if (listOfTagsInInput.Contains(auxTags[1]))
        {
            Debug.Log("Person Tag Found in Input");
            listOfTagsInInput.Remove(auxTags[1]);
            //Debug.Log(auxTags[1].name + " removed from InputTags in input.");

            if (previousTopic != null)
            {
                bool inInput = false;

                for (int i = 0; i < listOfTagsInInput.Count; i++)
                {
                    if (previousTopic.name == "Adelaide Shackleton" ||
                    listOfTagsInInput[i].name == "Lawrence Wilson" ||
                    listOfTagsInInput[i].name == "Edgar Evans" ||
                    listOfTagsInInput[i].name == "Alexander Veltman" ||
                    listOfTagsInInput[i].name == "Amelie Veltman")
                    {
                        inInput = true;
                        break;
                    }
                }

                if (!inInput &&
                    previousTopic.name == "Adelaide Shackleton" ||
                    previousTopic.name == "Lawrence Wilson" ||
                    previousTopic.name == "Edgar Evans" ||
                    previousTopic.name == "Alexander Veltman" ||
                    previousTopic.name == "Amelie Veltman"
                    )
                {
                    listOfTagsInInput.Add(previousTopic);
                    Debug.Log("Input Tag" + previousTopic + " added");
                }
            }
        }
    }

    public void CheckDialoguePartner()
    {
        //check DialoguePartner Input Tag
        if (listOfTagsInInput.Contains(auxTags[0]) /* && listOfTagsInInput.Count == 1 */)
        {
            listOfTagsInInput.Remove(auxTags[0]);
            Debug.Log(auxTags[0].name + " removed from InputTags in input.");

            InputTag characterIT = DialoguePartner.GetComponent<CharacterDialogueInfos>().characterInputTag;

            if (! listOfTagsInInput.Contains(characterIT))
            {
                listOfTagsInInput.Add(characterIT);
                Debug.Log(characterIT + " added");
            }
            else
            {
                Debug.Log(characterIT + " Dialogue Partner InputTag already in input.");
            }
        }
    }

    public void DecideTopic()
    {

        //check which Topic Tags are available in Input and write them in List
        for (int i = 0; i < listOfAllTopics.Count; i++)
        {
            for (int j = 0; j < listOfTagsInInput.Count; j++)
            {
                //Debug.Log("Comparing Input Tag: "+ listOfAllTopics[i].name +" to "+ listOfTagsInInput[j].name);
                if (listOfAllTopics[i].name == listOfTagsInInput[j].name)
                {
                    Debug.Log("Input Tag: " + listOfAllTopics[i].name + " added to list of Topics in Input");
                    listOfTopicsInInput.Add(listOfTagsInInput[j]);
                }
            } 
        }

        if (listOfTopicsInInput.Count == 0)
        {
            Debug.Log("No Topic found. Adding: " + Resources.Load("InputTags/Topics/#NO_TOPIC", typeof(InputTag)));
            listOfTopicsInInput.Add(Resources.Load("InputTags/Topics/#NO_TOPIC", typeof(InputTag)) as InputTag);
        }
    }

    public List<ConversationalMove> GetStandardConversationalMovesFromInput()
    {
        for (int i = 0; i < listOfStandardConversationalMoves.Count; i++)
        {
            for (int j = 0; j < listOfStandardConversationalMoves[i].associatedTagCombinations.Length; j++)
            {

                Debug.Log("Comparing: " + listOfStandardConversationalMoves[i].associatedTagCombinations[j].InputTags[0].name);

                    for (int l = 0; l < listOfStandardConversationalMoves[i].associatedTagCombinations[j].InputTags.Length; l++)
                    {
                        bool temp = true;
                        if (!listOfTagsInInput.Contains(listOfStandardConversationalMoves[i].associatedTagCombinations[j].InputTags[l]))
                        {
                            temp = false;
                        }
                        if (temp)
                        {
                            conversationalMovesInInput.Add(listOfStandardConversationalMoves[i]);
                            Debug.Log("CM added : " + listOfStandardConversationalMoves[i].name);
                        }
                    }
            }
        }

        if (conversationalMovesInInput.Count == 0)
        {
                conversationalMovesInInput.Add(Catchall);
                Debug.Log("CM added: " + Catchall);
                return conversationalMovesInInput;
        }
        else
        {
            List<ConversationalMove> conversationalMovesInInputOrdered = conversationalMovesInInput.OrderByDescending(e => e.priority).ToList();
            return conversationalMovesInInputOrdered;
        }
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
        if (conversationalMovesInInput.Count > 0)
        {
            List<ConversationalMove> conversationalMovesInInputOrdered = conversationalMovesInInput.OrderByDescending(e => e.priority).ToList();
            Debug.Log("SCM");
            return conversationalMovesInInputOrdered;
        }
        else
        {
            Debug.Log("no SCM");
            return null;
        }
        
    }

    public List<InputTag> GetTopicsFromInput()
    {
        if (listOfTopicsInInput.Count > 0)
        {
            List<InputTag> topicsInInputOrdered = listOfTopicsInInput.OrderByDescending(e => e.priority).ToList();

            return topicsInInputOrdered;
        }
        else
        {
            return null;
        }

    }

    public List<Context> OrderContexts()
    {
        if (gameManager.currentContexts.Count > 0)
        {
            List<Context> contextsOrdered = gameManager.currentContexts.OrderByDescending(e => e.priority).ToList();

            return contextsOrdered;
        }
        else
        {
            return null;
        }

    }

    // return a answer based on current contexts and Input Tags
    public string FindAnswerOfHighestPriority()
    {
        //// CONTEXT ////
        List<Context> contextsToSearch = OrderContexts();

        //// CONVERSATIONAL MOVE ////
        List<ConversationalMove> CMsToSearch = GetStandardConversationalMovesFromInput();

        //// SPECIAL CASE CONVERSATIONAL MOVE ////
        List<ConversationalMove> SCMsToSearch = GetSpecialCaseCMFromInput();

        //// TOPIC ////
        List<InputTag> topicsToSearch = GetTopicsFromInput();


        for (int h = 0; h < gameManager.currentContexts.Count; h++)
        { 
            int countTOPIC = topicsToSearch.Count + 1;
            Debug.Log("Highest Context is: " + contextsToSearch[0].name);
            ///////////////// FOR SPECIAL CASE CONVERSATIONAL MOVES
            if (GetSpecialCaseCMFromInput() != null)
            {
                for (int top = 0; top < countTOPIC; top++)
                {
                    
                    // search every CM in currently highest CONTEXT
                    for (int j = 0; j < contextsToSearch[0].specialCases.Count; j++)
                    {
                        int countSCM = SCMsToSearch.Count + 1;

                        // search all available CMs in priority order
                        for (int jj = 0; jj < countSCM; jj++)
                        {
                            // Check if answer exists for highest priority Topic in highest Priority SCM
                            for (int m = 0; m < contextsToSearch[0].specialCases.Count; m++)
                            {
                                Debug.Log(SCMsToSearch);
                                if (SCMsToSearch[0] == contextsToSearch[0].specialCases[m].conversationalMoveObject)
                                {
                                    for (int n = 0; n < contextsToSearch[0].specialCases[m].topics.Count; n++)
                                    {
                                        if (contextsToSearch[0].specialCases[m].topics[n].topic == topicsToSearch[0])
                                        {
                                            if (contextsToSearch[0].specialCases[m].topics[n].answers != null)
                                            {
                                                topicsToSearch.Clear();
                                                currentCM = null;
                                                previousTopic = contextsToSearch[0].specialCases[m].topics[n].topic;
                                                contextsToSearch[0].specialCases[m].topics[n].DialogueEvent.Invoke();

                                                //output the answers in order
                                                if (contextsToSearch[0].specialCases[m].topics[n].answercount >= contextsToSearch[0].specialCases[m].topics[n].answers.Length)
                                                {
                                                    contextsToSearch[0].specialCases[m].topics[n].answercount = 0; //if every answer is output once, restart cycle
                                                }

                                                string temp = "";

                                                if (contextsToSearch[0].specialCases[m].topics[n].answers.Length > 0)
                                                {
                                                    contextsToSearch[0].specialCases[m].topics[n].answercount++;
                                                    temp = contextsToSearch[0].specialCases[m].topics[n].answers[contextsToSearch[0].specialCases[m].topics[n].answercount - 1];

                                                }

                                                return temp;
                                                
                                            }
                                        }
                                    }
                                }
                            }
                            //if no match for currentlyHighestTopic is found in all available Conversational Moves --> try next highest Topic
                            SCMsToSearch.Remove(SCMsToSearch[0]);
                            if (SCMsToSearch.Count == 0)
                            {
                                SCMsToSearch.Add(Catchall);
                            }
                        }

                        // repopulate the Search list if no answer is found
                        SCMsToSearch.Clear();
                        SCMsToSearch.AddRange(GetSpecialCaseCMFromInput());
                    }
                    topicsToSearch.Remove(topicsToSearch[0]);

                    if (topicsToSearch.Count == 0)
                    {
                        topicsToSearch.Add(Resources.Load("InputTags/Topics/#NO_TOPIC", typeof(InputTag)) as InputTag);
                    }
                }
            }

///////////////// FOR REGULAR CONVERSATIONAL MOVES
    
            for (int top = 0; top < countTOPIC; top++)
            {
                // search every CM in currently highest CONTEXT
                for (int j = 0; j < contextsToSearch[0].listOfConversationalMoves.Count; j++)
                {
                    int countCM = CMsToSearch.Count + 1;

                    // search all available CMs in priority order
                    for (int jj = 0; jj < countCM; jj++)
                    {
                        // Check if answer exists for highest priority Topic in highest Priority CM
                        for (int m = 0; m < contextsToSearch[0].listOfConversationalMoves.Count; m++)
                        {
                            if (CMsToSearch[0] == contextsToSearch[0].listOfConversationalMoves[m].conversationalMoveObject)
                            {
                                for (int n = 0; n < contextsToSearch[0].listOfConversationalMoves[m].topics.Count; n++)
                                {
                                    if (contextsToSearch[0].listOfConversationalMoves[m].topics[n].topic == topicsToSearch[0])
                                    {
                                        topicsToSearch.Clear();
                                        currentCM = contextsToSearch[0].listOfConversationalMoves[m].conversationalMoveObject;
                                        previousTopic = contextsToSearch[0].listOfConversationalMoves[m].topics[n].topic;
                                        contextsToSearch[0].listOfConversationalMoves[m].topics[n].DialogueEvent.Invoke();

                                        //output the answers in order
                                        if (contextsToSearch[0].listOfConversationalMoves[m].topics[n].answercount >= contextsToSearch[0].listOfConversationalMoves[m].topics[n].answers.Length)
                                        {
                                            contextsToSearch[0].listOfConversationalMoves[m].topics[n].answercount = 0; //if every answer is output once, restart cycle
                                        }

                                        contextsToSearch[0].listOfConversationalMoves[m].topics[n].answercount++;

                                        string temp = "";

                                        if (contextsToSearch[0].listOfConversationalMoves[m].topics[n].answers.Length > 0)
                                        {
                                             temp = contextsToSearch[0].listOfConversationalMoves[m].topics[n].answers[contextsToSearch[0].listOfConversationalMoves[m].topics[n].answercount - 1];

                                        }

                                        return temp;
                                    }
                                }
                            }
                        }
                        //if no match for currentlyHighestTopic is found in all available Conversational Moves --> try next highest Topic
                        CMsToSearch.Remove(CMsToSearch[0]);
                        if (CMsToSearch.Count == 0)
                        {
                            CMsToSearch.Add(Catchall);
                        }

                    }

                    // repopulate the Search list if no answer is found
                    CMsToSearch.Clear();
                    CMsToSearch.AddRange(GetStandardConversationalMovesFromInput());
                }
                topicsToSearch.Remove(topicsToSearch[0]);

                if (topicsToSearch.Count == 0)
                {
                    topicsToSearch.Add(Resources.Load("InputTags/Topics/#NO_TOPIC", typeof(InputTag)) as InputTag);
                }
            }
            topicsToSearch.Clear();
            topicsToSearch.AddRange(GetTopicsFromInput());

            contextsToSearch.Remove(contextsToSearch[0]);
        }

        contextsToSearch.Clear();
        topicsToSearch.Clear();
        CMsToSearch.Clear();
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
        /*
        string[] answerBuffer = null;

        answerBuffer = FindAnswerOfHighestPriority();
        

        if (answerBuffer.Length > 0)
        {
            Debug.Log(answerBuffer[0]);
            //output = answerBuffer[Random.Range(0, answerBuffer.Length)];
            output = answerBuffer[];
        }
        */


        string answerBuffer = null;

        answerBuffer = FindAnswerOfHighestPriority();


        if (answerBuffer != null)
        {
            output = answerBuffer;

            DialoguePartner.GetComponent<Transform>().GetChild(0).GetComponent<Animator>().SetTrigger("Answer_"+ Random.Range(1, 3).ToString());
            //Debug.Log("Answer_" + Random.Range(1, 3).ToString());
            //Debug.Log(DialoguePartner.GetComponent<Transform>().GetChild(0).GetComponent<Animator>());
        }


        //CLEAR ALL LISTS
        listOfTopicsInInput.Clear();
        conversationalMovesInInput.Clear();

        //write the output
        Debug.Log("Output: " + output +"////////////////////////");

        GetComponent<MessageManager>().SendMessageToChat(output, DialoguePartner.GetComponent<CharacterDialogueInfos>().messageType);
        gameManager.DialoguePartner.GetComponent<EnterDialogue>().UpdateContexts();
        // clear the output buffer and empty the list of recieved words
        output = "";
        answerBuffer = null;
    }

    // displays greeting Message if conversation with NPC is started
    public void Greeting()
    {
            string[] answerBuffer = GreetHighestPriorityContext();

            output = answerBuffer[Random.Range(0, answerBuffer.Length)];

            GetComponent<MessageManager>().SendMessageToChat(output, DialoguePartner.GetComponent<CharacterDialogueInfos>().messageType);

        if (answerBuffer[0] == "") {
            answerBuffer = null;
        }
        else { Debug.Log("no greeting available"); }

    }

    // check which greeting Message to display
    public string[] GreetHighestPriorityContext()
    {
        List<Context> contextsToSearch = OrderContexts();

            //check which of the current contexts has the highest priority and put the one into currentlyHighestContext
            for (int i = 0; i < contextsToSearch.Count; i++)
            {
                    if (contextsToSearch[0].greetings.Length > 0)
                    {
                    contextsToSearch[0].GreetEvent.Invoke();
                    return contextsToSearch[0].greetings;
                    }
            }

        contextsToSearch.Clear();
        Debug.Log("No Greet answer found");
        string[] temp = new string[] { "" };
        return temp;
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