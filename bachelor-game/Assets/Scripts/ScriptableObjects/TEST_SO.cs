using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/TEST_SO")]
public class TEST_SO : ScriptableObject
{
    public string name;

    private void Awake()
    {
        name = "bobo";
    }
}

