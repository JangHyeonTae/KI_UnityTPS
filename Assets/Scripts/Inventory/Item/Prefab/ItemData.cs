using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string Name;
    [TextArea] public string Description; //scriptableObject에서 한줄만 가능한데 여러줄 가능하도록 TextArea사용
    public Sprite Icon;
    public GameObject Prefab;

    public abstract void Use(PlayerCon controller);
}
