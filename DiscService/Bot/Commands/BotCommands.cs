namespace DiscService.Bot.Commands;

/// <summary>
/// Содержит константы для команд сервиса DISC-теста
/// </summary>
public static class BotCommands
{
    /// <summary>
    /// Команда для приглашения к прохождению DISC-теста
    /// </summary>
    public const string StartTestCommand = "/start_disc";

    /// <summary>
    /// Callback-команда для начала DISC-теста
    /// </summary>
    public const string BeginTestCallback = "begin_disc";

    /// <summary>
    /// Команда для отмены текущего DISC-теста
    /// </summary>
    public const string CancelTestCommand = "/cancel_disc";

    /// <summary>
    /// Команда для получения последнего результата DISC-теста
    /// </summary>
    public const string LastResultCommand = "/disc_result";

    /// <summary>
    /// Команда для сравнения текущего и предыдущего результатов DISC-теста
    /// </summary>
    public const string CompareResultsCommand = "/compare_disc_results";

    /// <summary>
    /// Callback-команда для сравнения текущего и предыдущего результатов DISC-теста
    /// </summary>
    public const string CompareResultsCallback = "compare_disc_results";

    /// <summary>
    /// Команда для получения информации о психотипах DISC
    /// </summary>
    public const string GetInfoCommand = "/disc_info";

    /// <summary>
    /// Callback-команда для получения информации о психотипах DISC
    /// </summary>
    public const string GetInfoCallback = "disc_info";

    /// <summary>
    /// Префикс для callback-команд для ответов на вопросы DISC-теста
    /// </summary>
    public const string AnswerPrefix = "disc_answer_";

    /// <summary>
    /// Callback-команда для ответа "А" на вопрос DISC-теста
    /// </summary>
    public const string AnswerA = "disc_answer_A";

    /// <summary>
    /// Callback-команда для ответа "Б" на вопрос DISC-теста
    /// </summary>
    public const string AnswerB = "disc_answer_B";

    /// <summary>
    /// Callback-команда для ответа "В" на вопрос DISC-теста
    /// </summary>
    public const string AnswerC = "disc_answer_C";

    /// <summary>
    /// Callback-команда для ответа "Г" на вопрос DISC-теста
    /// </summary>
    public const string AnswerD = "disc_answer_D";
}