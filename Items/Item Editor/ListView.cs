using UnityEngine;
using System.Collections;

public partial class ItemEditor
{
    Vector2 scrollPos = Vector2.zero;
    int listviewWidth = 200;

    

    void ListView()
    {
        
        scrollPos = GUILayout.BeginScrollView(scrollPos, "Box", GUILayout.ExpandHeight(true), GUILayout.Width(listviewWidth));
        GUILayout.Label("List View");
        //admin.LoadItems();
        //admin.testMessage();
        if (GUILayout.Button("load all"))
        {
            admin.testMessage();
        }
        GUILayout.EndScrollView();
    }

    
}
