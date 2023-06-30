namespace TollCalculator;

public class FeeTimeTable
{
    private readonly decimal defaultAmount;
    public List<FeeTimeTableItem> Fees;

    public FeeTimeTable(decimal defaultAmount)
    {
        this.defaultAmount = defaultAmount;
        this.Fees = new List<FeeTimeTableItem>();
    }

    public void Add(TimeOnly start, TimeOnly end, decimal amount)
    {
        this.Fees.Add(new FeeTimeTableItem(start, end, amount));
    }

    public decimal GetFee(TimeOnly timeOfDay)
    {
        return this.Fees.FirstOrDefault(fee => fee.Start <= timeOfDay && fee.End >= timeOfDay)?.Amount ?? this.defaultAmount;
    }
}