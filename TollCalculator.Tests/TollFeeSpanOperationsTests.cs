namespace TollCalculator.Tests;

using global::TollCalculator.Helpers;

public class TollFeeSpanOperationsTests
{
    [Test]
    public void KeepOnlyHighestFee_GivenSingleFee_KeepsItAsIs()
    {
        // Arrange
        var fees = new TollFee[]
        {
            new(new Pass(DateTime.Parse("2023-07-01 08:00"), new Car()), 10)
        };

        // Act
        TollFeeSpanOperations.KeepOnlyHighestFee(fees);

        // Assert
        Assert.That(fees[0].Amount, Is.EqualTo(10));
    }

    [Test]
    public void KeepOnlyHighestFee_MultipleDifferentFees_KeepsHighestOnly()
    {
        // Arrange

        var fees = new TollFee[]
        {
            new(new Pass(TestDates.TodayAtEightAm, new Car()), 11),
            new(new Pass(TestDates.TodayAtEightAm.AddMinutes(1), new Car()), 10),
            new(new Pass(TestDates.TodayAtEightAm.AddMinutes(2), new Car()), 12)
        };

        // Act
        TollFeeSpanOperations.KeepOnlyHighestFee(fees);

        // Assert
        Assert.That(fees[0].Amount, Is.EqualTo(0));
        Assert.That(fees[1].Amount, Is.EqualTo(0));
        Assert.That(fees[2].Amount, Is.EqualTo(12));
    }

    [Test]
    public void KeepOnlyHighestFee_MultipleSameFees_KeepsOnlyOne()
    {
        // Arrange
        var fees = new TollFee[]
        {
            new(new Pass(TestDates.TodayAtEightAm, new Car()), 10),
            new(new Pass(TestDates.TodayAtEightAm.AddMinutes(1), new Car()), 10),
            new(new Pass(TestDates.TodayAtEightAm.AddMinutes(2), new Car()), 10)
        };

        // Act
        TollFeeSpanOperations.KeepOnlyHighestFee(fees);

        // Assert
        Assert.That(fees[0].Amount, Is.EqualTo(10));
        Assert.That(fees[1].Amount, Is.EqualTo(0));
        Assert.That(fees[2].Amount, Is.EqualTo(0));
    }

    [Test]
    public void IterateTimeSpans_GivenTenMinutesTimeSpan_IteratesOverTenMinutesTollFeesSpan()
    {
        // Arrange
        var firstSpanStart = TestDates.TodayAtEightAm;
        var firstSpanEnd = firstSpanStart.AddMinutes(9).AddSeconds(59);

        var secondSpanStart = firstSpanStart.AddMinutes(10);
        var secondSpanEnd = firstSpanStart.AddMinutes(9).AddSeconds(59);

        TollFee TollFee(DateTime passDateTime) => new(new Pass(passDateTime, new Car()), 10);

        var fees = new[]
        {
            TollFee(firstSpanStart),
            TollFee(firstSpanStart.AddMinutes(1)),
            TollFee(firstSpanEnd),

            TollFee(secondSpanStart),
            TollFee(secondSpanStart.AddMinutes(1)),
            TollFee(secondSpanEnd),
        };

        var expectedIterations = new List<TimeSpanIteration>
        {
            new(firstSpanStart, firstSpanEnd, 3),
            new(secondSpanStart, secondSpanEnd, 3)
        };

        var iterations = new List<TimeSpanIteration>();

        // Act
        TollFeeSpanOperations.IterateTimeSpans(fees, TimeSpan.FromMinutes(10), span =>
        {
            var iteration = new TimeSpanIteration(
                span[0].Pass.DateTime,
                span[^1].Pass.DateTime,
                span.Length);
            iterations.Add(iteration);

            TestContext.WriteLine($"Span {iteration.Start} > {iteration.End}, Count {iteration.Count}");
        });

        // Assert
        CollectionAssert.AreEqual(expectedIterations, iterations);
    }

    private record TimeSpanIteration(DateTime Start, DateTime End, int Count);
}