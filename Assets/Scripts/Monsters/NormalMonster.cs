using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;

public class NormalMonster : Monster, IDamagable
{
    private bool IsActivateControl = true;
    private bool _canTracking = true;

    [SerializeField] private int MaxHp;

    private ObservableProperty<int> CurrentHp;
    private ObservableProperty<bool> IsMoving = new();
    private ObservableProperty<bool> IsAttacking = new();


    [SerializeField] LayerMask layerMask;
    [SerializeField] private float range;
    [SerializeField] private float attackRange;

    [Header("Config Navmesh")]
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _targetTransform;

    private Animator _animator;
    private void Awake() => Init();

    private void Init()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _navMeshAgent.isStopped = true;
    }

    private void OnEnable() => SubScribeEvents();
    private void OnDisable() => UnSubScribeEvents();


    private void Update() => HandleControl();

    private void HandleControl()
    {
        if (!IsActivateControl) return;

        HandleMove();
    }

    private void HandleMove()
    {
        if (_targetTransform == null) return;

        _navMeshAgent.isStopped = !TargetIn() || _navMeshAgent.remainingDistance<= attackRange;
        IsMoving.Value = TargetIn();

        if (TargetIn())
        {
            //정지중인지, 만약 true라면 false로 바꿈
            _navMeshAgent.SetDestination(_targetTransform.position);

            if (_navMeshAgent.remainingDistance <= attackRange)
            {
                //강제로 정지 상태로 전환
                IsAttacking.Value = true;   
            }
            else
            {
                IsAttacking.Value = false;
            }
        }
    }

    public bool TargetIn()
    {
        if (Physics.OverlapSphere(transform.position, range, layerMask).Length>0)
        {
            return true;
        }

        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void TakeDamage(int value)
    {
        // 데미지 판정 구현
        //체력 깎고
        //체력이 0 이하가 되면 죽도록 처리
    }

    private void SubScribeEvents()
    {
        IsMoving.Subscribe(SetMoveAnim);
        IsAttacking.Subscribe(SetAttackAnim);
    }
    private void UnSubScribeEvents()
    {
        IsMoving.UnSubscribe(SetMoveAnim);
        IsAttacking.UnSubscribe(SetAttackAnim);
    }

    private void SetMoveAnim(bool value) => _animator.SetBool("IsMoving", value);
    private void SetAttackAnim(bool value) => _animator.SetBool("Attack", value);
}
