using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using System;
using UnityEngine.Events;

public class PlayerStatus : MonoBehaviour
{
    //Model
    [field: SerializeField][field :Range(0, 10)]
    public float WalkSpeed { get; set; }

    [field: SerializeField][field: Range(0, 10)]
    public float RunSpeed {  get; set; }

    [field: SerializeField][field: Range(0, 10)]
    public float RotateSpeed {  get; set; }

    [field: SerializeField][field: Range(0,500)]
    public int MaxHp { get; set; }


    //Player Stat Event
    public ObservableProperty<int> CurrentHp { get; private set; } = new();


    //Player State Event
    public ObservableProperty<bool> IsAiming { get; private set; } = new();
    //public bool isAiming { get { return  isAiming; } set { isAiming = value; OnAiming?.Invoke(isAiming); } }
    //public event Action<bool> OnAiming;
    public ObservableProperty<bool> IsMoving { get; private set; } = new();
    public ObservableProperty<bool> IsAttacking { get; private set; } = new();
    

}

//판정 후 실제 적용돼야하는 FillAmount - A 데이터로 가지고있고
//현재 적용중인 FillAmount를 따로두고 - B
//A와 B가 다르다면 deltaTime만큼 적용
