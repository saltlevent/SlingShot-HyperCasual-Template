using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stages
{
    public string name;
    [SerializeField]
    public Stage[] stages;

    public string ToJson()
    {
        string json = JsonUtility.ToJson(this);
        return json;
    }

    public static Stages FromJson(string json)
    {
        Stages obj = JsonUtility.FromJson<Stages>(json);
        return obj;
    }

    public override string ToString()
    {
        return $"{name},{stages.Length}";
    }
}
