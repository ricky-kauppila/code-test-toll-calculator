namespace TollCalculator.Policies;

public class PolicyBuilder
{
    private FeeTimeTable feeTimeTable;
    private bool vehicleException;
    private decimal maxFee = 0;
    private bool aggregateHourly;
    private DayOfWeek[] weekdayExceptions;
    private DateOnly[] dateExceptions;
    private Month[] monthExceptions;

    public PolicyBuilder()
    {
        this.feeTimeTable = new FeeTimeTable(0);
        vehicleException = false;
        maxFee = 0;
        aggregateHourly = false;
        weekdayExceptions = Array.Empty<DayOfWeek>();
        dateExceptions = Array.Empty<DateOnly>();
        monthExceptions = Array.Empty<Month>();
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

    public PolicyBuilder WithMonthException(params Month[] months)
    {
        monthExceptions = months;
        return this;
    }

    public TollFee[] Run(IEnumerable<Pass> enumerable)
    {
        var policyStack = new Stack<FeePolicyDelegate>();

        var timeTablePolicy = new FeeTimeTablePolicy(this.feeTimeTable);
        policyStack.Push(timeTablePolicy.Apply);

        if (this.monthExceptions.Any())
        {
            var monthExceptionPolicy = new MonthExceptionPolicy(this.monthExceptions, policyStack.Pop());
            policyStack.Push(monthExceptionPolicy.Apply);
        }

        if (this.weekdayExceptions.Any())
        {
            var weekdayPolicy = new WeekdayExceptionsPolicy(weekdayExceptions, policyStack.Pop());
            policyStack.Push(weekdayPolicy.Apply);
        }

        if (this.dateExceptions.Any())
        {
            var datePolicy = new DateExceptionsPolicy(this.dateExceptions, policyStack.Pop());
            policyStack.Push(datePolicy.Apply);
        }

        if (this.vehicleException)
        {
            var vehicleTypePolicy = new VehicleTypeExceptionsPolicy(policyStack.Pop());
            policyStack.Push(vehicleTypePolicy.Apply);
        }

        var apply = policyStack.Pop();

        var result = enumerable.Select(pass => apply(pass));
        if (this.maxFee > 0)
        {
            var maxDailyFeePolicy = new MaxDailyFeePolicy(maxFee);
            result = maxDailyFeePolicy.Apply(result);
        }

        if (this.aggregateHourly)
        {
            var hourlyFeesPolicy = new HourlyFeesPolicy();
            result = hourlyFeesPolicy.Apply(result);
        }

        return result.ToArray();
    }
}