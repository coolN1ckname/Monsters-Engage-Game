using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeefPassiveItem : PassiveItems
{
    protected override void ApplyModifier()
    {
        player.CurrentHealth *= 1+passiveItemData.Multipler/100f;
    }
}

