using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/ConversationalMove")]
public class ConversationalMove : ScriptableObject
{
    public int priority;
    public AssociatedTagCombinations[] associatedTagCombinations;
    public TopicAnswer[] topicAnswers;

    [System.Serializable]
    public class AssociatedTagCombinations
    {
        public InputTag[] InputTags;
    }

    [System.Serializable]
    public class TopicAnswer
    {
        public InputTag Topic;
        public string[] Answers;
    }
}
