namespace Shared.Methods
{
    public static class EnumsHumanizer
    {
        public static string Translate(string word)
        {
            switch (word)
            {
                case "Low":
                    {
                        return "Низкий";
                    }
                case "Medium":
                    {
                        return "Средний";
                    }
                case "High":
                    {
                        return "Высокий";
                    }
                case "Critical":
                    {
                        return "Критический";
                    }
                case "Blocker":
                    {
                        return "Блокер";
                    }
                case "Backlog":
                    {
                        return "Бэклог";
                    }
                case "Current":
                    {
                        return "Текущая";
                    }
                case "Active":
                    {
                        return "Активная";
                    }
                case "Testing":
                    {
                        return "Тестируется";
                    }
                case "Completed":
                    {
                        return "Завершена";
                    }
                case "Canceled":
                    {
                        return "Отменена";
                    }
                default:
                    {
                        return "Неизвестно";
                    }
            }
        }
    }
}
