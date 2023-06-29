namespace TollCalculator.Tests;

public class TollCalculatorTests
{
    private TollCalculator tollCalculator;

    [SetUp]
    public void Setup()
    {
        tollCalculator = new TollCalculator();
    }

    [Test]
    public void GetTollFee_WhenTollFreeDate_ReturnsZero()
    {
        // Arrange
        var tollFreeDate = new DateTime(2023, 12, 25);
        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(tollFreeDate, vehicle);

        // Assert
        Assert.AreEqual(0, tollFee);
    }

    [Test]
    public void GetTollFee_WhenWithinTollFreeTimeRange_ReturnsCorrectFee()
    {
        // Arrange
        var date = new DateTime(2023, 6, 28, 6, 15, 0);
        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(date, vehicle);

        // Assert
        Assert.AreEqual(8, tollFee);
    }

    [TestCase("2023-06-28 06:05", 8.0m)]
    public void GetTollFee_WhenOutsideTollFreeTimeRange_ReturnsCorrectFee(string dateAndTime, )
    {
        // Arrange
        var date = new DateTime(2023, 6, 28, 9, 0, 0);
        var vehicle = new Car();

        // Act
        var tollFee = tollCalculator.GetTollFee(date, vehicle);

        // Assert
        Assert.AreEqual(13, tollFee);
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
        Assert.AreEqual(0, tollFee);
    }
}