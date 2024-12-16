using UnityEngine;
using static GameEnum;

public static class FactionUtility
{
    public static Color GetColorForFaction(GameFaction faction)
    {
        if (faction == GameFaction.Red)
        {
            return GameConstants.SAFERIO_RED;
        }
        else if (faction == GameFaction.Green)
        {
            return GameConstants.SAFERIO_GREEN;
        }
        else if (faction == GameFaction.Orange)
        {
            return GameConstants.SAFERIO_ORANGE;
        }
        else if (faction == GameFaction.Purple)
        {
            return GameConstants.SAFERIO_PURPLE;
        }
        else if (faction == GameFaction.Blue)
        {
            return GameConstants.SAFERIO_BLUE;
        }

        return GameConstants.SAFERIO_RED;
    }
}
