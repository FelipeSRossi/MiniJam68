using UnityEngine;

public class ParticleEvents
{
    public delegate void OnAttackParticleType(Vector3 position);
    public static event OnAttackParticleType OnAttackParticle;
    public static void EmitAttackParticle(Vector3 position)
    {
        OnAttackParticle?.Invoke(position);
    }


    public delegate void OnPowerRingType(Vector3 position, float radius);
    public static event OnPowerRingType OnPowerRing;
    public static void EmitPowerRing(Vector3 position, float radius = 1)
    {
        OnPowerRing?.Invoke(position, radius);
    }
}
