namespace TollCalculator.Policies;

public class MonthExceptionPolicy : FeePolicy
{
    private readonly Month[] months;
    private readonly FeePolicyDelegate next;

    public MonthExceptionPolicy(Month[] months, FeePolicyDelegate next)
    {
        this.months = months;
        this.next = next;
    }

    public override TollFee Apply(Pass pass)
    {
        if (this.months.Contains((Month)pass.Date.Month))
        {
            return TollFee.Free(pass);
        }

        return next(pass);
    }
}