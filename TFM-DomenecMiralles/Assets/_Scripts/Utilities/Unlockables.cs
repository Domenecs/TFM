

[System.Serializable]

public class Unlockables
{
    public FishingRods fishingRods;
    public Baits baits;
    public Hooks hooks;
    public int currency;


    public Unlockables()
    {
        fishingRods = new FishingRods();
        baits = new Baits();   
        hooks = new Hooks();
        currency = 0;
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