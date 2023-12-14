public class Staff
{
    public int Count { get; private set; }

    public override string ToString()
    {
        return Count.ToString();
    }
}