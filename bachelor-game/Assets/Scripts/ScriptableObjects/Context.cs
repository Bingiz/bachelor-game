using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

[System.Serializable]
public class TagList
{
    public List<InputTag> inputTags = new List<InputTag>();
}

[CreateAssetMenu(menuName = "AI/Context")]
public class Context : ScriptableObject
{
    public int priority;
    private string[] characters;
    public bool needsAnswer;

    [System.Serializable]
    public class TagResponseCombinations
    {
        public string name;

        [Header("Input Tags required for response")]
        public TagList[] tagList;

        [Header("Response Output")]
        public string[] responses;

        [Header("Context add")]
        //public GameObject addContextToCharacter;
        public List<Context> addContexts;

        [Header("Context remove")]
        //public GameObject removeContextFromCharacter;
        public List<Context> removeContexts;

        [Header("Topic switch")]
        public Context switchTopicTo;

        [Header("Event")]
        public UnityEngine.Events.UnityEvent DialogueEvent;

        [HideInInspector]
        public bool askedBefore;
    }

    [System.Serializable]
    public class Topic
    {
        public string name;
        public InputTag topic;
        public string[] answers;

        public Topic(string n, InputTag it)
        {
            name = n;
            topic = it;
        }
    }

    [System.Serializable]
    public class CM
    {
        public string name;
        public ConversationalMove conversationalMoveObject;
        public Topic[] topics;

        public CM(string n, ConversationalMove cm, Topic[] to)
        {
            name = n;
            conversationalMoveObject = cm;
            topics = to;
        }

    }

    public string[] noFittingAnswer;

    public string[] greetings;

    public CM[] listOfConversationalMoves;

    [HideInInspector]
    Topic[] listOfTopics;

    public Context()
    {
        priority = -1;
        //askedBefore = false;
    }

    private void Awake()
    {
        Object[] count = Resources.LoadAll("Topics");
        int countOfTopics = count.Length;
        listOfTopics = new Topic[countOfTopics];

        count = Resources.LoadAll("Conversational Moves");
        int countOfConversationalMoves = count.Length;
        listOfConversationalMoves = new CM[countOfConversationalMoves];
        
            int i = 0;
            foreach (InputTag top in Resources.LoadAll("Topics", typeof(InputTag)))
            {
                listOfTopics[i] = new Topic(top.name, top);
                i++;
            }

            int j = 0;
            foreach (ConversationalMove cmove in Resources.LoadAll("Conversational Moves", typeof(ConversationalMove)))
            {
                listOfConversationalMoves[j] = new CM(cmove.name, cmove, listOfTopics);
                j++;
            }
    }
}