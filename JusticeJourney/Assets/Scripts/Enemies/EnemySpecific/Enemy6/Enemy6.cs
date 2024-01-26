using System.Collections;
using UnityEngine;

public class Enemy6 : Entity
{
    // Các trạng thái của đối tượng địch
    public E6_MoveState moveState { get; private set; }
    public E6_IdleState idleState { get; private set; }
    public E6_PlayerDetectedState playerDetectedState { get; private set; }
    public E6_MeleeAttackState meleeAttackState { get; private set; }
    public E6_StunState stunState { get; private set; }
    public E6_DeadState deadState { get; private set; }
    public E6_TeleportState teleportState { get; private set; }
    public E6_RangeAttackState rangeAttackState { get; private set; }
    public E6_RespawnState respawnState { get; private set; }
    // Các vị trí và transform liên quan
    [SerializeField] D_MoveState _moveStateData;
    [SerializeField] D_IdleState _idleStateData;
    [SerializeField] D_PlayerDetectedState _playerDetectedStateData;
    [SerializeField] D_MeleeAttackState _meleeAttackStateData;
    [SerializeField] D_StunState _stunStateData;
    [SerializeField] D_DeadState _deadStateData;
    [SerializeField] D_TeleportState _teleportStateData;
    [SerializeField] D_RangeAttackState _rangeAttackStateData;
    [SerializeField] D_RespawnState _respawnStateData;

    [SerializeField] Transform _ledgeBehindCheck;
    [SerializeField] Transform _meleeAttackPosition;
    [SerializeField] Transform _rangeAttackPosition;
    // Dữ liệu trạng thái teleport
    public D_TeleportState teleportStateData  { get { return _teleportStateData; } }
    // Transform của đầu (head)
    Transform _head;
    // Coroutine để reset vị trí cơ thể
    IEnumerator _resetBodyParts;
    // Phương thức khởi tạo khi đối tượng được tạo ra
    public override void Awake()
    {
        base.Awake();
        // Khởi tạo các trạng thái với dữ liệu tương ứng
        moveState = new E6_MoveState(this, stateMachine, "move", _moveStateData);
        idleState = new E6_IdleState(this, stateMachine, "idle", _idleStateData);
        playerDetectedState = new E6_PlayerDetectedState(this, stateMachine, "playerDetected", _playerDetectedStateData);
        meleeAttackState = new E6_MeleeAttackState(this, stateMachine, "meleeAttack", _meleeAttackPosition, _meleeAttackStateData);
        stunState = new E6_StunState(this, stateMachine, "stun", _stunStateData);
        deadState = new E6_DeadState(this, stateMachine, "dead", _deadStateData);
        teleportState = new E6_TeleportState(this, stateMachine, "teleport", _teleportStateData);
        rangeAttackState = new E6_RangeAttackState(this, stateMachine, "rangeAttack", _rangeAttackPosition, _rangeAttackStateData);
        respawnState = new E6_RespawnState(this, stateMachine, "respawn", _respawnStateData);
        // Tìm transform của đầu
        _head = transform.Find("Body").Find("MoveHead");
    }
    // Phương thức được gọi khi đối tượng được bật
    public override void OnEnable()
    {
        base.OnEnable();
        // Reset vị trí cơ thể và khởi tạo trạng thái di chuyển
        ResetBodyPosition();
        stateMachine.Initialize(moveState);
    }
    // Phương thức xử lý khi đối tượng bị tấn công
    public override bool Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);
        // Kiểm tra trạng thái hiện tại của đối tượng
        if (stateMachine.currentState == deadState || stateMachine.currentState == respawnState)
            return false;
        // Xử lý khi đối tượng đã chết
        if (isDead)
        {
            JustDied();
        }
        // Xử lý khi đối tượng bị choáng
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }

        return true;
    }

    // Phương thức xử lý khi đối tượng bị tấn công bởi bề mặt
    public override void DamageBySurface()
    {
        base.DamageBySurface();

        if (stateMachine.currentState == deadState || stateMachine.currentState == respawnState)
            return;

        if (isDead)
            JustDied();
    }

    // Phương thức xử lý khi đối tượng chết
    public override void JustDied()
    {
        base.JustDied();

        stateMachine.ChangeState(deadState);
    }
    // Phương thức xử lý khi đối tượng bị choáng do parry của người chơi
    public override void StunnedByPlayerParry(int parryDirection)
    {
        base.StunnedByPlayerParry(parryDirection);
        // Kiểm tra trạng thái hiện tại của đối tượng
        if (stateMachine.currentState != deadState || stateMachine.currentState != respawnState)
            stateMachine.ChangeState(stunState);
    }
    // Phương thức xử lý khi đối tượng sử dụng Vaporize power-up
    public override void PowerupManager_Vaporize()
    {
        base.PowerupManager_Vaporize();

        Enemy6Pool.Instance.ReturnToPool(this);
    }
    // Phương thức xử lý khi đối tượng nhảy khi bị tấn công
    public override void DamageHop(float velocity)
    {
        if (stateMachine.currentState != teleportState)
            rb.velocity = new Vector2(rb.velocity.x, velocity);
    }

    // Phương thức điều chỉnh hướng quay của cơ thể để nhìn theo người chơi
    public override void RotateBodyToPlayer()
    {
        // Xác định hướng để quay cơ thể và đầu của đối tượng địch
        // (Mã này có sử dụng Quaternion để đảm bảo quay mượt mà)
        Vector3 direction;
        float angle;
        Quaternion _bodyLookAtRotation;
        Quaternion _headLookAtRotation;

        if (_resetBodyParts != null)
            StopCoroutine(_resetBodyParts);

        direction = (playerTransform.position - _head.position).normalized;

        if (direction.x > 0f)
        {
            if (facingDirection == -1)
            {
                SetVelocityX(0f);
                Flip();
            }

            angle = Vector2.SignedAngle(Vector2.right, direction);
            _bodyLookAtRotation = Quaternion.AngleAxis(angle, Vector3.forward);

            angle = Mathf.Clamp(angle, -20f, 20f);
             _headLookAtRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else
        {
            if (facingDirection == 1)
            {
                SetVelocityX(0f);
                Flip();
            }

            angle = Vector2.SignedAngle(-Vector2.right, direction);
            _bodyLookAtRotation = Quaternion.AngleAxis(-angle, Vector3.forward);

            angle = Mathf.Clamp(angle, -20f, 20f);
            _headLookAtRotation = Quaternion.AngleAxis(-angle > 40 ? 40 : -angle, Vector3.forward);
        }
        // Áp dụng hướng quay cho đầu và vị trí tấn công range
        _head.localRotation = Quaternion.Slerp(_head.localRotation, _headLookAtRotation, Time.deltaTime * 5f);
        _rangeAttackPosition.localRotation = _bodyLookAtRotation;
    }

    // Phương thức đặt lại vị trí cơ thể sau một khoảng thời gian
    public override void ResetBodyPosition()
    {
        if (_resetBodyParts != null)
            StopCoroutine(_resetBodyParts);

        _rangeAttackPosition.localRotation = Quaternion.Euler(0f, 0f, 0f);

        _resetBodyParts = ResetBodyParts();
        StartCoroutine(_resetBodyParts);
    }
    // Coroutine để đặt lại vị trí của cơ thể
    IEnumerator ResetBodyParts()
    {
        while (Mathf.Abs(_head.localRotation.z) > 0.01f)
        {
            _head.localRotation = Quaternion.Slerp(_head.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 5f);

            yield return new WaitForFixedUpdate();
        }
    }
    // Phương thức kiểm tra ledge phía sau đối tượng
    public override bool CheckLedgeBehind()
    {
        return Physics2D.Raycast(_ledgeBehindCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.ground);
    }
    // Phương thức để vẽ các hình học trên Scene để giúp xác định thông số của đối tượng
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (_meleeAttackStateData.attackDetails.Length > 0)
            Gizmos.DrawWireCube(_meleeAttackPosition.position, _meleeAttackStateData.attackDetails[0].size);
    }

}
