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

        var startOfHour = feesArray[0].Pass.DateTime;
        var hourlySpanStartIndex = 0;
        var hourlySpanCount = 0;

        for (var i = 0; i < feesArray.Length; i++)
        {
            var isLast = i == feesArray.Length - 1;
            var current = feesArray[i];

            if (IsWithinAnHourOfEachOther(startOfHour, current.Pass.DateTime))
            {
                hourlySpanCount++;
                if (!isLast)
                {
                    continue;
                }
            }

            var hourlySpan = new Span<TollFee>(feesArray, hourlySpanStartIndex, hourlySpanCount);
            for (var j = 0; j < hourlySpan.Length; j++)
            {
                for (var k = 0; k < hourlySpan.Length; k++)
                {
                    if (j != k && hourlySpan[k].Amount >= hourlySpan[j].Amount)
                    {
                        hourlySpan[j] = TollFee.Free(hourlySpan[j].Pass);
                    }
                }
            }

            hourlySpanStartIndex = i;
            hourlySpanCount = 0;
            startOfHour = current.Pass.DateTime;
            if (!isLast)
            {
                i--;
            }
        }

        return feesArray;
    }

    private bool IsWithinAnHourOfEachOther(DateTime first, DateTime second)
    {
        return first.AddHours(1) > second && first.AddHours(-1) <= second;
    }
}