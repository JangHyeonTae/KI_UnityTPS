using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //몸체가 움직이는것, 몸체가 회전, 몸체만 움직이는것(camera 안움직)

        [Header("References")]
        [SerializeField] private Transform _avatar;
        [SerializeField] private Transform _aim;

        private Rigidbody rigid;
        private PlayerStatus _playerStatus;

        [Header("Mouse Config")] //원래는 따로 class만들어줘서 구현이 맞음
        [SerializeField][Range(-90, 0)] private float _minPitch; //최소각도
        [SerializeField][Range(0, 90)] private float _maxPitch; //최대각도
        [SerializeField][Range(0, 5)] private float _mouseSensitivity = 1;

        Vector2 _currentRotation; 
        public Vector2 InputDirection { get; private set; }
        public Vector2 MouseDirection { get; private set; }

        private void Awake() => Init();

        private void Init()
        {
            rigid = GetComponent<Rigidbody>();
            _playerStatus = GetComponent<PlayerStatus>();
        }

        //왜 Vector3? 다시보기
        public Vector3 SetMove(float moveSpeed)
        {
            //실제이동(이동방향)
            Vector3 moveDirection = GetMoveDirection();

            Vector3 velocity = rigid.velocity;
            velocity.x = moveDirection.x * moveSpeed;
            velocity.z = moveDirection.z * moveSpeed;

            rigid.velocity = velocity;

            return moveDirection;
        }
         
        //어떤 방향으로 회전하고있는지 -Vector3
        public Vector3 SetAimRotation()
        {
            //에임의 회전
            //x는 가로의 움직임, y는 세로의 움직임
            //Vector2 mouseDir = GetMouseDirection();
            
            //y축 회전 설명 다시
            //x축의 경우라면 제한을 걸 필요 없음
            _currentRotation.x += MouseDirection.x;

            //y축의 경우엔 각도 제한을 걸어야함
            _currentRotation.y =
                Mathf.Clamp(_currentRotation.y + MouseDirection.y, _minPitch, _maxPitch);

            //하나 더 생각해야할 경우 : 캐릭터 오브젝트의 경우에는 y축 회전만반영
            transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

            //에임의 경우 상하 회전 반영
            Vector3 currentEuler = _aim.localEulerAngles; //현재 에임의 각도를 받아옴
                                                          //aim은 상하 회전을 해야하는데 x축을 변경해야 세로가 회전이 됨 그러므로 currentRotation.y를 사용 -> 다시
            _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

            //회전 방향 벡터 반환 -> 물어보기
            //어느 방향으로 회전을 했는지 반환?
            Vector3 rotateDirVector = transform.forward;
            rotateDirVector.y = 0; //y축의 회전은 적용되지 않게

            return rotateDirVector.normalized;
        }

        public void SetAvatarRotation(Vector3 direction)
        {
            //아바타만 회전하도록
            if (direction == Vector3.zero) return;
            //어느 방향으로 회전해야할지 설정
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //조금 더 부드럽게 Lerp
            _avatar.rotation = Quaternion.Lerp(_avatar.rotation, targetRotation, _playerStatus.RotateSpeed * Time.deltaTime);

        }

        //움직임 
        public Vector3 GetMoveDirection()
        {
            Vector3 input = InputDirection;

            //실제 움직여야하는 방향 - 플레이어 위치기반 움직여아하는 방향
            Vector3 direction =
                (transform.right * input.x) +
                (transform.forward * input.y);

            //vector3의 방향 가져오기
            return direction.normalized;
        }
        //InputSystem - callback
        
        public void OnMove(InputValue value)
        {
            InputDirection = value.Get<Vector2>();
        }

        public void OnRotate(InputValue value)
        {
            Vector2 mouseDir = value.Get<Vector2>();
            mouseDir.y *= -1;
            MouseDirection = mouseDir * _mouseSensitivity;
        }

        public Vector3 GlobalPos()
        {
            Vector3 globalPos = gameObject.transform.position;
            return globalPos;
        }

        //InputManager
        //public Vector3 GetInputDirection()
        //{
        //    float x = Input.GetAxisRaw("Horizontal");
        //    float z = Input.GetAxisRaw("Vertical");
        //
        //    return new Vector3(x, 0, z);
        //}

        //마우스 - InputManager
        //private Vector2 GetMouseDirection()
        //{
        //    float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;//가로축 마우스 - Horizontal
        //    float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;//세로축 마우스 - Vertical, 반대로 적용해줘야함
        //
        //    return new Vector2(mouseX, mouseY);
        //}
    }



}