using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Ensure this namespace matches your project folder structure if needed.
// If your project file is Hangman.csproj, the namespace might just be 'Hangman.Components'
// Adjust if necessary for consistency with your mono-repo naming later.
namespace Hangman.Components // Adjust this namespace if needed
{
    public partial class HangmanGame : ComponentBase
    {
        private List<string> wordList = new List<string> {
            "BLAZOR", "COMPONENT", "DEVELOPER", "FRAMEWORK", "MICROSOFT",
            "MONOREPO", "PROJECT", "INTERACTIVE", "WEBSITE", "CLASS", "NORWAY", "SANDEFJORD"
        };
        private string selectedWord = "";
        protected StringBuilder maskedWord = new StringBuilder();
        protected string maskedWordDisplay => maskedWord.ToString();

        protected HashSet<char> guessedLetters = new HashSet<char>();
        protected int incorrectGuesses = 0;
        protected int maxIncorrectGuesses = 6;
        protected bool isGameOver = false;
        protected bool isWin = false;
        protected string gameStatusMessage = "";

        private Random random = new Random();

        protected override void OnInitialized()
        {
            StartNewGame();
        }

        protected void StartNewGame()
        {
            selectedWord = wordList[random.Next(wordList.Count)].ToUpper();
            guessedLetters.Clear();
            incorrectGuesses = 0;
            isGameOver = false;
            isWin = false;
            gameStatusMessage = "";
            UpdateMaskedWord();
            StateHasChanged();
        }

        protected void HandleGuess(char letter)
        {
            if (isGameOver || guessedLetters.Contains(letter))
            {
                return;
            }

            guessedLetters.Add(letter);

            if (selectedWord.Contains(letter))
            {
                UpdateMaskedWord();
                CheckWinCondition();
            }
            else
            {
                incorrectGuesses++;
                CheckLossCondition();
            }
        }

        private void UpdateMaskedWord()
        {
            maskedWord.Clear();
            foreach (char c in selectedWord)
            {
                if (guessedLetters.Contains(c) || !char.IsLetter(c))
                {
                    maskedWord.Append(c);
                }
                else
                {
                    maskedWord.Append('_');
                }
                maskedWord.Append(' ');
            }
            if (maskedWord.Length > 0) maskedWord.Length--;
        }

        private void CheckWinCondition()
        {
            bool wordGuessed = !maskedWord.ToString().Replace(" ", "").Contains('_');
            if (wordGuessed)
            {
                isGameOver = true;
                isWin = true;
                gameStatusMessage = $"You guessed it! The word was: {selectedWord}";
            }
        }


        private void CheckLossCondition()
        {
            if (incorrectGuesses >= maxIncorrectGuesses)
            {
                isGameOver = true;
                isWin = false;
                gameStatusMessage = $"Game Over! The word was: {selectedWord}";
                maskedWord.Clear();
                foreach (char c in selectedWord) { maskedWord.Append(c).Append(' '); }
                if (maskedWord.Length > 0) maskedWord.Length--;
            }
        }
    }
}