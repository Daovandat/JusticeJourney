using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationToStateMachine : MonoBehaviour
{
    // Các biến để lưu trữ đối tượng quản lý trạng thái
    public AttackState attackState;
    public DeadState deadState;
    public RespawnState respawnState;
    public TeleportState teleportState;

    // Khi bắt đầu hành động tấn công
    void StartAttack()
    {
        attackState.StartAttack();
    }

    // Khi kích hoạt hành động tấn công
    void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    // Khi hành động tấn công hoàn thành
    void FinishedAttack()
    {
        attackState.FinishAttack();
    }

    // Khi đối tượng chết
    void Dead()
    {
        deadState.Dead();
    }

    // Khi đối tượng được hồi sinh
    void Respawned()
    {
        respawnState.Respawned();
    }

    // Khi đối tượng bắt đầu thực hiện chuyển động
    void Teleported()
    {
        teleportState.TeleportStarted();
    }

    // Khi đối tượng kết thúc chuyển động
    void TeleportEnded()
    {
        teleportState.TeleportEnded();
    }
}
