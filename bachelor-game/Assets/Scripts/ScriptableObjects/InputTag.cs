using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/InputTag")]
public class InputTag : ScriptableObject
{
    

    public string name;

    public int priority;

    public List<string> associatedStrings = new List<string>();

    [System.Serializable]
    public class AssociatedTagCombinations
    {
        public InputTag[] InputTags;
    }

    public AssociatedTagCombinations[] associatedTagCombinations;


    public InputTag(string n)
    {
        name = n;
    }
}
