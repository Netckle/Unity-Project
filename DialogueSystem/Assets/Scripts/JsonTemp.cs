using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonClass
{
    public int dialogue_key;    
    public string dialogue_name;
    public string dialogue_sentence;
    public string event_category;
    public string event_key;
    public int next_key;
}

public class JsonTemp : MonoBehaviour
{
    JsonClass tempOBJ = new JsonClass();
    tempOBJ.dialogue_key = 1;
}
