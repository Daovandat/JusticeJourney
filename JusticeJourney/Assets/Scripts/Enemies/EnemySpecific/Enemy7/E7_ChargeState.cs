public class E7_ChargeState : ChargeState
{
    readonly Enemy7 enemy;

    public E7_ChargeState(Enemy7 enemy, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData)
        : base(enemy, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        entity.CheckIfShouldFlip();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (performMeleeRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (!isDetectingLedge || isDetectingWall)
        {
            if (!isPlayerInMaxAgroRange)
            {
                enemy.idleState.SetFlipAfterIdle(true);
                stateMachine.ChangeState(enemy.idleState);
            }
            else
            {
                entity.SetVelocityX(0f);
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
        }
        else if (!canLeaveChargeState)
        {
            return;
        }
        else if (isChargeTimeOver)
        {
            if (isPlayerInMaxAgroRange)
            {
                stateMachine.ChangeState(enemy.playerDetectedState);
            }
            else
            {
                enemy.idleState.SetFlipAfterIdle(false);
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

}
