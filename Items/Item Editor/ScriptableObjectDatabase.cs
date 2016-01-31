using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

public class ScriptableObjectDatabase<T> : ScriptableObject where T : class
{
    [SerializeField]
    List<T> database = new List<T>();

    public void Add(T item)
    {
        database.Add(item);
        EditorUtility.SetDirty(this);
    }

    public void Insert(int index, T item)
    {
        database.Insert(index, item);
        EditorUtility.SetDirty(this);
    }

    public void Remove(T item)
    {
        database.Remove(item);
        EditorUtility.SetDirty(this);
    }

    public void RemoveAt(int index)
    {
        database.RemoveAt(index);
        EditorUtility.SetDirty(this);
    }

    public int Count
    {
        get { return database.Count; }
    }

    public T Get(int index)
    {
        return database.ElementAt(index);
    }

    public void Replace(int index, T item)
    {
        database[index] = item;
        EditorUtility.SetDirty(this);
    }

    public static U GetDatabase<U>(string dbPath, string dbName) where U : ScriptableObject
    {
        string dbfullPath = @"Assets/" + dbPath + "/" + dbName;

        U db = AssetDatabase.LoadAssetAtPath(dbfullPath, typeof(U)) as U;


        if (db == null)
        {
            if (!AssetDatabase.IsValidFolder(Application.dataPath + dbPath))
            {
                AssetDatabase.CreateFolder("Assets", dbPath);
            }

            //create the database and the assetdatabase
            db = ScriptableObject.CreateInstance<U>();
            AssetDatabase.CreateAsset(db, dbfullPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        return db;
    }
}
