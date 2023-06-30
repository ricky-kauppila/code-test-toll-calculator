namespace TollCalculator;

public class Pass
{
    public Pass(DateTime dateTime, IVehicle vehicle)
    {
        this.DateTime = dateTime;
        this.Vehicle = vehicle;
    }

    public DateTime DateTime { get; init; }
    public IVehicle Vehicle { get; init; }

    public TimeOnly TimeOfDay => TimeOnly.FromDateTime(this.DateTime);
    public DateOnly Date => DateOnly.FromDateTime(this.DateTime);
}