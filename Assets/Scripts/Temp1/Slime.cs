using CustomUtility.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public DataManager Data;
    public MonsterType type;

    public string Name;

    [SerializeField] private string _name;
    [SerializeField] private int _atk;
    [SerializeField] private int _dfe;
    [SerializeField] private int _spd;
    [SerializeField] private string _dsc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Init(type);
            Init(Name);
        }
    }

    private void Init(string name)
    {
        _name = Data.MonsterDic.GetData(name, "�̸�");
        _atk = int.Parse(Data.MonsterDic.GetData(name, "���ݷ�"));
        _dfe = int.Parse(Data.MonsterDic.GetData(name, "����"));
        _spd = int.Parse(Data.MonsterDic.GetData(name, "�̵��ӵ�"));
        _dsc = Data.MonsterDic.GetData(name, "����");
        //_name = Data.MonsterCSV.GetData((int)type, (int)MonsterData.Name);
        //_atk = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Atk));
        //_dfe = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Dfe));
        //_spd = int.Parse(Data.MonsterCSV.GetData((int)type, (int)MonsterData.Spd));
        //_dsc = Data.MonsterCSV.GetData((int)type, (int)MonsterData.Name);
    }

}

public enum MonsterType
{
    Slime = 1,
    Skel
}