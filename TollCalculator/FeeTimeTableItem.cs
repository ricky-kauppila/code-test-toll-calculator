namespace TollCalculator;

public record FeeTimeTableItem(TimeOnly Start, TimeOnly End, decimal Amount);