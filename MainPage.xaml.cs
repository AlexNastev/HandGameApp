using Microsoft.Extensions.Logging.Abstractions;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.IO.Pipes;

namespace HandGameApp
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private string spotlight;
        private string currentImage = "img0.jpg";
        private string message;
        List<string> words = new List<string>()
        {
            "python",
            "java",
            "sql",
            "csharp",
            "javascript",
            "xaml",
            "maui",
            "excel"
        };
        string answer = "";
        List<char> guessed = new List<char>();
        private List<char> letters = new List<char>();
        private string gameStatus;
        int mistakes = 0;

        public string CurrentImage
        {
            get { return currentImage; }
            set { currentImage = value;
                OnPropertyChanged();
            }
        }
        public string Spotlight
        {
            get { return spotlight; }
            set { spotlight = value;
                OnPropertyChanged();
            }
        }

        public List<char> Letters
        {
            get => letters; set
            {
                letters = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get { return message; }
            set { message = value;
                OnPropertyChanged();
            }
        }
        public string GameStatus
        {
            get => gameStatus; set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

       

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower());
            BindingContext = this;
            PickWord();
            CauculateWord(answer,guessed);
        }

       private void PickWord()
        {
            answer = words[new Random().Next(0, words.Count)];
        }

        private void CauculateWord(string answer, List<char> guessed)
        {
            var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();
            Spotlight = string.Join(' ', temp);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn != null)
            {
                var letter = btn.Text;
                btn.IsEnabled = false;
                HandleGuess(letter[0]);
            }
        }

        private void UpdateStatus()
        {
            GameStatus = $"Errors: {mistakes} of 6";
        }

        private void HandleGuess(char v)
        {
            if (guessed.IndexOf(v) == -1)
            {
                guessed.Add(v);
            }
            if (answer.IndexOf(v) >= 0)
            {
                CauculateWord(answer, guessed);
                CheckIfGameWon();
            }
            else if (answer.IndexOf(v) == -1)
            {
                mistakes++;
                UpdateStatus();
                CheckIfLost();
                CurrentImage = $"img{mistakes}.jpg";
            }
        }

        private void CheckIfLost()
        {
            if (mistakes == 6)
            {
                Message = "You Lost!";
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            foreach (var item in lettersContainer.Children)
            {
                var btn = item as Button;
                if (btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }

        private void EnableLetters()
        {
            foreach (var item in lettersContainer.Children)
            {
                var btn = item as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }

        private void CheckIfGameWon()
        {
            if (Spotlight.Replace(" ", "") == answer)
            {
                Message = "You Win!";
                DisableLetters();
            }
            
        }


        private void Reset_Clicked(object sender, EventArgs e)
        {
            mistakes = 0;
            guessed = new List<char>();
            CurrentImage = "img0.jpg";
            PickWord();
            CauculateWord(answer, guessed);
            Message = "";
            UpdateStatus();
            EnableLetters();
        }
    }

}
