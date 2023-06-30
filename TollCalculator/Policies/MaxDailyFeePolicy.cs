namespace TollCalculator.Policies;

public class MaxDailyFeePolicy : FeesPolicy
{
    private readonly decimal maxAmount;

    public MaxDailyFeePolicy(decimal maxAmount)
    {
        this.maxAmount = maxAmount;
    }

    public override IEnumerable<TollFee> Apply(IEnumerable<TollFee> fees)
    {
        var dailyFees = new Dictionary<DateOnly, decimal>();

        foreach (var tollFee in fees)
        {
            if (!dailyFees.TryGetValue(tollFee.Pass.Date, out var dailyFee))
            {
                dailyFee = 0;
            }

            dailyFee += tollFee.Amount;
            if (dailyFee >= maxAmount)
            {
                var exceededAmount = dailyFee - maxAmount;
                dailyFee -= exceededAmount;
                var reducedFee = new TollFee(tollFee.Pass, tollFee.Amount - exceededAmount);
                yield return reducedFee;
            }
            else
            {
                yield return tollFee;
            }

            dailyFees[tollFee.Pass.Date] = dailyFee;
        }
    }
}