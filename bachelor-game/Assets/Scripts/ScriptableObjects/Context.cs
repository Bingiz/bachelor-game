using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

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

    private GameObject thisGameObject;

    public Context(string[] greet, List<CM> cm, List<CM> scm)
    {
        greetings = greet;
        listOfConversationalMoves = cm;
        specialCases = scm;
    }

    [System.Serializable]
    public class Topic
    {
        [HideInInspector]
        public string name;
        public InputTag topic;
        public string[] answers;
        [HideInInspector]
        public int answercount;

        [Header("Event")]
        public UnityEvent DialogueEvent;

        /*
        public void AddContext(Context C,GameObject D)
        {
            D.GetComponent<DialogueTrigger>().context.Add(C);
        }

        public void RemoveContext(Context C, GameObject D)
        {
            D.GetComponent<DialogueTrigger>().context.Remove(C);
        }
        */

        public Topic(string n, InputTag it, string[] an)
        {
            name = n;
            topic = it;
            answers = an;
            answercount = 0;
        }
    }

    [System.Serializable]
    public class CM
    {
        [HideInInspector]
        public string name;
        public ConversationalMove conversationalMoveObject;
        public List<Topic> topics;

        public CM(string n, ConversationalMove cm, List<Topic> to)
        {
            name = n;
            conversationalMoveObject = cm;
            topics = to;
        }

        public void Init(string n, ConversationalMove cm, List<Topic> to)
        {
            name = n;
            conversationalMoveObject = cm;
            topics = to;
        }
    }

    public string[] greetings;

    [Header("Event")]
    public UnityEvent GreetEvent;

    public List<CM> listOfConversationalMoves;

    public List<CM> specialCases;


    public Context()
    {
        priority = -1;
        //askedBefore = false;
    }
}
