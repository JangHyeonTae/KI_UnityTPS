using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//참조 생성용 임시 네임스페이스 참조
//작업물 병합 시 삭제 예정

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsControlActivate { get; set; } = true;

        private PlayerStatus _status;
        private PlayerMovement _movement;
        private Animator _animator;
        private Image _aimImage;

        [SerializeField] private CinemachineVirtualCamera _aimCamera;
        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _aimAnimator;
        [SerializeField] private HpGuageUI _hpUI;

        //일반카메라
        //[SerializeField] private GameObject _aimCamera;
        //private GameObject _mainCamera;

        [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();
        private void OnDisable() => UnSubscribeEvents();
        
        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
            _aimImage = _aimAnimator.GetComponent<Image>();
            //_mainCamera = Camera.main.gameObject;

            _hpUI.SetImageFillAmount(1);
            _status.CurrentHp.Value = _status.MaxHp;

        }

        private void HandlePlayerControl()
        {
            if (!IsControlActivate) return;

            HandleMovement();
            HandleAiming();
            HandleShooting();

            if (Input.GetKey(KeyCode.Alpha1))
            {
                TakeDamage(1);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                RecoveryHP(1);
            }
        }

        private void HandleShooting()
        {
            if (_status.IsAiming.Value && Input.GetKey(_shootKey))
            {
                _status.IsAttacking.Value = _gun.Shoot();
            }
            else
            {
                _status.IsAttacking.Value = false;
            }
        }

        private void HandleMovement()
        {
            // TODO: Movement 병합시 기능 추가예정
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            // 몸체의 회전기능 SetAvatarRotation
            Vector3 avatarDir;
            if (_status.IsAiming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir;

            _movement.SetAvatarRotation(avatarDir);

            // SetAnimationParameter
            if (_status.IsAiming.Value)
            {
                Vector3 input = _movement.GetInputDirection();
                _animator.SetFloat("X", input.x);
                _animator.SetFloat("Z", input.z);
            }
        }

        private void HandleAiming()
        {
            _status.IsAiming.Value = Input.GetKey(_aimKey);
        }

        public void TakeDamage(int value)
        {
            // 체력을 떨어뜨리되, 체력이 0이 되면 플레이어가 죽도록 처리함
            _status.CurrentHp.Value = Mathf.Max(0, _status.CurrentHp.Value - value);
            if (_status.CurrentHp.Value == 0)
            {
                Dead();
            }
        }

        public void RecoveryHP(int value)
        {
            // 체력을 높이되, MaxHp 초과를 막아야함
            //_status.CurrentHp.Value = Mathf.Min(_status.MaxHp, _status.CurrentHp.Value + value);
            int hp = _status.CurrentHp.Value + value;

            _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
        }

        public void Dead()
        {
            Debug.Log("플레이어 사망 처리");
        }

        public void SubscribeEvents()
        {
            _status.CurrentHp.Subscribe(SetHpUIGuage);
            //_status.IsAiming.Subscribe(value => SetActivateAimCamera(value));
            _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Subscribe(SetAimAnimation); //SetAimAnimation의 value는 어떻게 바뀐걸 알까?
            
            _status.IsAttacking.Subscribe(SetAttackAnimation);
            
            _status.IsMoving.Subscribe(SetMoveAnimation);
            //_status.OnAiming += _aimCamera.gameObject.SetActive; //- CinemachineVirtualCamera.gameObject.SetActive가 왜 될까?

        }

        public void UnSubscribeEvents()
        {
            _status.CurrentHp.UnSubscribe(SetHpUIGuage);
            //_status.IsAiming.UnSubscribe(value => SetActivateAimCamera(value));
            _status.IsAiming.UnSubscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.UnSubscribe(SetAimAnimation);
           
            _status.IsAttacking.UnSubscribe(SetAttackAnimation);

            _status.IsMoving.UnSubscribe(SetMoveAnimation);
        }

        private void SetActivateAimCamera(bool value)
        {
            //_aimCamera.SetActive(value);
            //_mainCamera.SetActive(!value);
        }

        private void SetAimAnimation(bool value)
        {
            if (!_aimImage.enabled)
            {
                _aimImage.enabled=true;
            }
            _animator.SetBool("IsAim", value);
            _aimAnimator.SetBool("IsAim", value);
        }
        private void SetMoveAnimation(bool value) => _animator.SetBool("IsMove", value);
        private void SetAttackAnimation(bool value) => _animator.SetBool("IsAttack", value);
        private void SetHpUIGuage(int currentHp)
        {
            //현재수치 / 최대수치
            float hp = currentHp / (float)_status.MaxHp;
            _hpUI.SetImageFillAmount(hp);
        }
    }
}

