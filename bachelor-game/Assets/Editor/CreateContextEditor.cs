using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateContexts))]
public class CreateContextEditor : Editor
{
    public override void OnInspectorGUI()
	{
        DrawDefaultInspector();
        CreateContexts createContextsScript = (CreateContexts)target;
        if(GUILayout.Button("Create Context"))
		{
            createContextsScript.CreateContext();
		}

	}
}
