using JogoDasMusicas.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Timers;

namespace JogoDasMusicas.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Lyric> Lyrics;
        private Timer LyricTimer;
        private int CurrentVerseIndex;

        public GameViewModel ()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JogoDasMusicas.Droid.musica.json");

            using (var reader = new StreamReader(stream))
            {
                Lyrics = JsonConvert.DeserializeObject<Game>(reader.ReadToEnd()).Lyrics;
            }

            var assembly = this.GetType().GetTypeInfo().Assembly.GetManifestResourceNames();

            Stream audioStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("JogoDasMusicas.Droid.Raul Seixas - Maluco Beleza.mp3");
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load(audioStream);
            player.Play();

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
            if (CurrentVerseIndex == Lyrics.Count)
            {
                CurrentVerse = string.Empty;
                LyricTimer.Enabled = false;
            }
            else
            {
                int lastTime = CurrentVerseIndex == 0 ? 10 : Lyrics[CurrentVerseIndex - 1].EndsIn;
                int currentTime = Lyrics[CurrentVerseIndex].EndsIn;
                CurrentVerse = Lyrics[CurrentVerseIndex++].Verse;
                LyricTimer.Interval = (currentTime - lastTime) * 1000;
            }
        }

        void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

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
    }
}
