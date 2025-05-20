using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

[CreateAssetMenu(fileName = "Item", menuName ="Item/Potion")]
public class ItemPotion : Item
{
    [SerializeField] private int value;

    public override void UseItem(PlayerController _controller)
    {
        _controller.RecoveryHP(value);
    }
}
