public class Staff
{
    public const int StaffPerHire = 10;
    
    public int Count { get; private set; }

    public override string ToString()
    {
        return Count.ToString();
    }

    public void Hire()
    {
        Count += StaffPerHire;
    }

    public void Fire()
    {
        Count -= StaffPerHire;
    }
}