using UnityEngine;

public abstract class TowerController : MonoBehaviour
{
    [SerializeField] protected int hitPoints = 500;
    [SerializeField] protected Animator animator;

    void OnDamageInflicted(MinionAttributeAttack attack)
    {
        ParticleEvents.EmitAttackParticle(transform.position);
        animator.SetTrigger("Damage");
        hitPoints -= attack.damage;

        if (hitPoints <= 0)
        {
            OnDeath();
        }
    }

    public abstract Party GetParty();
    protected abstract void OnDeath();
}
