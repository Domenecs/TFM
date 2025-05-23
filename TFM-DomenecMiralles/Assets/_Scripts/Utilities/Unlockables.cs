

[System.Serializable]

public class Unlockables
{
    public FishingRods fishingRods;
    
    public Unlockables()
    {
        fishingRods = new FishingRods();
    }
}




[System.Serializable]
public class FishingRods
{
    public bool basicRod;
    public bool spinningRod;
    public bool proRod;

    //Default values.
    public FishingRods()
    {
        basicRod = true;
        spinningRod = false;
        proRod = false;
    }
    
}