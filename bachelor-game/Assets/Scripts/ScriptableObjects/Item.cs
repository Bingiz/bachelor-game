using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

[CreateAssetMenu(menuName = "Object/Item")]
public class Item : ScriptableObject
{
    public string name;

    [TextArea]
    public string description;

    public Sprite icon;

    public bool collectible;
}
