namespace ShopService.Conventions
{
    /// <summary>
    /// Результат выполнения команды
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// Результат выполнения команды
        /// </summary>
        /// <param name="isDone">Успешность выполнения команды</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <param name="addition">Доп информация (id созданной сущности, ...)</param>
        public CommandResult(bool isDone, string message = "", object addition = null)
        {
            IsDone = isDone;
            Message = !isDone && string.IsNullOrWhiteSpace(message) ? "Произошла ошибка" : message;
            Addition = addition;
        }

        /// <summary>
        /// Успешность выполнения команды
        /// true - успешно
        /// false - произошла ошибка и команда не выполнена
        /// </summary>
        public bool IsDone { get; set; }
        
        /// <summary>
        /// Сообщение об ошибках, если они есть
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Доп. информация из команды
        /// </summary>
        public object Addition { get; set; }

        public static CommandResult Success = new CommandResult(true);
        public static CommandResult Failure = new CommandResult(false);
    }
}