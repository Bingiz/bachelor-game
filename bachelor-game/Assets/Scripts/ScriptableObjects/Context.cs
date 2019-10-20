using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TagList
{
    public List<InputTag> inputTags = new List<InputTag>();
}

[CreateAssetMenu(menuName = "AI/Context")]
public class Context : ScriptableObject
{
    public string name;
    [TextArea]
    [SerializeField]
    string description;
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



    [Header("No fitting Answer")]
    public string[] noFittingAnswer;

    [Header("Greetings")]
    public string[] greetings;

    public TagResponseCombinations[] tagResponseCombinations;


    public Context()
    {
        priority = -1;
       //askedBefore = false;
    }
}