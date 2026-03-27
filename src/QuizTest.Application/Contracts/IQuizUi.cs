using QuizTest.Domain;

namespace QuizTest.Application.Contracts;

/// <summary>
/// Defines the contract for the quiz user interface.
/// </summary>
public interface IQuizUi
{
    /// <summary>
    /// Displays the welcome screen to the user.
    /// </summary>
    void ShowWelcome();

    /// <summary>
    /// Prompts the user to select a difficulty level.
    /// </summary>
    /// <returns>The selected difficulty level.</returns>
    Difficulty PromptDifficulty();

    /// <summary>
    /// Prompts the user to select the number of questions.
    /// </summary>
    /// <returns>The selected number of questions as an integer.</returns>
    int PromptQuestionCount();

    /// <summary>
    /// Prompts the user to select a quiz category.
    /// </summary>
    /// <param name="categories">The list of available quiz categories.</param>
    /// <returns>The selected category, or null if all categories were selected.</returns>
    QuizCategory? PromptCategory(List<QuizCategory> categories);

    /// <summary>
    /// Displays a loading indicator while asynchronous work executes.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the work.</typeparam>
    /// <param name="message">The status message to display during loading.</param>
    /// <param name="work">The asynchronous work to execute.</param>
    /// <returns>A task that returns the result of the work.</returns>
    Task<T> WithStatusAsync<T>(string message, Func<Task<T>> work);

    /// <summary>
    /// Displays a quiz question and prompts the user to select an answer.
    /// </summary>
    /// <param name="category">The category of the question.</param>
    /// <param name="question">The question text.</param>
    /// <param name="answers">The list of answer options.</param>
    /// <param name="questionNumber">The ordinal number of the current question.</param>
    /// <param name="totalQuestions">The total number of questions in the quiz.</param>
    /// <returns>The user's selected answer.</returns>
    string PromptAnswer(string category, string question, IReadOnlyList<string> answers, int questionNumber, int totalQuestions);

    /// <summary>
    /// Displays feedback on the user's answer.
    /// </summary>
    /// <param name="isCorrect">Whether the user's answer was correct.</param>
    /// <param name="correctAnswer">The correct answer to display if the user was incorrect.</param>
    void ShowAnswerResult(bool isCorrect, string correctAnswer);

    /// <summary>
    /// Displays progress between questions.
    /// </summary>
    /// <param name="answeredCount">The number of questions answered so far.</param>
    /// <param name="totalCount">The total number of questions in the quiz.</param>
    void ShowInterQuestionProgress(int answeredCount, int totalCount);

    /// <summary>
    /// Displays the final quiz results.
    /// </summary>
    /// <param name="correctAnswerCount">The number of correctly answered questions.</param>
    /// <param name="totalQuestions">The total number of questions in the quiz.</param>
    void ShowFinalResults(int correctAnswerCount, int totalQuestions);
}
