using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject[] inventory = new GameObject[4]; //0 is empty hand
    
    private int _currentIndex = 0;

    public void NextItem()
    {
        //cycle through inventory if there is a blank space we skip it
        int newIndex = _currentIndex + 1;
        if (newIndex > inventory.Length - 1 || inventory[newIndex] == null)
        {
            newIndex = 0;
        }
        _currentIndex = newIndex;
        UpdateInventory();
    }

    public void PreviousItem()
    {
        int newIndex = _currentIndex - 1;
        if (newIndex < 0)
        {
            newIndex = inventory.Length - 1;
        }
        if (inventory[newIndex] == null)
        {
            newIndex = 0;
        }
        _currentIndex = newIndex;
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        //cycle through inventory
        int i = 0;
        foreach (GameObject item in inventory)
        {
            if (i == _currentIndex)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
            i++;
        }
    }

    public void AddItem(GameObject item)
    {
        //find the first open slot in inventory
        int i = 0;
        foreach (GameObject slot in inventory)
        {
            if (slot == null)
            {
                inventory[i] = item;
                break;
            }
            i++;
        }
    }

    public GameObject GetCurrentItem()
    {
        return inventory[_currentIndex];
    }

    public void RemoveCurrentItem()
    {
        inventory[_currentIndex] = null;
        RearangeInventorySlots();
    }

    public void RearangeInventorySlots()
    {
        //rearrange inventory slots
        int i = 0;
        foreach (GameObject item in inventory)
        {
            if (item == null)
            {
                for (int j = i + 1; j < inventory.Length; j++)
                {
                    if (inventory[j] != null)
                    {
                        inventory[i] = inventory[j];
                        inventory[j] = null;
                        break;
                    }
                }
            }
            i++;
        }
    }
    
}
