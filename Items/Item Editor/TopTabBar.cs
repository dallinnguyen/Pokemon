using UnityEngine;
using System.Collections;

public partial class ItemEditor
{

    void TopTabBar()
    {
        GUILayout.BeginHorizontal("Box", GUILayout.ExpandWidth(true));
        MedicineTab();
        BattleItemTab();
        MachineTab();
        GUILayout.EndHorizontal();
    }
    void BattleItemTab()
    {
        GUILayout.Button("Battle");
    }

    void MedicineTab()
    {
        GUILayout.Button("Medicine");
    }

    void MachineTab()
    {
        GUILayout.Button("Machine");
    }
}
