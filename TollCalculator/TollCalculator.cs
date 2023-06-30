namespace TollCalculator;

using global::TollCalculator.Policies;

public class TollCalculator
{
    private readonly FeeTimeTable feeTimeTable;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public TollCalculator()
    {
        this.feeTimeTable = new FeeTimeTable(0);
        feeTimeTable.Add(TimeOnly.Parse("06:00"), TimeOnly.Parse("06:29"), 8.0m);
        feeTimeTable.Add(TimeOnly.Parse("06:30"), TimeOnly.Parse("06:59"), 13.0m);
        feeTimeTable.Add(TimeOnly.Parse("07:00"), TimeOnly.Parse("07:59"), 18.0m);
        feeTimeTable.Add(TimeOnly.Parse("08:00"), TimeOnly.Parse("08:29"), 13.0m);
        feeTimeTable.Add(TimeOnly.Parse("08:30"), TimeOnly.Parse("14:59"), 8.0m);
        feeTimeTable.Add(TimeOnly.Parse("15:00"), TimeOnly.Parse("15:29"), 13.0m);
        feeTimeTable.Add(TimeOnly.Parse("15:30"), TimeOnly.Parse("16:59"), 18.0m);
        feeTimeTable.Add(TimeOnly.Parse("17:00"), TimeOnly.Parse("17:59"), 13.0m);
        feeTimeTable.Add(TimeOnly.Parse("18:00"), TimeOnly.Parse("18:29"), 8.0m);
    }

    public decimal GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        var passes = dates.Select(d => new Pass(d, vehicle));

        var policies = new PolicyBuilder()
            .WithFees(feeTimeTable)
            .WithWeekdayExceptions(DayOfWeek.Saturday, DayOfWeek.Sunday)
            .WithDateExceptions(
                DateOnly.Parse("2023-12-24"),
                DateOnly.Parse("2023-12-25"),
                DateOnly.Parse("2023-12-26"),
                DateOnly.Parse("2023-12-31"))
            .WithMonthException(Month.July)
            .WithVehicleException()
            .WithMaxDailyFee(60)
            .AggregateHourly();

        var fees = policies.Run(passes);

        return fees.Sum(f => f.Amount);
    }

    public decimal GetTollFee(DateTime date, IVehicle vehicle)
    {
        var pass = new Pass(date, vehicle);

        var policies = new PolicyBuilder()
            .WithFees(feeTimeTable)
            .WithWeekdayExceptions(DayOfWeek.Saturday, DayOfWeek.Sunday)
            .WithDateExceptions(
                DateOnly.Parse("2023-12-24"),
                DateOnly.Parse("2023-12-25"),
                DateOnly.Parse("2023-12-26"),
                DateOnly.Parse("2023-12-31"))
            .WithMonthException(Month.July)
            .WithVehicleException();

        var fees = policies.Run(new[] { pass });

        return fees.Sum(f => f.Amount);
    }
}