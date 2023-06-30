namespace TollCalculator.Policies;

public class WeekdayExceptionsPolicy : FeePolicy
{
    private readonly DayOfWeek[] weekDays;
    private readonly FeePolicyDelegate next;

    public WeekdayExceptionsPolicy(DayOfWeek[] weekDays, FeePolicyDelegate next)
    {
        this.weekDays = weekDays;
        this.next = next;
    }

    public override TollFee Apply(Pass pass)
    {
        return weekDays.Contains(pass.DateTime.DayOfWeek) ? TollFee.Free(pass) : next(pass);
    }
}