using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Hangman.Services;

namespace Hangman.Components
{
    public class GameState
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GameState> _logger;
        private readonly WordCategoryService _wordCategoryService;
        
        public string CurrentWord { get; private set; } = string.Empty;
        public HashSet<char> GuessedLetters { get; private set; }
        public int RemainingAttempts { get; private set; }
        public bool GameOver => RemainingAttempts <= 0 || IsWordGuessed;
        public bool IsWordGuessed => !string.IsNullOrEmpty(CurrentWord) && CurrentWord.All(c => GuessedLetters.Contains(c));
        public WordCategory CurrentCategory { get; private set; } = WordCategory.CommonWords;

        private class DatamuseWord
        {
            public string Word { get; set; } = string.Empty;
            public int Score { get; set; }
        }

        public GameState(HttpClient httpClient, ILogger<GameState> logger, WordCategoryService wordCategoryService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _wordCategoryService = wordCategoryService ?? throw new ArgumentNullException(nameof(wordCategoryService));
            GuessedLetters = new HashSet<char>();
            RemainingAttempts = 6;
        }

        public async Task StartNewGameAsync(WordCategory category = WordCategory.CommonWords)
        {
            CurrentCategory = category;
            try
            {
                _logger.LogInformation("Starting new game with category: {Category}", category);
                
                if (category == WordCategory.CommonWords)
                {
                    // Use Datamuse API for common words
                    var queryParams = "?ml=common&max=100&md=f"; // Get frequent common words
                    var response = await _httpClient.GetStringAsync($"{WordCategoryService.DatamuseApiUrl}{queryParams}");
                    var words = JsonSerializer.Deserialize<DatamuseWord[]>(response);
                    
                    if (words != null && words.Length > 0)
                    {
                        // Filter words to get appropriate length (4-8 letters) and pick random
                        var validWords = words.Where(w => w.Word.Length >= 4 && w.Word.Length <= 8 
                                                     && !w.Word.Contains("-") 
                                                     && !w.Word.Contains(" "))
                                            .ToList();
                        
                        if (validWords.Any())
                        {
                            CurrentWord = validWords[Random.Shared.Next(validWords.Count)].Word.ToUpper();
                            _logger.LogInformation("Retrieved word from Datamuse API successfully");
                        }
                        else
                        {
                            CurrentWord = GetFallbackWord();
                            _logger.LogWarning("No suitable words from API, using fallback");
                        }
                    }
                    else
                    {
                        CurrentWord = GetFallbackWord();
                        _logger.LogWarning("Failed to get word from API, using fallback");
                    }
                }
                else
                {
                    // Use category-specific word
                    CurrentWord = _wordCategoryService.GetRandomWordFromCategory(category);
                    _logger.LogInformation("Using word from category: {Category}", category);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching word");
                CurrentWord = GetFallbackWord();
            }
            
            GuessedLetters.Clear();
            RemainingAttempts = 6;
            _logger.LogInformation("New game started with word length: {Length}", CurrentWord.Length);
        }

        private string GetFallbackWord()
        {
            var fallbackWords = new[] { "BLAZOR", "CODING", "PUZZLE", "PLAYER", "SYSTEM" };
            return fallbackWords[Random.Shared.Next(fallbackWords.Length)];
        }

        public bool MakeGuess(char letter)
        {
            letter = char.ToUpper(letter);
            
            if (GuessedLetters.Contains(letter) || GameOver)
            {
                _logger.LogInformation("Invalid guess: {Letter}", letter);
                return false;
            }

            GuessedLetters.Add(letter);
            
            if (!CurrentWord.Contains(letter))
            {
                RemainingAttempts--;
                _logger.LogInformation("Incorrect guess: {Letter}, Remaining attempts: {Attempts}", letter, RemainingAttempts);
            }
            else
            {
                _logger.LogInformation("Correct guess: {Letter}", letter);
            }
                
            return true;
        }

        public (bool isCorrect, bool isGameOver) GuessFullWord(string word)
        {
            if (string.IsNullOrEmpty(word) || GameOver)
            {
                return (false, GameOver);
            }

            word = word.ToUpper();
            bool isCorrect = word == CurrentWord;

            if (!isCorrect)
            {
                RemainingAttempts--;
                _logger.LogInformation("Incorrect word guess: {Word}, Remaining attempts: {Attempts}", word, RemainingAttempts);
            }
            else
            {
                // Add all letters to guessed letters to show the word
                foreach (char c in CurrentWord)
                {
                    GuessedLetters.Add(c);
                }
                _logger.LogInformation("Correct word guess!");
            }

            return (isCorrect, RemainingAttempts <= 0 || isCorrect);
        }

        public void Reset()
    {
        CurrentWord = string.Empty;
        GuessedLetters.Clear();
        RemainingAttempts = 6;
    }

    public string GetMaskedWord()
        {
            if (string.IsNullOrEmpty(CurrentWord))
                return string.Empty;
                
            return string.Join(" ", CurrentWord.Select(c => GuessedLetters.Contains(c) ? c.ToString() : "_"));
        }
    }
}

