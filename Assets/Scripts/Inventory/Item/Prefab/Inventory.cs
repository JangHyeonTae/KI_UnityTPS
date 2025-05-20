using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> _slots = new();
    private PlayerCon _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerCon>();
    }

    public void GetItem(ItemData itemData)
    {
        _slots.Add(itemData);
    }

    public void UseItem(int index)
    {
        //_slots[index] = null; // 비어있는 기능구현
        _slots[index].Use(_controller);
        _slots.RemoveAt(index);
    }
}
