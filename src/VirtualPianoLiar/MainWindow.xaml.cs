using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace VirtualPianoLiar
{
    public partial class MainWindow : Window
    {
        KeyboardSimulator simulator;
        Pauses pauses;
        bool pause, stop;

        public MainWindow()
        {
            InitializeComponent();

            simulator = new KeyboardSimulator(new InputSimulator());
            pauses = new Pauses();
            DataContext = pauses;
        }

        //methods
        async Task Pause(int delay = 100)
        {
            await Task.Factory.StartNew(() =>
            {
                while (pause) Task.Delay(delay);
            });
        }
        VirtualKeyCode GetKey(char c)
        {
            switch (c)
            {
                case '1':
                    return VirtualKeyCode.VK_1;
                case '2':
                    return VirtualKeyCode.VK_2;
                case '3':
                    return VirtualKeyCode.VK_3;
                case '4':
                    return VirtualKeyCode.VK_4;
                case '5':
                    return VirtualKeyCode.VK_5;
                case '6':
                    return VirtualKeyCode.VK_6;
                case '7':
                    return VirtualKeyCode.VK_7;
                case '8':
                    return VirtualKeyCode.VK_8;
                case '9':
                    return VirtualKeyCode.VK_9;
                case 'A':
                case 'a':
                    return VirtualKeyCode.VK_A;
                case 'B':
                case 'b':
                    return VirtualKeyCode.VK_B;
                case 'C':
                case 'c':
                    return VirtualKeyCode.VK_C;
                case 'D':
                case 'd':
                    return VirtualKeyCode.VK_D;
                case 'E':
                case 'e':
                    return VirtualKeyCode.VK_E;
                case 'F':
                case 'f':
                    return VirtualKeyCode.VK_F;
                case 'G':
                case 'g':
                    return VirtualKeyCode.VK_G;
                case 'H':
                case 'h':
                    return VirtualKeyCode.VK_H;
                case 'I':
                case 'i':
                    return VirtualKeyCode.VK_I;
                case 'J':
                case 'j':
                    return VirtualKeyCode.VK_J;
                case 'K':
                case 'k':
                    return VirtualKeyCode.VK_K;
                case 'L':
                case 'l':
                    return VirtualKeyCode.VK_L;
                case 'M':
                case 'm':
                    return VirtualKeyCode.VK_M;
                case 'N':
                case 'n':
                    return VirtualKeyCode.VK_N;
                case 'O':
                case 'o':
                    return VirtualKeyCode.VK_O;
                case 'P':
                case 'p':
                    return VirtualKeyCode.VK_P;
                case 'Q':
                case 'q':
                    return VirtualKeyCode.VK_Q;
                case 'R':
                case 'r':
                    return VirtualKeyCode.VK_R;
                case 'S':
                case 's':
                    return VirtualKeyCode.VK_S;
                case 'T':
                case 't':
                    return VirtualKeyCode.VK_T;
                case 'U':
                case 'u':
                    return VirtualKeyCode.VK_U;
                case 'V':
                case 'v':
                    return VirtualKeyCode.VK_V;
                case 'W':
                case 'w':
                    return VirtualKeyCode.VK_W;
                case 'X':
                case 'x':
                    return VirtualKeyCode.VK_X;
                case 'Y':
                case 'y':
                    return VirtualKeyCode.VK_Y;
                default:
                    return VirtualKeyCode.SPACE;
            }
        }
        List<Action> GetActionsByLine(string line)
        {
            List<Action> list = new List<Action>();
            bool IsBrackest = false;
            string Brackest = "";

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '[')
                {
                    IsBrackest = true;
                }
                else if (line[i] == ']')
                {
                    list.Add(GetActionByBrackest(Brackest));
                    list.Add(() => { simulator.Sleep(pauses.BrackestPause); });
                    Brackest = "";
                    IsBrackest = false;
                }
                else if (IsBrackest)
                {
                    Brackest += line[i];
                }
                else if (!char.IsWhiteSpace(line[i]))
                {
                    list.Add(GetActionByChar(line[i]));
                }

                if (line[i] == '|')
                {
                    list.Add(() => { simulator.Sleep(pauses.TooLongPause); });
                }
                else if (char.IsWhiteSpace(line[i]))
                {
                    list.Add(() => { simulator.Sleep(pauses.LongPause); });
                }
                else if (!IsBrackest)
                {
                    list.Add(() => { simulator.Sleep(pauses.ShortPause); });
                }
            }

            list.Add(() => { simulator.Sleep(pauses.LinePause); });

            return list;
        }
        Action GetActionByChar(char c)
        {
            if (char.IsLower(c))
                return () => { simulator.KeyDown(GetKey(c)); };
            else
                return () => { simulator.ModifiedKeyStroke(VirtualKeyCode.SHIFT, GetKey(c)); };
        }
        Action GetActionByBrackest(string Brackest)
        {
            List<VirtualKeyCode> keys = new List<VirtualKeyCode>();
            VirtualKeyCode mod = VirtualKeyCode.INSERT;


            foreach (var item in Brackest)
                if (char.IsUpper(item))
                {
                    mod = VirtualKeyCode.SHIFT;
                    keys.Add(GetKey(item));
                }
                else
                    keys.Add(GetKey(item));

            return () => { simulator.ModifiedKeyStroke(mod, keys); };
        }
        async void PlayAction(List<Action> list)
        {
            foreach (var item in list)
            {
                if (stop) break;

                if (pause)
                    await Pause();
                else
                    item();
            }
        }
        async Task PlayActionAsync(List<Action> list, int delay)
        {
            await Task.Factory.StartNew(() =>
            {
                simulator.Sleep(delay);
                PlayAction(list);
            });
        }
        void SetButtonState(string param)
        {
            switch (param)
            {
                case "play":
                    btnPlay.IsEnabled = false;
                    btnPause.IsEnabled = true;
                    btnStop.IsEnabled = true;
                    break;
                case "pause":
                    btnPlay.IsEnabled = true;
                    btnPause.IsEnabled = false;
                    btnStop.IsEnabled = true;
                    break;
                case "stop":
                    btnPlay.IsEnabled = true;
                    btnPause.IsEnabled = false;
                    btnStop.IsEnabled = false;
                    break;
                default:
                    break;
            }
        }

        //events
        private async void btnPlayClick(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int.TryParse(tbDelay.Text, out i);
            if (i == 0) i = 2000;

            if (!pause)
            {
                pause = false;
                stop = false;

                SetButtonState("play");

                string textwr = tbNote.Text.Replace("\r", "");

                string[] lines = tbNote.Text.Split('\n');

                List<Action> list = new List<Action>();

                foreach (var item in lines)
                    list.AddRange(GetActionsByLine(item));
                
                do
                {
                    await PlayActionAsync(list, i);
                } while (cbRepeat.IsChecked.Value);

                SetButtonState("stop");
            }
            else
            {
                SetButtonState("play");
                await Pause(i);
                pause = false;
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://virtualpiano.net/");
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("http://verloka.xyz/");
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            MyAboutBox amb = new MyAboutBox();
            amb.ShowDialog();
        }
        private void btnPauseClick(object sender, RoutedEventArgs e)
        {
            SetButtonState("pause");
            pause = true;
        }
        private void btnStopClick(object sender, RoutedEventArgs e)
        {
            SetButtonState("stop");
            stop = true;
        }
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
