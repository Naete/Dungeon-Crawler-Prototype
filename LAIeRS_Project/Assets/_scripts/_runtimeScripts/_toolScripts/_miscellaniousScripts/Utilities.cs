using System;

public static class Utilities
{
    public static bool DiceChance(float chanceRate, float maxChanceRate = 1)
    {
        if (chanceRate > maxChanceRate)
            throw new Exception($"ChanceRate({chanceRate}) is higher than MaxChanceRate({maxChanceRate})." +
                                $"ChanceRate must be within 0 and MaxChanceRate({maxChanceRate})");
                
        return UnityEngine.Random.value < (chanceRate / maxChanceRate);
    }
}