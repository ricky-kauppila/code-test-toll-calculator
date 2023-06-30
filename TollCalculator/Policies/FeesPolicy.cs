namespace TollCalculator.Policies;

public abstract class FeesPolicy
{
    public abstract IEnumerable<TollFee> Apply(IEnumerable<TollFee> tollFees);
}