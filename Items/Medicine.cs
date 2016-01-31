using UnityEngine;
using System.Collections;

[System.Serializable]
public class Medicine : Item, IMedicine {
    public override void ItemDetailsInEditor()
    {
        base.ItemDetailsInEditor();
    }
}
