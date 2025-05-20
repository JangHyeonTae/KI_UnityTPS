using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HP Potion", menuName = "Scriptable Objects/Hp Potion", order = 1)]
public class HpPotion : ItemData
{
    public int Value;

    public override void Use(PlayerCon controller)
    {
        controller.Recover(Value);
    }

}
