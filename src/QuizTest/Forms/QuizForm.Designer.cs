namespace QuizTest.Ui.Forms;

partial class QuizForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.lblQuestion = new System.Windows.Forms.Label();
        this.btnA = new System.Windows.Forms.Button();
        this.btnB = new System.Windows.Forms.Button();
        this.btnC = new System.Windows.Forms.Button();
        this.btnD = new System.Windows.Forms.Button();
        this.btnNext = new System.Windows.Forms.Button();
        this.lblProgress = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // lblQuestion
        // 
        this.lblQuestion.Location = new System.Drawing.Point(12, 9);
        this.lblQuestion.Name = "lblQuestion";
        this.lblQuestion.Size = new System.Drawing.Size(560, 80);
        this.lblQuestion.TabIndex = 0;
        this.lblQuestion.Text = "Question text";
        // 
        // btnA
        // 
        this.btnA.Location = new System.Drawing.Point(12, 100);
        this.btnA.Name = "btnA";
        this.btnA.Size = new System.Drawing.Size(270, 40);
        this.btnA.TabIndex = 1;
        this.btnA.Text = "A";
        this.btnA.UseVisualStyleBackColor = true;
        this.btnA.Click += new System.EventHandler(this.AnswerButton_Click);
        // 
        // btnB
        // 
        this.btnB.Location = new System.Drawing.Point(302, 100);
        this.btnB.Name = "btnB";
        this.btnB.Size = new System.Drawing.Size(270, 40);
        this.btnB.TabIndex = 2;
        this.btnB.Text = "B";
        this.btnB.UseVisualStyleBackColor = true;
        this.btnB.Click += new System.EventHandler(this.AnswerButton_Click);
        // 
        // btnC
        // 
        this.btnC.Location = new System.Drawing.Point(12, 150);
        this.btnC.Name = "btnC";
        this.btnC.Size = new System.Drawing.Size(270, 40);
        this.btnC.TabIndex = 3;
        this.btnC.Text = "C";
        this.btnC.UseVisualStyleBackColor = true;
        this.btnC.Click += new System.EventHandler(this.AnswerButton_Click);
        // 
        // btnD
        // 
        this.btnD.Location = new System.Drawing.Point(302, 150);
        this.btnD.Name = "btnD";
        this.btnD.Size = new System.Drawing.Size(270, 40);
        this.btnD.TabIndex = 4;
        this.btnD.Text = "D";
        this.btnD.UseVisualStyleBackColor = true;
        this.btnD.Click += new System.EventHandler(this.AnswerButton_Click);
        // 
        // btnNext
        // 
        this.btnNext.Location = new System.Drawing.Point(12, 210);
        this.btnNext.Name = "btnNext";
        this.btnNext.Size = new System.Drawing.Size(560, 35);
        this.btnNext.TabIndex = 5;
        this.btnNext.Text = "Next question";
        this.btnNext.UseVisualStyleBackColor = true;
        this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
        // 
        // lblProgress
        // 
        this.lblProgress.Location = new System.Drawing.Point(12, 255);
        this.lblProgress.Name = "lblProgress";
        this.lblProgress.Size = new System.Drawing.Size(560, 23);
        this.lblProgress.TabIndex = 6;
        this.lblProgress.Text = "Progress";
        // 
        // QuizForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(584, 291);
        this.Controls.Add(this.lblProgress);
        this.Controls.Add(this.btnNext);
        this.Controls.Add(this.btnD);
        this.Controls.Add(this.btnC);
        this.Controls.Add(this.btnB);
        this.Controls.Add(this.btnA);
        this.Controls.Add(this.lblQuestion);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "QuizForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Quiz";
        this.Load += new System.EventHandler(this.QuizForm_Load);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblQuestion;
    private System.Windows.Forms.Button btnA;
    private System.Windows.Forms.Button btnB;
    private System.Windows.Forms.Button btnC;
    private System.Windows.Forms.Button btnD;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Label lblProgress;
}
