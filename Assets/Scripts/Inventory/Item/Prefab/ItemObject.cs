using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [field: SerializeField] public ItemData Data { get; private set; }

    private GameObject _childObject;

    private void OnEnable()
    {
        _childObject = Instantiate(Data.Prefab, transform);
    }

    private void OnDisable()    
    {
        Destroy(_childObject);
    }
}
