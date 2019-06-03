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



    [System.Serializable]
    public class TagResponseCombinations
    {
        public string name;
        public List<InputTag> inputTags = new List<InputTag>();
        public string[] responses;
        public bool askedBefore;
    }

    public TagResponseCombinations[] tagResponseCombinations;


    public Context()
    {
        priority = -1;
       //askedBefore = false;
    }
}