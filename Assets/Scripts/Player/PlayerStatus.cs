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

    public ObservableProperty<bool> IsAiming { get; private set; } = new();
    //public bool isAiming { get { return  isAiming; } set { isAiming = value; OnAiming?.Invoke(isAiming); } }
    //public event Action<bool> OnAiming;
    public ObservableProperty<bool> IsMoving { get; private set; } = new();
    public ObservableProperty<bool> IsAttacking { get; private set; } = new();
}
