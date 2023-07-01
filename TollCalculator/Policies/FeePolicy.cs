namespace TollCalculator.Policies;

public abstract class FeePolicy
{
    public abstract TollFee Apply(Pass pass);
}