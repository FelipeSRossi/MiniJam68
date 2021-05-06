using UnityEngine;

public class LevelEvents
{
    public delegate void OnLevelStartedType();
    public static event OnLevelStartedType OnLevelStarted;
    public static void EmitBegin()
    {
        OnLevelStarted?.Invoke();
    }

    public delegate void OnWonType();
    public static event OnWonType OnWon;
    public static void EmitWon()
    {
        OnWon?.Invoke();
    }

    public delegate void OnDefeatType();
    public static event OnDefeatType OnDefeat;
    public static void EmitDefeat()
    {
        OnDefeat?.Invoke();
    }

    public delegate void OnSpawnAllyType(int unitIndex, Lane lane);
    public static event OnSpawnAllyType OnSpawnAlly;
    public static void EmitSpawnAlly(int unitIndex, Lane lane)
    {
        OnSpawnAlly?.Invoke(unitIndex, lane);
    }
}
