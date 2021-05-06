using System;
using UnityEngine;

[Serializable]
public class MinionAttributeAttack
{
    [SerializeField] public float cooldown;
    [SerializeField] public float distance;
    [SerializeField] public float speed;
    [SerializeField] public float recovery;
    [SerializeField] public int damage;
    [SerializeField] public int force;
}
