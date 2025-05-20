using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjected : MonoBehaviour
{
    [field: SerializeField] public Item Data {  get; set; }

    private GameObject _childObject;

    private void Awake()
    {
        _childObject = GetComponent<GameObject>();
    }

    private void OnEnable()
    {
        _childObject = Instantiate(Data.dataPrefab, transform);
    }

    private void OnDisable()
    {
        Destroy(_childObject);
    }
}
