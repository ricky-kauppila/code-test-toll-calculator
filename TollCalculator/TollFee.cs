namespace TollCalculator;

public class TollFee
{
    public TollFee(Pass pass, decimal amount)
    {
        this.Pass = pass;
        this.Amount = amount;
    }

    public Pass Pass { get; init; }
    public decimal Amount { get; init; }

    public static TollFee Free(Pass pass) => new(pass, 0);

    public override string ToString()
    {
        return $"{this.Pass.DateTime} - {this.Amount:C}";
    }
}