

using JetBrains.Annotations;

[System.Serializable]

public class Unlockables
{
    public FishingRods fishingRods;
    public Baits baits;
    public Hooks hooks;
    public int currency;
    public Buckets buckets;
    public Locations locations;

    public Unlockables()
    {
        fishingRods = new FishingRods();
        baits = new Baits();   
        hooks = new Hooks();
        buckets = new Buckets();
        locations = new Locations();
        currency = 0;
    }
}


[System.Serializable]
public class Locations
{
    public bool rockSea;
    public bool river;
    public bool deepSea;


    public Locations()
    {
        rockSea = true;
        river = false;
        deepSea = false;
    }
}


[System.Serializable]
public class Buckets
{
    public bool smallBucket;
    public bool mediumBucket;
    public bool largeBucket;    
    public Buckets()
    {
        smallBucket = true;
        mediumBucket = false;
        largeBucket = false;
    }
}

[System.Serializable]
public class FishingRods
{
    public bool basicRod;
    public bool betterRod;
    public bool spinningRod;
    

    //Default values.
    public FishingRods()
    {
        basicRod = true;
        betterRod = false;
        spinningRod = false;
        
    }
    
}

[System.Serializable]
public class Baits
{
    public bool gacha;
    public bool mondarina;
    public bool worm;

    public Baits()
    {
        gacha = true;
        mondarina = false;
        worm = false;
    }
}


[System.Serializable]
public class Hooks
{
    public bool smallhook;
    public bool bighook;
    public bool triplehook;


    public Hooks()
    {
        smallhook = true;
        bighook = false;
        triplehook = false;

    }
}