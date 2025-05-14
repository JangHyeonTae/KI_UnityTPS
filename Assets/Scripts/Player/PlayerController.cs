using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//참조 생성용 임시 네임스페이스 참조
//작업물 병합 시 삭제 예정
using PlayerMovement = Temp.PlayerMovement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool IsControlActivate { get; set; } = true;

        private PlayerStatus _status;
        private PlayerMovement _movement;

        [SerializeField] private GameObject _aimCamera;
        private GameObject _mainCamera;

        [SerializeField] private KeyCode _aimKey = KeyCode.Mouse1;

        private void Awake() => Init();
        private void OnEnable() => SubscribeEvents();
        private void Update() => HandlePlayerControl();
        private void OnDisable() => UnSubscribeEvents();
        
        private void Init()
        {
            _status = GetComponent<PlayerStatus>();
            _movement = GetComponent<PlayerMovement>();
            _mainCamera = Camera.main.gameObject;
        }

        private void HandlePlayerControl()
        {
            if (!IsControlActivate) return;

            HandleMovement();
            HandleAiming();
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

            // 몸체의 회전기능 SetbodyRotation
            Vector3 avatarDir;
            if (_status.IsAiming.Value) avatarDir = camRotateDir;
            else avatarDir = moveDir;

            _movement.SetAvatarRotation(avatarDir);
        }

        private void HandleAiming()
        {
            _status.IsAiming.Value = Input.GetKey(_aimKey);
        }

        public void SubscribeEvents()
        {
            _status.IsAiming.Subscribe(value => SetActivateAimCamera(value));
        
            //람다식 아닌버전

        }

        public void UnSubscribeEvents()
        {
            _status.IsAiming.UnSubscribe(value => SetActivateAimCamera(value));
        }

        private void SetActivateAimCamera(bool value)
        {
            _aimCamera.SetActive(value);
            _mainCamera.SetActive(!value);
        }
    }
}

