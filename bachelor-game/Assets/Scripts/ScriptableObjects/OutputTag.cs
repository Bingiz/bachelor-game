using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/OutputTag")]
public class OutputTag : ScriptableObject
{
    public string outputTagName;

    public List<string> outputPossibilities = new List<string>();
}
