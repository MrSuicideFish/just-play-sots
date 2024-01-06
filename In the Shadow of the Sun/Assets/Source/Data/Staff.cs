public class Staff
{
    public const int StaffPerHire = 5;
    
    public int Count { get; set; }
    public int Total { get; private set; }


    public Staff(int total)
    {
        Total = Count = total;
    }

    public override string ToString()
    {
        return Count.ToString();
    }

    public void Hire()
    {
        Count += StaffPerHire;
        Total += StaffPerHire;
    }

    public void Fire()
    {
        Total -= StaffPerHire;
        if (Total < 0)
        {
            Total = 0;
        }
        
        Count -= StaffPerHire;
        if (Count < 0)
        {
            Count = 0;
        }
    }
}