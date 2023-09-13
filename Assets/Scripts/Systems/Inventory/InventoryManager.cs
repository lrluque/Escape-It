using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Inventory inventory = GetComponent<Inventory>();
        inventory.UpdateInventory();
    }

    // Update is called once per frame
    void Update()
    {
        //Scroll wheel to change inventory
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Inventory inventory = GetComponent<Inventory>();
            inventory.PreviousItem();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Inventory inventory = GetComponent<Inventory>();
            inventory.NextItem();
        }
    }
}
