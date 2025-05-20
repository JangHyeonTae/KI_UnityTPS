using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    protected ObservableProperty<int> CurrentHp = new();
    protected ObservableProperty<bool> IsMoving = new();
    protected ObservableProperty<bool> IsAttacking = new();
    protected ObservableProperty<bool> IsDead = new();
}
