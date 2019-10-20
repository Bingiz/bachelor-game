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

    public void AddInputTags()
    {
        //test if input String contains a question mark
        if(input.Contains("?"))
        {
            listOfTagsInInput.Add(listOfAllInputTags.Find(x => x.name.Contains("question")));
            Debug.Log(listOfAllInputTags.Find(x => x.name.Contains("question")).name + " added"); //add question tag to list
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
        Debug.Log("No answer found");
        return null;

        //gameManager.currentContexts[0].tagResponseCombinations[0].inputTags[0]
    }


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
                        DialoguePartner.GetComponent<DialogueTrigger>().context.AddRange(currentTopic.tagResponseCombinations[i].addContexts);
                        DialoguePartner.GetComponent<EnterDialogue>().UpdateContexts();
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
            if(currentTopic.needsAnswer)
            {
                return currentTopic.noFittingAnswer;
            }
            return null;
            
    }


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

    public void outputAnswer()
    {   
        string[] answerBuffer = null;
        
        if (gameManager.currentTopic != null)
        {
            answerBuffer = AnswerCurrentTopic();
        }
        if (answerBuffer != null)
        {
            
        }
        else {answerBuffer = FindAnswerOfHighestPriorityContext();}
            


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

    public void WriteNPCLine(string output)
    {
        DialoguePartner = gameManager.DialoguePartner;
        DialoguePartner.GetComponent<CharacterDialogueInfos>().responseHistory += "\n" + output + "\n\n";
    }

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


//old version (separates input in array and searches for corresponding Tags)
/*
public class SendPlayerInput : MonoBehaviour
{

    public GameObject PlayerInput;

    public GameObject AnswerText;

    public List<InputTag> listOfAllInputTags;

    [HideInInspector] public List<InputTag> listOfTagsInInput;

    private GameManager gameManager;

    [HideInInspector] public List<string> listOfInputWords = new List<string>();

    //backup
    private char[] wordSplitPattern = { ' ', ',', '.', ':', ';', '!','?', '\'' }; //Split
    //private string regexWordPattern = "[a-z,A-Z]+|[\\?,\\!,\\.\\,]";                                    //RegEx


    string output;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

        listOfAllInputTags = gameManager.listOfAllInputTags;

    }

    public void SendInputMessage()
    {
        listOfInputWords.Clear();

        string input = PlayerInput.GetComponent<Text>().text;

        input = input.Trim();

        input = input.ToLower();

        //listOfInputWords.AddRange(input.Split(wordSplitPattern));         //Split

        //listOfInputWords.AddRange(Regex.Split(input,regexWordSplitPattern));//RegEx

        listOfInputWords.AddRange(SplitString(input, wordSplitPattern, true, true));

        //DOES NOT WORK ATM should remove spaces
        listOfInputWords.Remove(" ");

        //currently disabled for test purposes
        //stemInputMessage(listOfInputWords);

        CompareInputWithInputTags();

        FindAnswerOfHighestPriorityContext();

        for (int i = 0; i < listOfInputWords.Count; i++)
        {
            Debug.Log(listOfInputWords[i]);
        }

        outputAnswer();

        listOfTagsInInput.Clear();

    }

    public void CompareInputWithInputTags()
    {
        for (int i = 0; i < listOfInputWords.Count; i++)
        {
            bool tagFoundForWord = false;

            for (int j = 0; j < listOfAllInputTags.Count; j++)
            {

                for (int k = 0; k < listOfAllInputTags[j].associatedStrings.Count && tagFoundForWord ==false; k++)
                {
                    //Debug.Log("Comparing " + listOfInputWords[i] + " to " + listOfAllInputTags[j].associatedStrings[k]);

                    if (listOfInputWords[i] == listOfAllInputTags[j].associatedStrings[k] )
                    {
                        listOfTagsInInput.Add(listOfAllInputTags[j]);
                        //Debug.Log(listOfAllInputTags[j] + " added");
                        tagFoundForWord = true;
                        break;
                    }
                }
            }
        }
    }

    public string[] FindAnswerOfHighestPriorityContext()
    {
        List<Context> contextsToSearch = new List<Context>();

        contextsToSearch.AddRange(gameManager.currentContexts);


        Context currentlyHighestContext = ScriptableObject.CreateInstance<Context>();

        bool tagsFound = false;
        //find highest priority context

        for (int h = 0; h < contextsToSearch.Count; h++)
        {
            //Debugging
            //Debug.Log(contextsToSearch[h].name);

            for (int i = 0; i < gameManager.currentContexts.Count; i++)
            {
                if (gameManager.currentContexts[i].priority > currentlyHighestContext.priority)
                {
                    currentlyHighestContext = gameManager.currentContexts[i];
                }
            }

            for (int i = 0; i < currentlyHighestContext.tagResponseCombinations.Length; i++)
            {

                //compare each Tag from tagResponseCombination[i] in current Context with each Tag from the Player Input
                if (UnorderedEqual(currentlyHighestContext.tagResponseCombinations[i].inputTags, listOfTagsInInput))
                {
                    //Debug.Log("TAGS MATCH");
                    string debugString = string.Join(",", currentlyHighestContext.tagResponseCombinations[i].responses);
                    Debug.Log(debugString);
                    return currentlyHighestContext.tagResponseCombinations[i].responses;
                }

            }

            //Problematic
            contextsToSearch.Remove(currentlyHighestContext);

        }
        contextsToSearch.Clear();
        Debug.Log("No fitting Answer found");
        return null;

        //gameManager.currentContexts[0].tagResponseCombinations[0].inputTags[0]
    }

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

    public void outputAnswer()
    {
        output = FindAnswerOfHighestPriorityContext()[Random.Range(0, FindAnswerOfHighestPriorityContext().Length)];
        //write the output
        AnswerText.GetComponent<Text>().text = output;

        // clear the output buffer and empty the list of recieved words
        output = "";
        listOfInputWords.Clear();
    }

    //https://answers.unity.com/questions/1187706/split-string-keep-split-chars-separators-c.html
    public string[] SplitString(string stringToSplit, char[] delimiters, bool includeDelimiter, bool trimEnds)
    {
        int index = -1;
        List<string> stringList = new List<string>();
        StringBuilder sBuilder = new StringBuilder();

        while (++index < stringToSplit.Length)
        {
            sBuilder.Append(stringToSplit[index]);

            foreach (char c in delimiters)
            {
                if (stringToSplit[index] == c)
                {
                    if (!includeDelimiter)
                    {
                        sBuilder.Remove(sBuilder.Length - 1, 1);
                    }
                    if (trimEnds)
                    {
                        stringList.Add(sBuilder.ToString().Trim());
                    }
                    else
                    {
                        stringList.Add(sBuilder.ToString());
                    }
                    sBuilder.Clear();
                    break;
                }
            }
        }
        return stringList.ToArray();
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
*/