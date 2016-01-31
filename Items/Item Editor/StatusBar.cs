using UnityEngine;
using System.Collections;

public partial class ItemEditor
{
    void StatusBar()
    {
        GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
        GUILayout.Label("status bar");
        GUILayout.EndHorizontal();
    }
}
