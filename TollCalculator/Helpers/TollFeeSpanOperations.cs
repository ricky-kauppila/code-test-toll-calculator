namespace TollCalculator.Helpers;

public delegate void TollFeeSpanAction(Span<TollFee> span);

public class TollFeeSpanOperations
{
    public static void KeepOnlyHighestFee(Span<TollFee> feeSpan)
    {
        for (var j = 0; j < feeSpan.Length; j++)
        {
            for (var k = 0; k < feeSpan.Length; k++)
            {
                if (j != k && feeSpan[k].Amount >= feeSpan[j].Amount)
                {
                    feeSpan[j] = TollFee.Free(feeSpan[j].Pass);
                }
            }
        }
    }

    public static void IterateTimeSpans(TollFee[] fees, TimeSpan timeSpan, TollFeeSpanAction action)
    {
        var spanStartDateTime = fees[0].Pass.DateTime;
        var spanStartIndex = 0;
        var spanCount = 0;
        for (var i = 0; i < fees.Length; i++)
        {
            var isLast = i == fees.Length - 1;
            var current = fees[i];
            
            if (spanStartDateTime.IsWithinSpanOf(current.Pass.DateTime, timeSpan))
            {
                spanCount++;
                if (!isLast)
                {
                    continue;
                }
            }

            var tollFeeSpan = new Span<TollFee>(fees, spanStartIndex, spanCount);
            action(tollFeeSpan);

            spanStartIndex = i;
            spanCount = 0;
            spanStartDateTime = current.Pass.DateTime;
            if (!isLast)
            {
                i--;
            }
        }
    }


}