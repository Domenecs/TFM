using System.Collections.Generic;
using UnityEngine;

public static class FishingData
{
    // Enums centrales
    public enum RodType
    {
        Simple,
        Intermediate
    }

    public enum FishSize
    {
        S,
        M,
        L,
        XL,
        XXL
    }

    public enum BaitType
    {
        Gacha,
        Mandarina,
        Gusano,
        Jig
    }

    public enum HookType
    {
        SmallHook,
        BigHook,
        TripleHook
    }

    public static Dictionary<HookType, int> HookChances = new Dictionary<HookType, int>()
{
    { HookType.SmallHook, 80 },
    { HookType.BigHook, 90 },
    { HookType.TripleHook, 100 }
};


    // Size depending the rod
    public static readonly Dictionary<RodType, Dictionary<FishSize, float>> RodSizeChances = new()
    {
        {
            RodType.Simple, new()
            {
                { FishSize.S, 40f },
                { FishSize.M, 30f },
                { FishSize.L, 15f },
                { FishSize.XL, 10f },
                { FishSize.XXL, 5f },
            }
        },
        {
            RodType.Intermediate, new()
            {
                { FishSize.S, 10f },
                { FishSize.M, 20f },
                { FishSize.L, 30f },
                { FishSize.XL, 25f },
                { FishSize.XXL, 15f },
            }
        }


    };

    public static readonly Dictionary<FishSize, float> ScaleMultipliers = new()
    {
        { FishSize.S, 1.0f },
        { FishSize.M, 1.1f },
        { FishSize.L, 1.2f },
        { FishSize.XL, 1.3f },
        { FishSize.XXL, 1.4f },
    };

    public static readonly Dictionary<FishSize, int> Frequencies = new()
    {
        { FishSize.S, 250 },
        { FishSize.M, 200 },
        { FishSize.L, 150 },
        { FishSize.XL, 100 },
        { FishSize.XXL, 50 },
    };



}
