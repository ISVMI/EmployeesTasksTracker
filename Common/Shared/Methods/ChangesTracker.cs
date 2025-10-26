namespace Shared.Methods
{
    public static class ChangesTracker
    {
        public static List<string> GetChanges<T>(T OldData, T newData)
            where T : class
        {
            var changes = new List<string>();

            var properites = typeof(T).GetProperties();

            foreach (var prop in properites)
            {
                var oldValue = prop.GetValue(OldData)?.ToString();
                var newValue = prop.GetValue(newData)?.ToString();

                if (oldValue != newValue) 
                {
                    changes.Add($"{prop.Name} изменено с {oldValue} на {newValue}");
                }
            }

            return changes;
        }
    }
}
