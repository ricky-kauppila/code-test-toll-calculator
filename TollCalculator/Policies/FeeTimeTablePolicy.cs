namespace TollCalculator.Policies;

public class FeeTimeTablePolicy : FeePolicy
{
    private readonly FeeTimeTable timeTable;

    public FeeTimeTablePolicy(FeeTimeTable timeTable)
    {
        this.timeTable = timeTable;
    }

    public override TollFee Apply(Pass pass)
    {
        return new TollFee(pass, timeTable.GetFee(pass.TimeOfDay));
    }
}