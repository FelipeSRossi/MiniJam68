public class TowerAllyController : TowerController
{
    public override Party GetParty()
    {
        return Party.Ally;
    }

    protected override void OnDeath()
    {
        TowerEvents.EmitTowerDestroyed(this);
    }
}
