public class Insurance
{
    public Funds fee;
    public Funds totalContributions;
    public Funds totalPayout;

    public Insurance(float startFee)
    {
        fee = new(startFee);
        totalContributions = new();
        totalPayout = new();
    }

    public void SettleLawsuit(float cost)
    {
        fee *= GameConfig.Instance.InsuranceMultiplier;
        totalPayout += cost;
    }
}