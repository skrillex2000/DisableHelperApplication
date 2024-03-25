using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Threading;
using System.Globalization;
using System.IO;

namespace DisableHelperApplication
{
    public partial class Form1 : Form
    {
        //
        string texttoSpeakOut;
        string InputSpeech;

        static bool shiftisPressed;
        FileStream fs;







        //
        public Form1()
        {
            InitializeComponent(); 
            comboBox1.SelectedIndex = 0;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
               await ConvertingTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            button1.Enabled = true;
        }

        private async Task ConvertingTask()
        {
            if (comboBox1.SelectedIndex == 0)
            {
                texttoSpeakOut= richTextBox1.Text;
                await Task.Run(ReadTheLines);
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                fs = File.Create(path+"\\Speech.txt");
                 
                await Task.Run(ListenToSpeech);
                richTextBox1.Text = InputSpeech; 
            }
       
        }

        void ReadTheLines()
        {
            SpeechSynthesizer script = new SpeechSynthesizer();
            script.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.Adult);
          //  script.Speak(texttoSpeakOut.ToString());
            if (true == checkBox1.Checked)
            {
                script.SetOutputToWaveFile(@"C:\Users\nanda\Downloads\MyWavFile.wav");
            }
            script.Speak(texttoSpeakOut.ToString());
           
        }

        void ListenToSpeech()
        {
            SpeechRecognitionEngine spe =  new SpeechRecognitionEngine();
            Grammar words = new DictationGrammar();
            spe.LoadGrammar(words);
            spe.SetInputToDefaultAudioDevice();
            RecognitionResult result = spe.Recognize();
            InputSpeech = result.Text;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.ShiftKey)shiftisPressed = true;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && shiftisPressed == false)
            {
                    button1.PerformClick();
            }
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey) shiftisPressed = false;
        }
    }
}
