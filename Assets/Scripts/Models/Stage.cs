using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stage
{
    public string name;
    public int stageNo;
    public int manCount;
    public int objectCount;
    public string prefabFileName;
    
    public string ToJson()
    {
        string json = JsonUtility.ToJson(this);
        return json;
    }

    public static Stage FromJson(string json)
    {
        Stage obj = JsonUtility.FromJson<Stage>(json);
        return obj;
    }

    public override string ToString()
    {
        return $"{name},{manCount},{objectCount},{prefabFileName}";
    }
}
