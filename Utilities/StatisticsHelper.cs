namespace CSTracker.Utilities
{
    public static class StatisticsHelper
    {
        public static decimal CalculateMedian(decimal[] numbers)
        {
            if (numbers == null || numbers.Length == 0)
                throw new ArgumentException("The list cannot be empty.");

            var sortedNumbers = numbers.OrderBy(x => x).ToList();
            int count = sortedNumbers.Count;
            int mid = count / 2;

            return (count % 2 != 0)
                ? sortedNumbers[mid]
                : (sortedNumbers[mid - 1] + sortedNumbers[mid]) / 2;
        }
    }
}