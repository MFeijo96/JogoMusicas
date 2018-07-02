using JogoDasMusicas.Models;
using Newtonsoft.Json;
using Plugin.SimpleAudioPlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace JogoDasMusicas.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand ButtonClickedCommand { get; set; }
        public List<Lyric> Lyrics;
        private Timer LyricTimer, AnswerTimer;
        private int CurrentVerseIndex;
        private ISimpleAudioPlayer Player;
        private DateTime StartTime;
        private bool WaitForAnswer;

        public GameViewModel ()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JogoDasMusicas.Droid.musica.json");
            CanAnswer = false;

            using (var reader = new StreamReader(stream))
            {
                Lyrics = JsonConvert.DeserializeObject<Game>(reader.ReadToEnd()).Lyrics;
            }

            var assembly = this.GetType().GetTypeInfo().Assembly.GetManifestResourceNames();

            Stream audioStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JogoDasMusicas.Droid.Raul Seixas - Maluco Beleza.mp3");
            Player = CrossSimpleAudioPlayer.Current;
            Player.Load(audioStream);
            Player.Play();

            StartTimer();
        }

        private void StartTimer()
        {
            LyricTimer = new Timer(10000);
            LyricTimer.Elapsed += OnFinishTime;
            LyricTimer.Enabled = true;
        }

        private void OnFinishTime(object sender, ElapsedEventArgs e)
        {
            if (WaitForAnswer)
            {
                Player.Pause();
                StartAnswerTimer();
            }
            else if (CurrentVerseIndex == Lyrics.Count)
            {
                CurrentVerse = string.Empty;
                LyricTimer.Enabled = false;
            }
            else
            {
                UpdateLyric();

                WaitForAnswer = CurrentVerse.Contains("*");
            }
        }

        private void UpdateLyric()
        {
            int lastTime = CurrentVerseIndex == 0 ? 10 : Lyrics[CurrentVerseIndex - 1].EndsIn;
            int currentTime = Lyrics[CurrentVerseIndex].EndsIn;
            CurrentVerse = Lyrics[CurrentVerseIndex++].Verse;
            LyricTimer.Interval = (currentTime - lastTime) * 1000;
        }

        private void StartAnswerTimer()
        {
            CanAnswer = true;
            StartTime = DateTime.Now;
            AnswerTimer = new Timer(10000);
            AnswerTimer.Elapsed += OnFinishAnswerTime;
            AnswerTimer.Enabled = true;
        }

        private void OnFinishAnswerTime(object sender, ElapsedEventArgs e)
        {
            ResetMusic();
        }

        public void OnCompletedEntry()
        {
            if (CanAnswer)
            {
                if (WordText.Equals(Lyrics[CurrentVerseIndex - 1].CorrectWord))
                {
                    TimeSpan span = DateTime.Now - StartTime;
                    TotalPoints += (int)span.TotalMilliseconds / 10;
                }

                ResetMusic();
            }
        }

        private void ResetMusic()
        {
            WordText = string.Empty;
            AnswerTimer.Enabled = false;
            AnswerTimer = null;
            WaitForAnswer = false;
            CanAnswer = false;
            UpdateLyric();
            Player.Play();
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #region Properties
        string currentVerse = string.Empty;
        public string CurrentVerse
        {
            get { return currentVerse; }
            set
            {
                currentVerse = value;
                OnPropertyChanged();
            }
        }

        int totalPoints = 0;
        public int TotalPoints
        {
            get { return totalPoints; }
            set
            {
                totalPoints = value;
                OnPropertyChanged();
            }
        }

        string wordText = string.Empty;
        public string WordText
        {
            get { return wordText; }
            set
            {
                wordText = value;
                OnPropertyChanged();
            }
        }

        bool canAnswer = false;
        public bool CanAnswer
        {
            get { return canAnswer; }
            set
            {
                canAnswer = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
