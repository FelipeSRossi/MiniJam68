
public class HUDEvents
{
    public delegate void OnUnitButtonStatusUpdateType(int index, bool enabled);
    public static event OnUnitButtonStatusUpdateType OnUnitButtonStatusUpdate;
    public static void EmitUnitButtonStatusUpdate(int index, bool enabled)
    {
        OnUnitButtonStatusUpdate?.Invoke(index, enabled);
    }
}
