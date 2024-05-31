using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<PassiveItems> passiveItemSlots = new List<PassiveItems>(25);
    public List<Image> passiveItemsUISlots = new List<Image>(25);



    public void AddPassiveItem(int slotIndex, PassiveItems passiveItems)
    {
        passiveItemSlots[slotIndex] = passiveItems;
        passiveItemsUISlots[slotIndex].sprite = passiveItems.passiveItemData.Icon;
        passiveItemsUISlots[slotIndex].enabled = true;
    }
}
