using TollCalculator.Policies;

namespace TollCalculator.Tests;

public class HourlyFeesPolicyTests
{
    private HourlyFeesPolicy policy;

    [SetUp]
    public void Setup()
    {
        this.policy = new HourlyFeesPolicy();
    }

    [Test]
    public void Apply_GivenTollPasses_WhenMultiplePassesWithinHour_UsesTheHighestFeeWithinEachHour()
    {
        // Arrange
        var tollFees = new TollFee[]
        {
            new(new Pass(DateTime.Parse("2023-06-29 13:00"), new Car()), 10),
            new(new Pass(DateTime.Parse("2023-06-30 13:00"), new Car()), 10),
            new(new Pass(DateTime.Parse("2023-06-30 13:01"), new Car()), 30),
            new(new Pass(DateTime.Parse("2023-06-30 13:02"), new Car()), 15),
            new(new Pass(DateTime.Parse("2023-06-30 14:00"), new Car()), 10),
            new(new Pass(DateTime.Parse("2023-06-30 14:30"), new Car()), 5)
        };

        var expected = new TollFee[]
        {
            new(new Pass(DateTime.Parse("2023-06-29 13:00"), new Car()), 10),
            new(new Pass(DateTime.Parse("2023-06-30 13:00"), new Car()), 0),
            new(new Pass(DateTime.Parse("2023-06-30 13:01"), new Car()), 30),
            new(new Pass(DateTime.Parse("2023-06-30 13:02"), new Car()), 0),
            new(new Pass(DateTime.Parse("2023-06-30 14:00"), new Car()), 10),
            new(new Pass(DateTime.Parse("2023-06-30 14:30"), new Car()), 0)
        };

        // Act
        var result = this.policy.Apply(tollFees).ToArray();

        // Assert
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.That(result[i].Amount, Is.EqualTo(expected[i].Amount));
            Assert.That(result[i].Pass.DateTime, Is.EqualTo(expected[i].Pass.DateTime));
        }
    }
}