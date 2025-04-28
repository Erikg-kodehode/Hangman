# Hangman Game

A classic Hangman word guessing game implemented as a Blazor WebAssembly application. Play directly in your browser with no server-side code required!

## Live Demo

Play the game directly in your browser: [Hangman Game on GitHub Pages](https://erikg-kodehode.github.io/Hangman/)

## Technologies Used

- **Blazor WebAssembly** - Client-side web framework using C# instead of JavaScript
- **.NET 8** - Latest .NET framework
- **Bootstrap 5** - For responsive UI components and styling
- **HTML5/CSS3** - For structure and custom styling
- **GitHub Pages** - For hosting the static web application

## Features

- Classic Hangman gameplay with visual gallows representation
- Multiple word categories to choose from
- Clean, responsive UI that works on both desktop and mobile devices
- Dark/light theme switcher
- Interactive keyboard interface for letter selection
- Visual feedback for correct and incorrect guesses
- Game state tracking (wins, losses, remaining attempts)
- No server-side code required - runs entirely in browser

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Running Locally

1. Clone the repository
   ```
   git clone https://github.com/Erikg-kodehode/Hangman.git
   cd Hangman
   ```

2. Build and run the application
   ```
   dotnet run
   ```

3. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

### Building for Production

```
dotnet publish -c Release
```

The published files will be in the `bin/Release/net8.0/publish/wwwroot` directory.

## Project Structure

- **Components/** - Reusable UI components
  - **Gallows.razor** - Visual representation of the hangman gallows
  - **HangmanGame.razor** - Main game component
  - **ThemeSwitch.razor** - Theme switcher component

- **Layout/** - Layout components including MainLayout

- **Pages/** - Application pages
  - **Index.razor** - Main landing page

- **Services/** - Service classes
  - **ThemeService.cs** - Manages theme switching
  - **WordCategoryService.cs** - Provides word categories and selections

- **wwwroot/** - Static assets (CSS, images, etc.)

## License

This project is open source and available under the [MIT License](LICENSE).

## Acknowledgments

- Built as a learning project for Blazor WebAssembly
- Inspired by the classic Hangman word game

