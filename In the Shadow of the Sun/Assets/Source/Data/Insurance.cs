public class Insurance
{
    public Funds fee;
    public Funds totalContributions;

    public Insurance(float startFee)
    {
        fee = new(startFee);
        totalContributions = new();
    }
}