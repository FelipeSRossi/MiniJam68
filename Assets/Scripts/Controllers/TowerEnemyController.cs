public class TowerEnemyController : TowerController
{
    public override Party GetParty()
    {
        return Party.Enemy;
    }

    protected override void OnDeath()
    {
        TowerEvents.EmitTowerDestroyed(this);
    }
}
