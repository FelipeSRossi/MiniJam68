using UnityEngine;

public class MinionAllyController : MinionController
{
    public override Party GetParty()
    {
        return Party.Ally;
    }
}
