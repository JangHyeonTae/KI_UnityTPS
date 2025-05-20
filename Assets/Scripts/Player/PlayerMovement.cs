using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        //��ü�� �����̴°�, ��ü�� ȸ��, ��ü�� �����̴°�(camera �ȿ���)

        [Header("References")]
        [SerializeField] private Transform _avatar;
        [SerializeField] private Transform _aim;

        private Rigidbody rigid;
        private PlayerStatus _playerStatus;

        [Header("Mouse Config")] //������ ���� class������༭ ������ ����
        [SerializeField][Range(-90, 0)] private float _minPitch; //�ּҰ���
        [SerializeField][Range(0, 90)] private float _maxPitch; //�ִ밢��
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

        //�� Vector3? �ٽú���
        public Vector3 SetMove(float moveSpeed)
        {
            //�����̵�(�̵�����)
            Vector3 moveDirection = GetMoveDirection();

            Vector3 velocity = rigid.velocity;
            velocity.x = moveDirection.x * moveSpeed;
            velocity.z = moveDirection.z * moveSpeed;

            rigid.velocity = velocity;

            return moveDirection;
        }
         
        //� �������� ȸ���ϰ��ִ��� -Vector3
        public Vector3 SetAimRotation()
        {
            //������ ȸ��
            //x�� ������ ������, y�� ������ ������
            //Vector2 mouseDir = GetMouseDirection();
            
            //y�� ȸ�� ���� �ٽ�
            //x���� ����� ������ �� �ʿ� ����
            _currentRotation.x += MouseDirection.x;

            //y���� ��쿣 ���� ������ �ɾ����
            _currentRotation.y =
                Mathf.Clamp(_currentRotation.y + MouseDirection.y, _minPitch, _maxPitch);

            //�ϳ� �� �����ؾ��� ��� : ĳ���� ������Ʈ�� ��쿡�� y�� ȸ�����ݿ�
            transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

            //������ ��� ���� ȸ�� �ݿ�
            Vector3 currentEuler = _aim.localEulerAngles; //���� ������ ������ �޾ƿ�
                                                          //aim�� ���� ȸ���� �ؾ��ϴµ� x���� �����ؾ� ���ΰ� ȸ���� �� �׷��Ƿ� currentRotation.y�� ��� -> �ٽ�
            _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

            //ȸ�� ���� ���� ��ȯ -> �����
            //��� �������� ȸ���� �ߴ��� ��ȯ?
            Vector3 rotateDirVector = transform.forward;
            rotateDirVector.y = 0; //y���� ȸ���� ������� �ʰ�

            return rotateDirVector.normalized;
        }

        public void SetAvatarRotation(Vector3 direction)
        {
            //�ƹ�Ÿ�� ȸ���ϵ���
            if (direction == Vector3.zero) return;
            //��� �������� ȸ���ؾ����� ����
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //���� �� �ε巴�� Lerp
            _avatar.rotation = Quaternion.Lerp(_avatar.rotation, targetRotation, _playerStatus.RotateSpeed * Time.deltaTime);

        }

        //������ 
        public Vector3 GetMoveDirection()
        {
            Vector3 input = InputDirection;

            //���� ���������ϴ� ���� - �÷��̾� ��ġ��� ���������ϴ� ����
            Vector3 direction =
                (transform.right * input.x) +
                (transform.forward * input.y);

            //vector3�� ���� ��������
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

        //���콺 - InputManager
        //private Vector2 GetMouseDirection()
        //{
        //    float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;//������ ���콺 - Horizontal
        //    float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;//������ ���콺 - Vertical, �ݴ�� �����������
        //
        //    return new Vector2(mouseX, mouseY);
        //}
    }



}