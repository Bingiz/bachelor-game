using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
        [Header("Context add")]
        public GameObject addContextToCharacter;
        public List<Context> addContexts;

        [Header("Context remove")]
        public GameObject removeContextFromCharacter;
        public List<Context> removeContexts;


        [Header("Topic switch")]
        public Context switchTopicTo;

        [Header("Input Tags required for response")]
        public List<InputTag> inputTags = new List<InputTag>();

        [Header("Response Output")]
        public string[] responses;

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