namespace TollCalculator.Helpers;

public delegate void TollFeeSpanAction(Span<TollFee> span);

public class TollFeeSpanOperations
{
    public static void KeepOnlyHighestFee(Span<TollFee> feeSpan)
    {
        if (feeSpan.IsEmpty)
        {
            return;
        }

        var highestFee = (Amount: feeSpan[0].Amount, Index: 0);

        for (var i = 1; i < feeSpan.Length; i++)
        {
            var current = feeSpan[i];
            if (current.Amount <= highestFee.Amount)
            {
                feeSpan[i] = TollFee.Free(feeSpan[i].Pass);
                continue;
            }

            feeSpan[highestFee.Index] = TollFee.Free(feeSpan[highestFee.Index].Pass);

            highestFee = (current.Amount, i);
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