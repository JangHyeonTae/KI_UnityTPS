using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> items = new();
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void GetItem(Item item)
    {
        items.Add(item);
    }

    public void UseItem(int index)
    {
        items[index].UseItem(controller);
        items.Remove(items[index]);
        Debug.Log($"{items[index]}");
    }

}
