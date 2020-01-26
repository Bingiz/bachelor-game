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
    public class Topic
    {
        [HideInInspector]
        public string name;
        public InputTag topic;
        public string[] answers;

        [Header("Event")]
        public UnityEngine.Events.UnityEvent DialogueEvent;

        public Topic(string n, InputTag it)
        {
            name = n;
            topic = it;
        }
    }

    [System.Serializable]
    public class CM
    {
        [HideInInspector]
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

    public string[] greetings;

    public CM[] listOfConversationalMoves;

    public CM[] specialCases;

    [HideInInspector]
    Topic[] listOfTopics;

    public Context()
    {
        priority = -1;
        //askedBefore = false;
    }

    private void Awake()
    {
        Object[] count = Resources.LoadAll("InputTags/Topics");
        int countOfTopics = count.Length;
        listOfTopics = new Topic[countOfTopics];

        count = Resources.LoadAll("Conversational Moves/Standard Moves");
        int countOfConversationalMoves = count.Length;
        listOfConversationalMoves = new CM[countOfConversationalMoves];
        
            int i = 0;
            foreach (InputTag top in Resources.LoadAll("InputTags/Topics", typeof(InputTag)))
            {
                listOfTopics[i] = new Topic(top.name, top);
                i++;
            }

            int j = 0;
            foreach (ConversationalMove cmove in Resources.LoadAll("Conversational Moves/Standard Moves", typeof(ConversationalMove)))
            {
                listOfConversationalMoves[j] = new CM(cmove.name, cmove, listOfTopics);
                j++;
            }
    }
}

/*
if (cmove.name.Contains("SCM_"))
            {
                Topic[] singleTopic = new Topic[1];
singleTopic[0] = new Topic("DEFAULT", null);

listOfConversationalMoves[j] = new CM(cmove.name, cmove, singleTopic);
            }






        [System.Serializable]
    public class TagResponseCombinations
    {
        [HideInInspector]
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

        //[HideInInspector]
        //public bool askedBefore;
    }
*/
