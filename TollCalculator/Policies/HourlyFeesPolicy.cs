using TollCalculator.Helpers;

namespace TollCalculator.Policies;

public class HourlyFeesPolicy : FeesPolicy
{
    public override IEnumerable<TollFee> Apply(IEnumerable<TollFee> fees)
    {
        var feesArray = fees.OrderBy(f => f.Pass.DateTime).ToArray();

        if (!feesArray.Any())
        {
            return Enumerable.Empty<TollFee>();
        }

        TollFeeSpanOperations.IterateTimeSpans(
            feesArray,
            TimeSpan.FromHours(1),
            TollFeeSpanOperations.KeepOnlyHighestFee);

        return feesArray;
    }
}