using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/ConversationalMove")]
public class ConversationalMove : ScriptableObject
{
    [System.Serializable]
    public class AssociatedTagCombinations
    {
        public InputTag[] InputTags;
    }

    public AssociatedTagCombinations[] associatedTagCombinations;

    [System.Serializable]
    public class TopicAnswer
    {
        public InputTag Topic;
        public string[] Answers;
    }

    public TopicAnswer[] topicAnswers;
}
