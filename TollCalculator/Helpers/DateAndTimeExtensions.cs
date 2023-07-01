namespace TollCalculator.Helpers;

public static class DateAndTimeExtensions
{
    public static bool IsWithinSpanOf(this DateTime first, DateTime second, TimeSpan timeSpan) =>
        first == second ||
        (first.Add(timeSpan) > second && first.Add(-timeSpan) <= second);
}