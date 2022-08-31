using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeNode : MonoBehaviour
{
    public Type type;
    
    [HideInInspector]
    public bool requirement = false;

    public enum Type
    {
        Plain,
        Town,
        Forest,
        Bridge,
        Water,
        Farm
    }

    private void Start()
    {
        if (type == Type.Water)
        {
            requirement = true;
        }
    }
}
