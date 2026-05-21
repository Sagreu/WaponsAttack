using UnityEngine;

public static class SlotRules 
{
   public static int GetSlotForLevel(int level)
    {
        if (level < 3) return 1;
        if (level < 5) return 2;
        if (level < 7) return 3;
        if (level < 9) return 4;

        return 5;
    }
}
