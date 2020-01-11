using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;

    //new parsing whole input text for Tags
public class SendPlayerInput : MonoBehaviour
{

    public GameObject PlayerInput;

    public GameObject AnswerText;

    public GameObject PlayerInputOutput;

    public GameObject DialoguePartner;

    public List<InputTag> listOfAllInputTags;

    [HideInInspector] public List<InputTag> listOfTagsInInput;

    private GameManager gameManager;

    string input;

    string inputForOutput;

    string output;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        //listOfAllInputTags = gameManager.listOfAllInputTags;

        //listOfAllInputTags.AddRange(Resources.LoadAll("InputTags", typeof(InputTag)));
        foreach(InputTag g in Resources.LoadAll("InputTags", typeof(InputTag)))
        {
            Debug.Log("prefab found: " + g.name);
            listOfAllInputTags.Add(g);
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

        outputAnswer();

        listOfTagsInInput.Clear();
    }

    // map player input to Input Tags
    public void AddInputTags()
    {
        //test if input String contains a question mark
        if(input.Contains("?"))
        {
            listOfTagsInInput.Add(listOfAllInputTags.Find(x => x.name.Contains("#Frage")));
            Debug.Log(listOfAllInputTags.Find(x => x.name.Contains("#Frage")).name + " added"); //add question tag to list
        }

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

    // return a answer based on current contexts and Input Tags
    public string[] FindAnswerOfHighestPriorityContext()
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

                }
            }

            //check the tagResponseCombinations of the currentlyHighestContext against the tags in Input
            for (int j = 0; j < currentlyHighestContext.tagResponseCombinations.Length; j++)
            {
                for (int k = 0; k < currentlyHighestContext.tagResponseCombinations[j].tagList.Length; k++)
                {

                    //compare each Tag from tagResponseCombination[j] in current Context with each Tag from the Player Input
                    if (UnorderedEqual(currentlyHighestContext.tagResponseCombinations[j].tagList[k].inputTags, listOfTagsInInput))
                    {
                        currentlyHighestContext.tagResponseCombinations[j].askedBefore = true;

                        //add contexts
                        if (currentlyHighestContext.tagResponseCombinations[j].addContexts != null)
                        {
                            DialoguePartner.GetComponent<DialogueTrigger>().context.AddRange(currentlyHighestContext.tagResponseCombinations[j].addContexts);
                            DialoguePartner.GetComponent<EnterDialogue>().UpdateContexts();
                        }

                        //remove contexts
                        if (currentlyHighestContext.tagResponseCombinations[j].removeContexts != null)
                        {
                            if (currentlyHighestContext.tagResponseCombinations[j].removeContexts != null)
                            {

                                List<Context> temp = new List<Context>();
                                temp.AddRange(DialoguePartner.GetComponent<DialogueTrigger>().context);

                                foreach (Context item in currentlyHighestContext.tagResponseCombinations[j].removeContexts)
                                {
                                    temp.Remove(item);
                                }

                                temp.Clear();

                            }
                        }

                        // change topic              
                        if (currentlyHighestContext.tagResponseCombinations[j].switchTopicTo != null)
                        {
                            gameManager.currentTopic = currentlyHighestContext.tagResponseCombinations[j].switchTopicTo;
                        }

                        // Invoke Event            
                        if (currentlyHighestContext.tagResponseCombinations[j].DialogueEvent != null)
                        {
                            currentlyHighestContext.tagResponseCombinations[j].DialogueEvent.Invoke();
                        }

                        return currentlyHighestContext.tagResponseCombinations[j].responses;
                    }

                }

                //Problematic
                contextsToSearch.Remove(currentlyHighestContext);
            }

        }
        contextsToSearch.Clear();
        return null;
    }

    // return answer if no Input Tag specific answer is found
    public string[] NoFittingAnswer()
    {
        gameManager.currentTopic = null;
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
                }
            }
            if (currentlyHighestContext.noFittingAnswer != null)
            {
                return currentlyHighestContext.noFittingAnswer;
            }
            else
            { contextsToSearch.Remove(currentlyHighestContext);}
        }
        return null;
    }

    // return a answer based on current topic and Input Tags
    public string[] AnswerCurrentTopic(){
        Context currentTopic = gameManager.currentTopic;

        bool answerFound = false;

        for (int i = 0; i < currentTopic.tagResponseCombinations.Length; i++)
        {
            for (int k = 0; k < currentTopic.tagResponseCombinations[i].tagList.Length; k++)
            {
                if (UnorderedEqual(currentTopic.tagResponseCombinations[i].tagList[k].inputTags, listOfTagsInInput))
                {
                    currentTopic.tagResponseCombinations[i].askedBefore = true;

                    //add context
                    if (currentTopic.tagResponseCombinations[i].addContexts != null)
                    {
                        for (int f = 0; f < currentTopic.tagResponseCombinations[i].addContexts.Count; f++)
                        {
/*CHECK IF WORKS*/          if (DialoguePartner.GetComponent<DialogueTrigger>().context.IndexOf(currentTopic.tagResponseCombinations[i].addContexts[f]) < 0)
                            {
                                DialoguePartner.GetComponent<DialogueTrigger>().context.Add(currentTopic.tagResponseCombinations[i].addContexts[f]);
                                DialoguePartner.GetComponent<EnterDialogue>().UpdateContexts();
                            }
                        }
                        
                    }

                    //remove contexts
                    if (currentTopic.tagResponseCombinations[i].removeContexts != null)
                    {
                        List<Context> temp = new List<Context>();
                        temp.AddRange(DialoguePartner.GetComponent<DialogueTrigger>().context);

                        foreach (Context item in currentTopic.tagResponseCombinations[i].removeContexts)
                        {
                            temp.Remove(item);
                        }

                        temp.Clear();
                    }

                    // change topic              
                    if (currentTopic.tagResponseCombinations[i].switchTopicTo != null)
                    {
                        gameManager.currentTopic = currentTopic.tagResponseCombinations[i].switchTopicTo;
                    }

                    // Invoke Event            
                    if (currentTopic.tagResponseCombinations[i].DialogueEvent != null)
                    {
                        currentTopic.tagResponseCombinations[i].DialogueEvent.Invoke();
                    }

                    return currentTopic.tagResponseCombinations[i].responses;
                }

            }
        }
            
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

        AnswerCurrentTopic();

        if (gameManager.currentTopic != null)
        {
            if (AnswerCurrentTopic() != null)
            {
                answerBuffer = AnswerCurrentTopic();
            }
            else if (gameManager.currentTopic.needsAnswer)
            {
                answerBuffer = gameManager.currentTopic.noFittingAnswer;
            }
        }
        else {answerBuffer = FindAnswerOfHighestPriorityContext();}

        if (answerBuffer != null)
        {
        }
        else if (gameManager.currentTopic.noFittingAnswer != null)
        {
            answerBuffer = gameManager.currentTopic.noFittingAnswer;
        }
        else answerBuffer = NoFittingAnswer();



        if (answerBuffer != null)
        {
            output = answerBuffer[Random.Range(0, answerBuffer.Length)];
        }
        else if (gameManager.currentTopic != null)
        {   
            Context currentTopic = gameManager.currentTopic;

            output = currentTopic.noFittingAnswer[Random.Range(0, currentTopic.noFittingAnswer.Length)];
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