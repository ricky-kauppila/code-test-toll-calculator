namespace TollCalculator.Tests;

public class TollCalculatorTests
{
    private TollCalculator tollCalculator;

    [SetUp]
    public void Setup()
    {
        tollCalculator = new TollCalculator();
    }

    [TestCase("2023-06-24 08:00", TestName = "{m}(Saturday)")]
    [TestCase("2023-06-25 08:00", TestName = "{m}(Sunday)")]
    [TestCase("2023-12-24 08:00", TestName = "{m}(Day before public holiday)")]
    [TestCase("2023-12-25 08:00", TestName = "{m}(Public holiday)")]
    [TestCase("2023-07-03 08:00", TestName = "{m}(Month of July)")]
    public void GetTollFee_WhenWithinTollFreeTimeRange_ReturnsZero(string dateAndTime)
    {
        // Arrange
        var date = DateTime.Parse(dateAndTime);
        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(date, vehicle);

        // Assert
        Assert.That(tollFee, Is.Zero);
    }

    [TestCase("2023-06-28 06:00", 8.0)]
    [TestCase("2023-06-28 06:30", 13.0)]
    [TestCase("2023-06-28 07:00", 18.0)]
    [TestCase("2023-06-28 08:00", 13.0)]
    [TestCase("2023-06-28 08:30", 8.0)]
    [TestCase("2023-06-28 15:00", 13.0)]
    [TestCase("2023-06-28 15:30", 18.0)]
    [TestCase("2023-06-28 17:00", 13.0)]
    [TestCase("2023-06-28 18:00", 8.0)]
    public void GetTollFee_WhenOutsideTollFreeTimeRange_ReturnsCorrectFee(string dateAndTime, decimal expectedFee)
    {
        // Arrange
        var date = DateTime.Parse(dateAndTime);
        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(date, vehicle);

        // Assert
        Assert.That(tollFee, Is.EqualTo(expectedFee));
    }


    [Test]
    public void GetTollFee_WhenTollFreeVehicle_ReturnsZero()
    {
        // Arrange
        var date = new DateTime(2023, 6, 28, 8, 0, 0);
        var vehicle = new Motorbike();

        // Act
        var tollFee = tollCalculator.GetTollFee(date, vehicle);

        // Assert
        Assert.That(tollFee, Is.Zero);
    }

    [Test]
    public void GetTollFee_GivenMultiplePasses_WhenAllWithinOneHour_ReturnsHighestAmount()
    {
        // Arrange
        const decimal ExpectedFee = 13;

        var passes = new[]
        {
            DateTime.Parse("2023-06-28 06:00"),
            DateTime.Parse("2023-06-28 06:30"),
            DateTime.Parse("2023-06-28 06:59")
        };

        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(vehicle, passes);

        // Assert
        Assert.That(tollFee, Is.EqualTo(ExpectedFee));
    }

    [Test]
    public void GetTollFee_GivenMultiplePasses_WhenFeeExceedsMaxDailyAmount_ReturnsMaxDailyAmount()
    {
        // Arrange
        const decimal ExpectedFee = 60;

        var passes = new[]
        {
            DateTime.Parse("2023-06-28 06:00"), // 8
            DateTime.Parse("2023-06-28 07:01"), // 18
            DateTime.Parse("2023-06-28 08:02"), // 13
            DateTime.Parse("2023-06-28 09:03"), // 8
            DateTime.Parse("2023-06-28 10:04"), // 8
            DateTime.Parse("2023-06-28 11:05") // 8
        };

        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(vehicle, passes);

        // Assert
        Assert.That(tollFee, Is.EqualTo(ExpectedFee));
    }
}