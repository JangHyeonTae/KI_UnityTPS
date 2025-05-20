using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditorInternal.Profiling.Memory.Experimental;

//���� ������ �ӽ� ���ӽ����̽� ����
//�۾��� ���� �� ���� ����

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

        //�Ϲ�ī�޶�
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
            if (inventroyImage.gameObject.activeSelf) //�̺κ� �κ��丮 �̺�Ʈ�� �ٲ��ֱ�
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
            //�κ��丮 �̺�Ʈ
            //�κ��丮 �ִϸ��̼�
        }

        //private void HandleShooting()
        public void OnShoot()
        {
            // _shootInputAction.WasPressedThisFrame(); => �̹� �����ӿ� �����°� (GetKeyDown)
            // _shootInputAction.WasReleasedThisFrame(); => �̹� �����ӿ� �������°� (GetKeyUp)
            // _shootInputAction.IsPressed() => ���� �����ִ°�?(GetKey)
            //if (_status.IsAiming.Value && Input.GetKey(_shootKey))
            if(_status.IsAiming.Value)
            {
                _status.IsAttacking.Value = _gun.Shoot();
            }
        }

        private void HandleMovement()
        {
            // TODO: Movement ���ս� ��� �߰�����
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir = _movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);

            // ��ü�� ȸ����� SetAvatarRotation
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


            // ���� ���·� �����ϰ� ���� ��
            // 1. Key Down ��Ȳ�� �� -> Ű �Է��� ���۵� �������� ü��,
            // 2. Key Up ��Ȳ�� �� -> Ű �Է��� ���۵� �������� üũ
            
            // ctx.started => Ű �Է��� ���� �ƴ��� �Ǻ�
            // ctx.performed => Ű �Է��� ���������� �Ǻ�
            // ctx.canceled => Ű �Է��� ��� �ƴ���(����������)�Ǻ�
        }

        public void TakeDamage(int value)
        {
            // ü���� ����߸���, ü���� 0�� �Ǹ� �÷��̾ �׵��� ó����
            _status.CurrentHp.Value = Mathf.Max(0, _status.CurrentHp.Value - value);
            if (_status.CurrentHp.Value == 0)
            {
                Dead();
            }
        }

        public void RecoveryHP(int value)
        {
            // ü���� ���̵�, MaxHp �ʰ��� ���ƾ���
            //_status.CurrentHp.Value = Mathf.Min(_status.MaxHp, _status.CurrentHp.Value + value);
            int hp = _status.CurrentHp.Value + value;

            _status.CurrentHp.Value = Mathf.Clamp(hp, 0, _status.MaxHp);
        }

        public void Dead()
        {
            Debug.Log("�÷��̾� ��� ó��");
        }

        public void SubscribeEvents()
        {
            _status.CurrentHp.Subscribe(SetHpUIGuage);
            //_status.IsAiming.Subscribe(value => SetActivateAimCamera(value));
            _status.IsAiming.Subscribe(_aimCamera.gameObject.SetActive);
            _status.IsAiming.Subscribe(SetAimAnimation); //SetAimAnimation�� value�� ��� �ٲ�� �˱�?
            
            _status.IsAttacking.Subscribe(SetAttackAnimation);
            
            _status.IsMoving.Subscribe(SetMoveAnimation);
            //_status.OnAiming += _aimCamera.gameObject.SetActive; //- CinemachineVirtualCamera.gameObject.SetActive�� �� �ɱ�?

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
            //�����ġ / �ִ��ġ
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

