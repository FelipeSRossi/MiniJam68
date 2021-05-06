using UnityEngine;

public class MinionEnemyController : MinionController
{
    public override Party GetParty()
    {
        return Party.Enemy;
    }
}
