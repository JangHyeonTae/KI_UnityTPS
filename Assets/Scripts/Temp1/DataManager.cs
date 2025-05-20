using CustomUtility.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [field : SerializeField] public CsvTable MonsterCSV { get; private set; }
    [field: SerializeField] public CsvDictionary MonsterDic { get; private set; }

    private void Awake() => Init();

    private void Init()
    {
        CsvReader.Read(MonsterCSV);
        CsvReader.Read(MonsterDic);
    }

}
public enum MonsterData
{
    Name = 1,
    Atk,
    Dfe,
    Spd,
    Dsc
}