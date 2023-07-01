namespace TollCalculator.Tests;

using global::TollCalculator.Helpers;

public class DateAndTimeExtensionsTests
{
    [Test]
    public void IsWithinSpanOf_GivenSameValues_ReturnsTrue()
    {
        // Arrange
        var first = TestDates.TodayAtEightAm;
        var second = TestDates.TodayAtEightAm;

        // Act & Assert
        Assert.That(first.IsWithinSpanOf(second, TimeSpan.Zero));
    }

    [Test]
    public void IsWithinSpanOf_GivenValueInSpan_WhenValueIsBefore_ReturnsTrue()
    {
        // Arrange
        var first = TestDates.TodayAtEightAm;
        var second = TestDates.TodayAtEightAm.AddMinutes(-1);

        // Act & Assert
        Assert.That(first.IsWithinSpanOf(second, TimeSpan.FromMinutes(1)));
    }

    [Test]
    public void IsWithinSpanOf_GivenValueOutsideSpan_WhenValueIsBefore_ReturnsFalse()
    {
        // Arrange
        var first = TestDates.TodayAtEightAm;
        var second = TestDates.TodayAtEightAm.AddMinutes(-1).AddSeconds(-1);

        // Act & Assert
        Assert.That(first.IsWithinSpanOf(second, TimeSpan.FromMinutes(1)), Is.False);
    }

    [Test]
    public void IsWithinSpanOf_GivenValueInSpan_WhenValueIsAfter_ReturnsTrue()
    {
        // Arrange
        var first = TestDates.TodayAtEightAm;
        var second = TestDates.TodayAtEightAm.AddSeconds(59);

        // Act & Assert
        Assert.That(first.IsWithinSpanOf(second, TimeSpan.FromMinutes(1)));
    }

    [Test]
    public void IsWithinSpanOf_GivenValueOutsideSpan_WhenValueIsAfter_ReturnsFalse()
    {
        // Arrange
        var first = TestDates.TodayAtEightAm;
        var second = TestDates.TodayAtEightAm.AddMinutes(1);

        // Act & Assert
        Assert.That(first.IsWithinSpanOf(second, TimeSpan.FromMinutes(1)), Is.False);
    }
}