using System;

namespace Hangman.Services
{
    public class ThemeService
    {
        private bool _isDarkMode;
        public bool IsDarkMode 
        { 
            get => _isDarkMode;
            private set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnThemeChange?.Invoke();
                }
            }
        }

        public event Action? OnThemeChange;

        public void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }
    }
}
