using UnityEngine;
using System.Collections;

public class Quality :IQuality {
    [SerializeField]
    string _name;
    [SerializeField]
    Sprite _icon;

    public Quality(){
        _name = string.Empty;
        _icon = new Sprite();
    }

    public string Name
    {
        get {return _name;}
        set { _name = value; }
    }

    public Sprite Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }
}
