using UnityEngine;

public abstract class MinionController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 0.5f;

    public abstract Party GetParty();

    [SerializeField] private MinionAttributeAttack attack;
    [SerializeField] private MinionAttributeResistence resistence;
    [SerializeField] private MinionAttributePrices prices;
    [SerializeField] private LayerMask enemyMask;

    private Timer attackCooldownTimer;
    private Timer attackSpeedTimer;
    private Timer attackRecoveryTimer;

    private float horizontalVelocity;

    [SerializeField] private MinionStates state;
    [SerializeField] private Animator animator;

    private Rigidbody minionBody;
    private BoxCollider minionCollider;

    void Start()
    {
        attackCooldownTimer = new Timer(attack.cooldown);
        attackSpeedTimer = new Timer(attack.speed);
        attackRecoveryTimer = new Timer(attack.recovery);

        minionBody = GetComponent<Rigidbody>();
        minionCollider = GetComponent<BoxCollider>();

        horizontalVelocity = 0;
        state = MinionStates.Walking;
    }

    void FixedUpdate()
    {
        UpdateTimers();
        UpdateVelocity();

        switch (state)
        {
            case MinionStates.Walking: HandleWalk(); break;
            case MinionStates.Attacking: HandleAttack(); break;
            case MinionStates.Recovering: HandleRecover(); break;
            case MinionStates.Bouncing: HandleBounce(); break;
            case MinionStates.Dead: HandleDead(); break;
        }
    }

    void OnDrawGizmos()
    {
        if (minionCollider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, minionCollider.bounds.extents.x + 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, GetAttackRadius());
        }
    }

    private void KillMe()
    {
        MinionEvents.EmitOnMinionDeath(prices);
        //animator.SetTrigger("Defeat");
        minionCollider.enabled = false;
        Destroy(gameObject, 1);
        UpdateState(MinionStates.Dead);
    }

    private void HandleDead()
    {
        horizontalVelocity = 0;
    }

    private void HandleWalk()
    {
        if (Physics.OverlapSphere(transform.position, minionCollider.bounds.extents.x + 0.1f, enemyMask).Length == 0)
        {
            horizontalVelocity = GetForwardVelocity();
        }
        else
        {
            horizontalVelocity = 0;
        }

        if (CanAttack())
        {
            UpdateState(MinionStates.Attacking);
            attackSpeedTimer.Reset();
        }
    }

    private void HandleAttack()
    {
        horizontalVelocity = 0;

        if (!attackSpeedTimer.isFinished())
        {
            return;
        }

        InflictDamage();

        UpdateState(MinionStates.Recovering);
        attackRecoveryTimer.Reset();
        attackCooldownTimer.Reset();
    }

    private void HandleRecover()
    {
        horizontalVelocity = 0;

        if (!attackRecoveryTimer.isFinished())
        {
            return;
        }

        UpdateState(MinionStates.Walking);
    }

    private void HandleBounce()
    {
        horizontalVelocity += GetForwardVelocity();

        if (GetDirection() * horizontalVelocity >= 0)
        {
            UpdateState(MinionStates.Walking);
        }
    }

    void OnDamageInflicted(MinionAttributeAttack attack)
    {
        ParticleEvents.EmitAttackParticle(transform.position);
        resistence.hitPoints -= attack.damage;

        if (resistence.hitPoints <= 0)
        {
            KillMe();
            return;
        }

        horizontalVelocity = GetBounceVelocity(attack.force);

        UpdateState(MinionStates.Bouncing);
    }

    private void InflictDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetAttackRadius(), enemyMask);

        foreach (var collider in colliders)
        {
            collider.SendMessage("OnDamageInflicted", attack);
        }

        ParticleEvents.EmitPowerRing(transform.position, GetAttackRadius());
        attackCooldownTimer.Reset();
    }

    private bool CanAttack()
    {
        if (!attackCooldownTimer.isFinished())
        {
            return false;
        }

        return Physics.OverlapSphere(transform.position, GetAttackRadius(), enemyMask).Length > 0;
    }

    private void UpdateTimers()
    {
        attackCooldownTimer.Update(Time.deltaTime);
        attackRecoveryTimer.Update(Time.deltaTime);
        attackSpeedTimer.Update(Time.deltaTime);
    }

    private void UpdateState(MinionStates state)
    {
        this.state = state;
    }

    private void UpdateVelocity()
    {
        minionBody.velocity = new Vector3(horizontalVelocity, minionBody.velocity.y, minionBody.velocity.z);
    }

    private float GetDirection()
    {
        return GetParty().Equals(Party.Ally) ? -1.0f : 1.0f;
    }

    private float GetForwardVelocity()
    {
        return GetDirection() * movementSpeed;
    }

    private float GetBounceVelocity(float strength)
    {
        return GetDirection() * -1 * strength * (1 - resistence.toughness);
    }

    private float GetAttackRadius()
    {
        return attack.distance + minionCollider.bounds.extents.x;
    }

    public int GetCost()
    {
        return prices.cost;
    }
}
