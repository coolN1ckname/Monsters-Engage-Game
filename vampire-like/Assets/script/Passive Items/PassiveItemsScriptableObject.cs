using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemsScriptableObject", menuName = "ScriptableObjects/Passive Items")]
public class PassiveItemsScriptableObject : ScriptableObject
{
    [SerializeField]
    float multipler;
    public float Multipler { get => multipler; private set => multipler= value;}

    [SerializeField]
    Sprite icon;

    public Sprite Icon { get => icon; private set => icon = value; }
}
