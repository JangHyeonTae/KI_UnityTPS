using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Unity.VisualScripting;

public abstract class Item : ScriptableObject
{
    public string name;
    public string description;
    public Sprite icon;
    public LayerMask layerPlayer;
    public GameObject dataPrefab;

    public abstract void UseItem(PlayerController controller);

    
}
