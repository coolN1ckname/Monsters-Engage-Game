using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemsScriptableObject passiveItemData;

    protected virtual void ApplyModifier()
    {

    }

    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }
}
