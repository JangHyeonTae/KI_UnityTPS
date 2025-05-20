using CustomUtility.IO;
using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveText : SaveData
{
    [SerializeField] List<Item> items = new List<Item>();
    [field : SerializeField] public int HP { get; set; }
    [field : SerializeField] public Vector3 playerPos { get; set; }

    public SaveText() { }
    
    public SaveText(int _hp, Vector3 _pos)//,List<Item> _items)
    {
        HP = _hp;
        playerPos = _pos;
        //items = _items;
    }
}

public class PlayerSaveTest : MonoBehaviour
{
    [SerializeField] private PlayerStatus status;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Inventory inventory;


    public SaveText _jsonSave;
    public SaveText _jsonLoad;

    public void SaveJson()
    {
        _jsonSave = new(status.CurrentHp.Value, player.GlobalPos());
        DataSaveController.Save(_jsonSave, SaveType.JSON);
    }

    public void LoadJson()
    {
        _jsonLoad = new(0,Vector3.zero);
        DataSaveController.Load(ref _jsonLoad, SaveType.JSON);
        Debug.Log(_jsonLoad.HP);
        Debug.Log(_jsonLoad.playerPos);
        status.CurrentHp.Value = _jsonLoad.HP;
        player.transform.position = _jsonLoad.playerPos;
    }
}