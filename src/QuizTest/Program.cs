using QuizTest.DependencyInjection;
using QuizTest.Application.Contracts;
using QuizTest.Application.Services;
using QuizTest.Ui.Services;
using QuizTest.Ui.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Forms;

var services = new ServiceCollection();

services.AddQuizCore();
services.AddSingleton<IQuizUi, SpectreQuizUi>();
services.AddSingleton<QuizForm>();

using var serviceProvider = services.BuildServiceProvider();
Application.SetHighDpiMode(HighDpiMode.SystemAware);
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

var quizForm = serviceProvider.GetRequiredService<QuizForm>();
Application.Run(quizForm);
