using System.Windows.Forms;
using QuizTest.Domain.Quiz;

namespace QuizTest.Ui.Forms;

public class StartForm : Form
{
    private ComboBox cmbDifficulty = new();
    private ComboBox cmbCount = new();
    private ComboBox cmbCategory = new();
    private Button btnOk = new();
    private Button btnCancel = new();

    public Difficulty SelectedDifficulty { get; private set; }
    public int SelectedCount { get; private set; }
    public QuizCategory? SelectedCategory { get; private set; }

    public StartForm(List<QuizCategory> categories)
    {
        this.Text = "Start quiz";
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.ClientSize = new System.Drawing.Size(400, 180);
        this.MaximizeBox = false;

        var lbl1 = new Label { Text = "Difficulty", Left = 10, Top = 10, Width = 100 };
        cmbDifficulty.Left = 120; cmbDifficulty.Top = 10; cmbDifficulty.Width = 250;
        cmbDifficulty.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDifficulty.Items.AddRange(new object[] { Difficulty.Easy, Difficulty.Medium, Difficulty.Hard });
        cmbDifficulty.SelectedIndex = 1;

        var lbl2 = new Label { Text = "Questions", Left = 10, Top = 45, Width = 100 };
        cmbCount.Left = 120; cmbCount.Top = 45; cmbCount.Width = 250;
        cmbCount.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCount.Items.AddRange(new object[] { 5, 10, 15, 20 });
        cmbCount.SelectedIndex = 0;

        var lbl3 = new Label { Text = "Category", Left = 10, Top = 80, Width = 100 };
        cmbCategory.Left = 120; cmbCategory.Top = 80; cmbCategory.Width = 250;
        cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCategory.Items.Add("All categories");
        foreach (var c in categories)
            cmbCategory.Items.Add(System.Net.WebUtility.HtmlDecode(c.Name));
        cmbCategory.SelectedIndex = 0;

        btnOk.Text = "Start"; btnOk.Left = 120; btnOk.Top = 120; btnOk.Width = 120; btnOk.DialogResult = DialogResult.OK;
        btnCancel.Text = "Cancel"; btnCancel.Left = 250; btnCancel.Top = 120; btnCancel.Width = 120; btnCancel.DialogResult = DialogResult.Cancel;

        this.Controls.AddRange(new Control[] { lbl1, cmbDifficulty, lbl2, cmbCount, lbl3, cmbCategory, btnOk, btnCancel });

        this.AcceptButton = btnOk;
        this.CancelButton = btnCancel;

        btnOk.Click += BtnOk_Click;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        SelectedDifficulty = (Difficulty)cmbDifficulty.SelectedItem!;
        SelectedCount = (int)cmbCount.SelectedItem!;
        if (cmbCategory.SelectedIndex <= 0)
            SelectedCategory = null;
        else
            SelectedCategory = new QuizCategory(cmbCategory.SelectedIndex, (string)cmbCategory.SelectedItem!);
    }
}
