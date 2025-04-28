using System;
using System.Collections.Generic;

namespace Hangman.Services
{
    public enum WordCategory
    {
        CommonWords,
        Animals,
        Countries,
        FoodAndDrinks,
        Sports,
        Technology
    }

    public class WordCategoryService
    {
        private readonly Dictionary<WordCategory, List<string>> _categoryWords = new()
        {
            [WordCategory.Animals] = new List<string> 
            { 
                "ELEPHANT", "GIRAFFE", "PENGUIN", "DOLPHIN", "KANGAROO",
                "LEOPARD", "OCTOPUS", "ZEBRA", "LION", "TIGER"
            },
            [WordCategory.Countries] = new List<string> 
            { 
                "FRANCE", "JAPAN", "BRAZIL", "CANADA", "SWEDEN",
                "ITALY", "SPAIN", "GERMANY", "MEXICO", "EGYPT"
            },
            [WordCategory.FoodAndDrinks] = new List<string> 
            { 
                "PIZZA", "BURGER", "SUSHI", "PASTA", "COFFEE",
                "COOKIE", "SALAD", "SANDWICH", "CHICKEN", "JUICE"
            },
            [WordCategory.Sports] = new List<string> 
            { 
                "SOCCER", "TENNIS", "HOCKEY", "BOXING", "SKIING",
                "SWIMMING", "BASEBALL", "RUGBY", "GOLF", "VOLLEYBALL"
            },
            [WordCategory.Technology] = new List<string> 
            { 
                "LAPTOP", "PHONE", "TABLET", "ROUTER", "SERVER",
                "KEYBOARD", "MOUSE", "MONITOR", "PRINTER", "CAMERA"
            }
        };

        public const string DatamuseApiUrl = "https://api.datamuse.com/words";

        public string GetRandomWordFromCategory(WordCategory category)
        {
            if (_categoryWords.TryGetValue(category, out var words))
            {
                return words[Random.Shared.Next(words.Count)];
            }
            return string.Empty;
        }

        public string GetCategoryName(WordCategory category)
        {
            return category switch
            {
                WordCategory.CommonWords => "Common Words",
                WordCategory.Animals => "Animals",
                WordCategory.Countries => "Countries",
                WordCategory.FoodAndDrinks => "Food & Drinks",
                WordCategory.Sports => "Sports",
                WordCategory.Technology => "Technology",
                _ => "Unknown Category"
            };
        }
    }
}
