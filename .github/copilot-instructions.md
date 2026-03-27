# Copilot Instructions

## Build & Run

```bash
dotnet run --project src/QuizTest       # Run the app
dotnet build                            # Build solution
dotnet test                             # Run all tests
dotnet test --filter "FullyQualifiedName~QuizGameTests.RunAsync_TracksScore"  # Single test
```

## Architecture

Clean Architecture across 5 projects, strict one-way dependency flow:

```
QuizTest (UI / Composition Root)
    ↓ references
QuizTest.Application  +  QuizTest.Infrastructure
    ↓ depends on
QuizTest.Domain
```

| Project | Contents | Key rule |
|---|---|---|
| `QuizTest.Domain` | `QuizQuestion`, `QuizCategory` records | Zero external dependencies |
| `QuizTest.Application` | Contracts (`IQuizApiClient`, `IQuizUi`, `IAnswerShuffler`) + Services (`QuizGame`, `RandomAnswerShuffler`) | No framework packages |
| `QuizTest.Infrastructure` | `QuizApiClient` (implements `IQuizApiClient`), OpenTrivia DTOs | Adapts the external API |
| `QuizTest.Core` | Empty placeholder | Reserved for future orchestration |
| `QuizTest` | `Program.cs`, `SpectreQuizUi`, `AddQuizCore()` extension | Only layer with DI + Spectre.Console |

## Key Conventions

**Domain models are immutable C# records:**
```csharp
public record QuizQuestion(string Type, string Difficulty, string Category,
    string Question, string CorrectAnswer, List<string> IncorrectAnswers);
```
API data arrives HTML-encoded; `System.Net.WebUtility.HtmlDecode()` is called in `QuizGame.RunAsync()`, not in the API client or domain.

**Contracts live in `Application/Contracts/`, implementations elsewhere:**
- `IQuizUi` is implemented by `SpectreQuizUi` in the UI project (namespace `QuizTest.Ui.Services`)
- `IQuizApiClient` is implemented by `QuizApiClient` in Infrastructure
- `IAnswerShuffler` is implemented by `RandomAnswerShuffler` in Application

**`IQuizUi.FetchCategoriesAsync` / `FetchQuestionsAsync` take a `Func<Task<T>>`** so the UI layer can wrap API calls in a loading spinner. `QuizGame` passes lambdas; it does not call the API client directly for these.

**DI lifetimes** (registered in `AddQuizCore()`):
- `IQuizApiClient` → `Singleton`
- `IAnswerShuffler` → `Singleton`
- `QuizGame` → `Transient`
- `IQuizUi` → `Singleton` (registered in `Program.cs`, outside `AddQuizCore()`)

**Infrastructure DTOs** use `[JsonPropertyName]` attributes and `record` types in `QuizTest.Infrastructure.Integrations.OpenTrivia`.

**All production service classes are `sealed`.**

**Primary constructor syntax** is used for dependency injection (C# 12):
```csharp
public sealed class QuizGame(IQuizApiClient apiClient, IQuizUi ui, IAnswerShuffler answerShuffler)
```

## Tests

`QuizTest.Tests` references only `QuizTest.Application` (not Infrastructure or UI) — tests mock all three interfaces with **Moq** and exercise `QuizGame` directly.

Test helper pattern — use a private factory method for domain objects:
```csharp
private static QuizQuestion CreateQuestion(string questionText, string correctAnswer, List<string> incorrectAnswers)
```

`xunit` is globally imported via `<Using Include="Xunit" />` in the test `.csproj` — no `using Xunit;` needed in test files.

## Tech Stack

- **.NET 10**, `Nullable` and `ImplicitUsings` enabled on all projects
- **Spectre.Console** for all terminal rendering (panels, tables, selection prompts, spinners)
- **Microsoft.Extensions.DependencyInjection** (UI project only)
- **xUnit + Moq + Coverlet** in tests
