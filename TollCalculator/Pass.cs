namespace TollCalculator;

public class Pass
{
    public Pass(DateTime dateTime, Vehicle vehicle)
    {
        this.DateTime = dateTime;
        this.Vehicle = vehicle;
    }

    public DateTime DateTime { get; init; }
    public Vehicle Vehicle { get; init; }

    public TimeOnly TimeOfDay => TimeOnly.FromDateTime(this.DateTime);
    public DateOnly Date => DateOnly.FromDateTime(this.DateTime);
}