using UnityEngine;
using System.Collections;

public partial class ItemEditor
{
    Medicine medicine;
    bool showNewItemDetails = false;

    void itemDetails()
    {
        GUILayout.BeginVertical("Box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        //GUILayout.Label("Detail view");
        if (showNewItemDetails)
        {
            DisplayItem();
        }
        
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.Space(50);
        DisplayButtons();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    void DisplayItem()
    {
        if (medicine != null)
        {
            medicine.ItemDetailsInEditor();
        }

    }

    void DisplayButtons()
    {
        if (showNewItemDetails)
        {
            if (GUILayout.Button("Save"))
            {
                database.Add(medicine);
                
                showNewItemDetails = false;
                medicine = null;
            }
            if (GUILayout.Button("Cancel"))
            {
                showNewItemDetails = false;
                medicine = null;
            }
        }
        else
            if (GUILayout.Button("Create Item"))
            {
                medicine = new Medicine();
                showNewItemDetails = true;
            }
    }
    
}
