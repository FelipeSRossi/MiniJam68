using UnityEngine;

public class TowerEvents
{
    public delegate void OnTowerDestroyedType(in TowerController tower);
    public static event OnTowerDestroyedType OnTowerDestroyed;
    public static void EmitTowerDestroyed(in TowerController tower)
    {
        OnTowerDestroyed?.Invoke(tower);
    }
}
