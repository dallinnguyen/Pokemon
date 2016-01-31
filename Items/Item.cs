using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class Item : IItem {
    [SerializeField]
    int _id;
    [SerializeField]
    string _name;
    [SerializeField]
    string _description;
    [SerializeField]
    ushort _image;
    [SerializeField]
    int _value;
    [SerializeField]
    Texture2D _icon;
    [SerializeField]
    float _weight;
    [SerializeField]
    Quality _quality;
    [SerializeField]
    bool _tradeable;

    public string Name
    {
        get { return _name;}
        set { _name = value; }
    }

    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    public int Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public Texture2D Icon
    {
        get { return _icon; }
        set { _icon = value; }
    }

    public float Weight
    {
        get { return _weight; }
        set { _weight = value; }
    }

    public Quality Quality
    {
        get { return _quality; }
        set { _quality = value; }
    }

    public int ID
    {
        get { return _id; }
        set { _id = value; }
    }

    public ushort Image
    {
        get { return _image; }
        set { _image = value; }
    }

    public bool Tradeable
    {
        get { return _tradeable; }
        set { _tradeable = value; }
    }

    public virtual void ItemDetailsInEditor()
    {
        GUILayout.BeginVertical();
        _name = EditorGUILayout.TextField("Name: ", _name);
        _value = int.Parse(EditorGUILayout.TextField("Value: ", _value.ToString()));
        _weight = int.Parse(EditorGUILayout.TextField("Weight: ", _weight.ToString()));

        DisplayIcon();
        DisplayQuality();
        GUILayout.EndVertical();
    }

    private void DisplayIcon()
    {
        GUILayout.Label("Icon");
    }

    public void DisplayQuality()
    {
        GUILayout.Label("Quality");
    }
}
