using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JHT_test //playercontroller 클래스 중복 될 수 있으니 namespace사용
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement _movement;
        public PlayerStatus _status;

        private void Update() => MoveTest();

        public void MoveTest()
        {
            // (회전 수행 후)좌우 회전에 대한 벡터 반환
            Vector3 camRotateDir = _movement.SetAimRotation();

            float moveSpeed;
            if (_status.IsAiming.Value) moveSpeed = _status.WalkSpeed;
            else moveSpeed = _status.RunSpeed;

            Vector3 moveDir =_movement.SetMove(moveSpeed);
            _status.IsMoving.Value = (moveDir != Vector3.zero);
            
            // TODO : 몸체의 회전기능 구현 후 호출해야 함
            
        }
    }
}

