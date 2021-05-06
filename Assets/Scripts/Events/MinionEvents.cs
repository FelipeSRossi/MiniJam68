
public class MinionEvents
{
    public delegate void OnMinionDeathType(MinionAttributePrices prices);
    public static event OnMinionDeathType OnMinionDeath;
    public static void EmitOnMinionDeath(MinionAttributePrices prices)
    {
        OnMinionDeath?.Invoke(prices);
    }
}
