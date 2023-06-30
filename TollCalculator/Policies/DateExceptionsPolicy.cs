namespace TollCalculator.Policies;

public class DateExceptionsPolicy : FeePolicy
{
    private readonly DateOnly[] dates;
    private readonly FeePolicyDelegate next;

    public DateExceptionsPolicy(DateOnly[] dates, FeePolicyDelegate next)
    {
        this.dates = dates;
        this.next = next;
    }

    public override TollFee Apply(Pass pass)
    {
        return dates.Contains(pass.Date) ? TollFee.Free(pass) : next(pass);
    }
}