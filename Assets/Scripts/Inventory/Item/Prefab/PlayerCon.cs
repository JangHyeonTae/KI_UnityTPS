using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCon : MonoBehaviour
{
    [SerializeField] private int _hp;
    [SerializeField] private LayerMask itemLayer;
    private Inventory _inventory;

    private void Awake() => Init();
    private void Init()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _inventory.GetItem(other.GetComponent<ItemObject>().Data);
        other.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _inventory.UseItem(0);
        }
    }

    public void Recover(int amount)
    {
        _hp += amount;
    }


}
