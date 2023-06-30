namespace TollCalculator.Policies;

public class PolicyBuilder
{
    private FeeTimeTable feeTimeTable;
    private bool vehicleException;
    private decimal maxFee = 0;
    private bool aggregateHourly;
    private DayOfWeek[] weekdayExceptions;
    private DateOnly[] dateExceptions;

    public PolicyBuilder()
    {
        feeTimeTable = null;
        vehicleException = false;
        maxFee = 0;
        aggregateHourly = false;
        weekdayExceptions = Array.Empty<DayOfWeek>();
        dateExceptions = Array.Empty<DateOnly>();
    }

    public PolicyBuilder WithFees(FeeTimeTable table)
    {
        this.feeTimeTable = table;
        return this;
    }

    public PolicyBuilder WithWeekdayExceptions(params DayOfWeek[] daysOfWeek)
    {
        weekdayExceptions = daysOfWeek;
        return this;
    }

    public PolicyBuilder WithDateExceptions(params DateOnly[] dates)
    {
        dateExceptions = dates;
        return this;
    }

    public PolicyBuilder WithVehicleException()
    {
        vehicleException = true;
        return this;
    }

    public PolicyBuilder WithMaxDailyFee(decimal amount)
    {
        maxFee = amount;
        return this;
    }


    public PolicyBuilder AggregateHourly()
    {
        aggregateHourly = true;
        return this;
    }


    public TollFee[] Run(IEnumerable<Pass> enumerable)
    {
        var timeTablePolicy = new FeeTimeTablePolicy(this.feeTimeTable);
        var weekdayPolicy = new WeekdayExceptionsPolicy(weekdayExceptions, timeTablePolicy.Apply);
        var datePolicy = new DateExceptionsPolicy(this.dateExceptions, weekdayPolicy.Apply);
        var vehicleTypePolicy = new VehicleTypeExceptionsPolicy(datePolicy.Apply);

        var maxDailyFeePolicy = new MaxDailyFeePolicy(maxFee);
        var hourlyFeesPolicy = new HourlyFeesPolicy();

        var result = enumerable.Select(pass => vehicleTypePolicy.Apply(pass));

        if (this.maxFee > 0)
        {
            result = maxDailyFeePolicy.Apply(result);
        }

        if (this.aggregateHourly)
        {
            result = hourlyFeesPolicy.Apply(result);
        }

        return result.ToArray();
    }
}