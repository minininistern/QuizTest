using System;
using System.Linq;
using System.Windows.Forms;
using QuizTest.Application.Contracts;
using QuizTest.Domain.Quiz;

namespace QuizTest.Ui.Forms;

public partial class QuizForm : Form
{
    private readonly IQuizApiClient _apiClient;
    private readonly IAnswerShuffler _shuffler;
    private readonly IQuizUi _consoleUi; // keep console UI for prompts not yet implemented in WinForms

    private List<QuizQuestion> _questions = new();
    private int _currentIndex = -1;
    private int _correctCount = 0;

    public QuizForm(IQuizApiClient apiClient, IAnswerShuffler shuffler, IQuizUi consoleUi)
    {
        _apiClient = apiClient;
        _shuffler = shuffler;
        _consoleUi = consoleUi;

        InitializeComponent();
    }

    private async void QuizForm_Load(object sender, EventArgs e)
    {
        // Fetch categories from API
        List<QuizCategory> categories;
        try
        {
            categories = await _apiClient.GetCategoriesAsync();
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show($"Failed to fetch categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        // Show start dialog to choose difficulty, count and category
        using var start = new StartForm(categories);
        if (start.ShowDialog(this) != DialogResult.OK)
        {
            this.Close();
            return;
        }

        var difficulty = start.SelectedDifficulty;
        var count = start.SelectedCount;
        var selectedCategory = start.SelectedCategory;

        try
        {
            _questions = await _apiClient.GetQuestionsAsync(count, difficulty, selectedCategory?.Id);
        }
        catch (Exception ex)
        {
            System.Windows.Forms.MessageBox.Show($"Failed to fetch questions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.Close();
            return;
        }

        _currentIndex = -1;
        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
        _currentIndex++;
        if (_currentIndex >= _questions.Count)
        {
            System.Windows.Forms.MessageBox.Show($"Quiz finished. Correct: {_correctCount} of {_questions.Count}");
            this.Close();
            return;
        }

        var q = _questions[_currentIndex];
        lblQuestion.Text = System.Net.WebUtility.HtmlDecode(q.Question);
        var answers = _shuffler.ShuffleAnswers(q).Select(a => System.Net.WebUtility.HtmlDecode(a)).ToArray();

        // assign up to 4 buttons
        var buttons = new[] { btnA, btnB, btnC, btnD };
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < answers.Length)
            {
                buttons[i].Text = answers[i];
                buttons[i].Visible = true;
                buttons[i].Enabled = true;
            }
            else
            {
                buttons[i].Visible = false;
            }
        }

        lblProgress.Text = $"Question {_currentIndex + 1} / {_questions.Count}";
    }

    private void AnswerButton_Click(object? sender, EventArgs e)
    {
        if (sender is not System.Windows.Forms.Button btn) return;
        var selected = btn.Text;
        var q = _questions[_currentIndex];
        var correct = System.Net.WebUtility.HtmlDecode(q.CorrectAnswer);
        if (string.Equals(selected, correct, StringComparison.Ordinal))
        {
            _correctCount++;
            System.Windows.Forms.MessageBox.Show("Correct!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            System.Windows.Forms.MessageBox.Show($"Wrong. Correct: {correct}", "Result", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        ShowNextQuestion();
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        ShowNextQuestion();
    }
}
