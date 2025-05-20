using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class NormalMonster : Monster, IDamagable
{
    private bool IsActivateControl = true;
    private bool _canTracking = true;

    [SerializeField] private HpGuageUI _hpUI;

    [SerializeField] private int MaxHp;

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
        CurrentHp.Value = MaxHp;
        _hpUI.SetImageFillAmount(1);


    }

    private void OnEnable() => SubScribeEvents();
    private void OnDisable() => UnSubScribeEvents();


    private void Update()
    {
        HandleControl();
    }


    private void HandleControl()
    {
        if (!IsActivateControl) return;

        HandleMove();
        //_navMeshAgent.isStopped = !IsMoving.Value;
    }

    private void HandleMove()
    {
        if (_targetTransform == null) return;

        if (TargetIn())
        {
            float distance = Vector3.Distance(transform.position, _targetTransform.position);
            if (distance <= attackRange)
            {
                _navMeshAgent.isStopped = true;
                IsAttacking.Value = true;
                IsMoving.Value = false;
            }
            else
            {
                _navMeshAgent.isStopped = false;
                IsAttacking.Value = false;
                _navMeshAgent.SetDestination(_targetTransform.position);
                IsMoving.Value = true;
            }
        }
        else
        {
            IsMoving.Value = false;
        }
    }


    public void OnAttack(int damage)
    {
        IDamagable target = MonsterAttack();
        float distance = Vector3.Distance(transform.position, _targetTransform.position);
        if (target == null) return;
        if (distance <= attackRange)
        {
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
        
    }

    public IDamagable MonsterAttack()
    {
        if (_targetTransform != null)
        {
            
            IsAttacking.Value = true;

            return _targetTransform.gameObject.GetComponent<IDamagable>();
            //return ReferenceRegistry.GetProvider(_targetTransform.gameObject).
            //    GetAs<PlayerController>();
        }
        return null;
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

    public void TakeDamage(int damage)
    {
        // ������ ���� ����
        //ü�� ���
        //ü���� 0 ���ϰ� �Ǹ� �׵��� ó��
        if (damage >= 1)
        {
            //ī��Ʈ = ������(1��)
            _animator.SetTrigger("TakeDamage");
        }

        //update�� ī��Ʈ -= Time.deltaTime
        //ī��Ʈ�� 0���� ���� �� canMove ����
        CurrentHp.Value = Mathf.Max(0,CurrentHp.Value - damage);

        Debug.Log($"{gameObject.name} : {CurrentHp.Value}");
        if (CurrentHp.Value <= 0)
        {
            EnemyDead();
        }
    }

    public void EnemyDead()
    {
        IsDead.Value = true;
        IsActivateControl = false;
        Destroy(gameObject, 2);
    }

    public Vector3 GlobalPos()
    {
        Vector3 globalPos = gameObject.transform.position;
        return globalPos;
    }

    private void SubScribeEvents()
    {
        IsMoving.Subscribe(SetMoveAnim);
        IsAttacking.Subscribe(SetAttackAnim);
        CurrentHp.Subscribe(SetHpAnim);
        IsDead.Subscribe(SetDeadAnim);
    }
    private void UnSubScribeEvents()
    {
        IsMoving.UnSubscribe(SetMoveAnim);
        IsAttacking.UnSubscribe(SetAttackAnim);
        CurrentHp.UnSubscribe(SetHpAnim);
        IsDead.UnSubscribe(SetDeadAnim);
    }

    private void SetMoveAnim(bool value) => _animator.SetBool("IsMoving", value);
    private void SetAttackAnim(bool value) => _animator.SetBool("Attack", value);
    private void SetDeadAnim(bool value) => _animator.SetBool("IsDead", value);
    
    private void SetHpAnim(int value)
    {
        float hp = value / (float)MaxHp;
        _hpUI.SetImageFillAmount(hp);
    }
}
