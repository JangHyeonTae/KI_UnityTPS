using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditorInternal.Profiling.Memory.Experimental;

//참조 생성용 임시 네임스페이스 참조
//작업물 병합 시 삭제 예정

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamagable
    {
        public bool IsControlActivate { get; set; } = true;

        private PlayerStatus _status;
        private PlayerMovement _movement;
        private Animator _animator;
        private Image _aimImage;

        private InputAction _aimInputAction;
        private InputAction _shootInputAction;

        [SerializeField] private CinemachineVirtualCamera _aimCamera;
        [SerializeField] private Gun _gun;
        [SerializeField] private Animator _aimAnimator;
        [SerializeField] private HpGuageUI _hpUI;

        //일반카메라
        //[SerializeField] private GameObject _aimCamera;
        //private GameObject _mainCamera;

        [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;
        [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;

        [Header("Save Settings")]
        [SerializeField] private PlayerSaveTest saveTest;

        [Header("Inventroy Settings")]
        [SerializeField] private LayerMask layerItem;
        [SerializeField] private PlayerInventory _inventory;

        [SerializeField] private Image inventroyImage;

        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update()
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                saveTest.SaveJson();
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                saveTest.LoadJson();
            }
            if (Input.GetKey(KeyCode.I))
            {
                ActiveInventory();
            }
            _status.IsAttacking.Value = false;
            HandlePlayerControl();
        }
        private void OnDisable() => UnSubscribeEvents();
        
        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            _animator = GetComponent<Animator>();
            _aimImage = _aimAnimator.GetComponent<Image>();
            _aimInputAction = GetComponent<PlayerInput>().actions["Aim"];
            //_mainCamera = Camera.main.gameObject;

            _hpUI.SetImageFillAmount(1);
            _status.CurrentHp.Value = _status.MaxHp;
        }

        private void HandlePlayerControl()
        {
            if (!IsControlActivate) return;
            HandleMovement();
            //HandleAiming();
            //HandleShooting();
        }

        public void ActiveInventory()
        {
            if (inventroyImage.gameObject.activeSelf) //이부분 인벤토리 이벤트로 바꿔주기
            {
                inventroyImage.gameObject.SetActive(false);
                IsControlActivate = true;
                //StartCoroutine(InvenOpen());
            }
            else
            {
                inventroyImage.gameObject.SetActive(true);
                IsControlActivate = false;
            }
            
        }

        private IEnumerator InvenOpen()
        {
            yield return new WaitForSeconds(0.5f);
            //인벤토리 이벤트
            //인벤토리 애니메이션
        }

        //private void HandleShooting()
        public void OnShoot()
        {
            // _shootInputAction.WasPressedThisFrame(); => 이번 프레임에 눌렀는가 (GetKeyDown)
            // _shootInputAction.WasReleasedThisFrame(); => 이번 프레임에 떼어졌는가 (GetKeyUp)
            // _shootInputAction.IsPressed() => 지금 눌러있는가?(GetKey)
            //if (_status.IsAiming.Value && Input.GetKey(_shootKey))
            if(_status.IsAiming.Value)
            {
                _status.IsAttacking.Value = _gun.Shoot();
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
                //Vector3 input = _movement.GetInputDirection();
                //_animator.SetFloat("X", input.x);
                //_animator.SetFloat("Z", input.z);

                _animator.SetFloat("X", _movement.InputDirection.x);
                _animator.SetFloat("Z", _movement.InputDirection.y);
            }
        }

        private void HandleAiming(InputAction.CallbackContext ctx)
        {
            //_status.IsAiming.Value = Input.GetKey(_aimKey);
            _status.IsAiming.Value = ctx.started;


            // 눌린 상태로 유지하고 싶을 때
            // 1. Key Down 상황일 때 -> 키 입력이 시작된 시점인지 체ㅋ,
            // 2. Key Up 상황일 때 -> 키 입력이 시작된 시점인지 체크
            
            // ctx.started => 키 입력이 시작 됐는지 판변
            // ctx.performed => 키 입력이 진행중인지 판별
            // ctx.canceled => 키 입력이 취소 됐는지(떼어졌는지)판별
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

            //inputs ----------------
            _aimInputAction.Enable();
            _aimInputAction.started += HandleAiming;
            _aimInputAction.canceled += HandleAiming;
        }

        public void UnSubscribeEvents()
        {
            _status.CurrentHp.UnSubscribe(SetHpUIGuage);
            //_status.IsAiming.UnSubscribe(value => SetActivateAimCamera(value));
            _status.IsAiming.UnSubscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.UnSubscribe(SetAimAnimation);
           
            _status.IsAttacking.UnSubscribe(SetAttackAnimation);

            _status.IsMoving.UnSubscribe(SetMoveAnimation);
            
            //inputs ----------------
            _aimInputAction.Disable();
            _aimInputAction.started -= HandleAiming;
            _aimInputAction.canceled -= HandleAiming;
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

        public void OnTriggerEnter(Collider other)
        {
             _inventory.GetItem(other.GetComponent<ItemObjected>().Data);
             other.gameObject.SetActive(false);
        }
    }
}

