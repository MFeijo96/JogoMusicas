using JogoDasMusicas.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace JogoDasMusicas.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<Lyric> Lyrics;

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
