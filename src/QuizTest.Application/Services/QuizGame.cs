using QuizTest.Application.Contracts;
using System.Text.Json;

namespace QuizTest.Application.Services;

public sealed class QuizGame(IQuizApiClient apiClient, IQuizUi ui, IAnswerShuffler answerShuffler)
{
    private readonly IQuizApiClient _apiClient = apiClient;
    private readonly IQuizUi _ui = ui;
    private readonly IAnswerShuffler _answerShuffler = answerShuffler;

    /// <summary>
    /// Runs an interactive quiz game by fetching categories and questions from the API, 
    /// prompting the user to answer questions, and displaying final results.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method orchestrates the entire quiz flow:
    /// 1. Displays a welcome screen
    /// 2. Prompts the user to select difficulty and question count
    /// 3. Fetches available quiz categories from the API
    /// 4. Prompts the user to select a category
    /// 5. Fetches the selected quiz questions from the API
    /// 6. Displays each question and collects the user's answer
    /// 7. Provides immediate feedback on answer correctness
    /// 8. Shows final results including score and percentage
    /// </remarks>
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        _ui.ShowWelcome();

        var difficulty = _ui.PromptDifficulty();
        var questionCount = _ui.PromptQuestionCount();

        var categories = await _ui.WithStatusAsync(
            "Fetching categories...",
            () => _apiClient.GetCategoriesAsync(cancellationToken));

        var selectedCategory = _ui.PromptCategory(categories);

        var questions = await _ui.WithStatusAsync(
            $"Fetching {questionCount} questions from API...",
            () => _apiClient.GetQuestionsAsync(questionCount, difficulty, selectedCategory?.Id, cancellationToken));

        var correctAnswerCount = 0;

        // Prepare data directory and files for logging
        var dataDir = Path.Combine(AppContext.BaseDirectory, "quiz_data");
        Directory.CreateDirectory(dataDir);
        var questionLogPath = Path.Combine(dataDir, "question_log.jsonl");
        var lastQuestionPath = Path.Combine(dataDir, "last_question.json");
        var resultsLogPath = Path.Combine(dataDir, "results_log.jsonl");

        var jsonOptions = new JsonSerializerOptions { WriteIndented = false };

        // Save the last fetched questions batch (without answers) for reference
        try
        {
            var batchInfo = new
            {
                FetchedAt = DateTime.UtcNow,
                Difficulty = difficulty,
                Category = selectedCategory?.Id,
                Questions = questions.Select(q => new { q.Question, q.Category, q.Difficulty }).ToList()
            };

            File.WriteAllText(lastQuestionPath, JsonSerializer.Serialize(batchInfo, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
            // Ignore file errors to avoid crashing the quiz
        }

        for (var i = 0; i < questions.Count; i++)
        {
            var question = questions[i];
            var answers = _answerShuffler.ShuffleAnswers(question);
            var decodedAnswers = answers.Select(a => System.Net.WebUtility.HtmlDecode(a) ?? string.Empty).ToArray();
            var decodedCorrectAnswer = System.Net.WebUtility.HtmlDecode(question.CorrectAnswer) ?? string.Empty;

            var selectedAnswer = _ui.PromptAnswer(
                System.Net.WebUtility.HtmlDecode(question.Category),
                System.Net.WebUtility.HtmlDecode(question.Question),
                decodedAnswers,
                i + 1,
                questions.Count);

            var isCorrect = string.Equals(selectedAnswer, decodedCorrectAnswer, StringComparison.Ordinal);
            if (isCorrect)
                correctAnswerCount++;
 
            _ui.ShowAnswerResult(isCorrect, decodedCorrectAnswer);

            // Log this answered question (append as JSON line)
            try
            {
                var logEntry = new
                {
                    Timestamp = DateTime.UtcNow,
                    Question = System.Net.WebUtility.HtmlDecode(question.Question),
                    Category = System.Net.WebUtility.HtmlDecode(question.Category),
                    Difficulty = question.Difficulty,
                    Answers = decodedAnswers,
                    CorrectAnswer = decodedCorrectAnswer,
                    SelectedAnswer = selectedAnswer,
                    IsCorrect = isCorrect
                };

                var line = JsonSerializer.Serialize(logEntry, jsonOptions) + Environment.NewLine;
                File.AppendAllText(questionLogPath, line);
            }
            catch
            {
                // Swallow file IO exceptions to keep the game running
            }

            if (i < questions.Count - 1)
                _ui.ShowInterQuestionProgress(i + 1, questions.Count);
        }

        _ui.ShowFinalResults(correctAnswerCount, questions.Count);

        // Append a result summary to results log
        try
        {
            var resultEntry = new
            {
                Timestamp = DateTime.UtcNow,
                Total = questions.Count,
                Correct = correctAnswerCount,
                Percentage = questions.Count > 0 ? (double)correctAnswerCount / questions.Count * 100 : 0,
                Difficulty = difficulty,
                Category = selectedCategory?.Id
            };

            var line = JsonSerializer.Serialize(resultEntry, jsonOptions) + Environment.NewLine;
            File.AppendAllText(resultsLogPath, line);
        }
        catch
        {
            // Ignore
        }
    }
}
