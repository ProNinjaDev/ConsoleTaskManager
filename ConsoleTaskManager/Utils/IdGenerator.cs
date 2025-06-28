namespace ConsoleTaskManager.Utils
{
    public static class IdGenerator
    {
        public static int GenerateNextId<T>(IEnumerable<T> items, Func<T, int> idSelector)
        {
            if (items != null && items.Any())
            {
                return items.Max(idSelector) + 1;
            }
            return 1;
        }
    }
} 