using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace Text2Speech
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SpeechConfig speechConfig;
        SpeechSynthesizer synthesizer;
        SynthesisVoicesResult synthesisVoicesResult;
        public SSMLModel ssml { get; set; } = new SSMLModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitSpeech();
            GetSynthesisVoices();
        }
        /// <summary>
        /// 初始化Speech服务
        /// </summary>
        private void InitSpeech()
        {
            speechConfig = SpeechConfig.FromSubscription("your key", "your area");
            synthesizer = new SpeechSynthesizer(speechConfig);
        }
        /// <summary>
        /// 获取所有发音人
        /// </summary>
        /// <param name="language">中文语言</param>
        private async void GetSynthesisVoices(string language = "zh-CN")
        {
            synthesisVoicesResult = await synthesizer.GetVoicesAsync(language);
            SpeakerComboBox.ItemsSource = from x in synthesisVoicesResult.Voices select x.LocalName;
            SpeakerComboBox.SelectedIndex = 0;
        }
        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            await synthesizer.StopSpeakingAsync();
            await synthesizer.SpeakSsmlAsync(ssml.ToString());
        }
        private void SpeakerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var selected = synthesisVoicesResult.Voices.First(x => x.LocalName == e.AddedItems[0].ToString()))
            {
                ssml.Speak = selected.ShortName;
                StyleComboBox.Items.Clear();
                StyleComboBox.IsEnabled = selected.StyleList.Length != 0;
                if (selected.StyleList.Length == 0)
                    return;
                StyleComboBox.ItemsSource = selected.StyleList;
                StyleComboBox.SelectedIndex = 0;
            }
        }
        private void StyleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                ssml.Style = e.AddedItems[0].ToString();
        }
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "wav音频文件|*.wav";
            if (sfd.ShowDialog() == true)
            {
                using (var audioConfig = AudioConfig.FromWavFileOutput(sfd.FileName.ToString()))
                {
                    using (var synthesizerSave = new SpeechSynthesizer(speechConfig, audioConfig))
                    {
                        await synthesizerSave.SpeakSsmlAsync(ssml.ToString());
                    }
                }
            }
        }
    }
}
