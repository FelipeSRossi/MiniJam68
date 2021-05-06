using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private GameObject attackParticle;
    [SerializeField] private GameObject powerRing;

    void OnEnable()
    {
        ParticleEvents.OnAttackParticle += OnAttackParticle;
        ParticleEvents.OnPowerRing += OnPowerRing;
    }

    void OnDisable()
    {
        ParticleEvents.OnAttackParticle -= OnAttackParticle;
        ParticleEvents.OnPowerRing -= OnPowerRing;
    }

    private void OnAttackParticle(Vector3 position)
    {
        var instance = Instantiate(attackParticle, position + (Vector3.back * 2f), Quaternion.identity);
        Destroy(instance, instance.GetComponent<ParticleSystem>().main.duration);
    }

    private void OnPowerRing(Vector3 position, float radius)
    {
        var instance = Instantiate(powerRing, position, Quaternion.identity);
        var width = radius * 2;
        var scale = instance.transform.localScale;
        instance.transform.localScale = new Vector3(width, scale.y, width);
        Destroy(instance, 0.2f);
    }
}
