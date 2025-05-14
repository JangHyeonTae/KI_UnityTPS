using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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

    Vector3 _currentRotation;
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
        Vector2 mouseDir = GetMouseDirection();

        //���ι����� �� ���Ƶ� �ż� ���������, �������� �����ž��ؼ� ����
        //Vector2 currentRotation = new()
        //{
        //    x = transform.rotation.eulerAngles.x,
        //    y = transform.rotation.eulerAngles.y
        //};

        //y�� ȸ�� ���� �ٽ�
        //x���� ����� ������ �� �ʿ� ����
        _currentRotation.x += mouseDir.x;

        //y���� ��쿣 ���� ������ �ɾ����
        _currentRotation.y = 
            Mathf.Clamp(_currentRotation.y + mouseDir.y, _minPitch, _maxPitch);

        //�ϳ� �� �����ؾ��� ��� : ĳ���� ������Ʈ�� ��쿡�� y�� ȸ�����ݿ�
        transform.rotation = Quaternion.Euler(0, _currentRotation.x, 0);

        //������ ��� ���� ȸ�� �ݿ�
        Vector3 currentEuler = _aim.localEulerAngles; //���� ������ ������ �޾ƿ�
        _aim.localEulerAngles = new Vector3(_currentRotation.y, currentEuler.y, currentEuler.z);

        //ȸ�� ���� ���� ��ȯ
        Vector3 rotateDirVector = transform.forward;
        rotateDirVector.y = 0;

        return rotateDirVector.normalized;
    }

    public void SetBodyRotation()
    {

    }

    //���콺 
    private Vector2 GetMouseDirection()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity;//������ ���콺 - Horizontal
        float mouseY = -Input.GetAxis("Mouse Y") * _mouseSensitivity;//������ ���콺 - Vertical, �ݴ�� �����������

        return new Vector2(mouseX, mouseY);
    }
    
    //������ 
    public Vector3 GetMoveDirection()
    {
        Vector3 input = GetInputDirection();

        //���� ���������ϴ� ���� - �÷��̾� ��ġ��� ���������ϴ� ����
        Vector3 direction =
            (transform.right * input.x) + 
            (transform.forward * input.z);

        //vector3�� ���� ��������
        return direction.normalized;
    }

    public Vector3 GetInputDirection()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        return new Vector3(x, 0, z);
    }
}

















