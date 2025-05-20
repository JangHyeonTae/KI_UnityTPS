using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string Name;
    [TextArea] public string Description; //scriptableObject���� ���ٸ� �����ѵ� ������ �����ϵ��� TextArea���
    public Sprite Icon;
    public GameObject Prefab;

    public abstract void Use(PlayerCon controller);
}
