using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace EnigmaProject
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public class Distributer
    {
        public StringBuilder distributer = new StringBuilder("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        static public int findc(char a, string s)
        {
            for (int i = 0; i <
                s.Length; i++)
            {
                if (Equals(s[i], a)) return i;
            }
            return 0;
        }
        static public int findc(char a, StringBuilder s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (Equals(s[i], a)) return i;
            }
            return 0;
        }
        public void distribute(char a, char b)
        {
            int index1 = findc(a, distributer);
            int index2 = findc(b, distributer);
            distributer[index1] = b;
            distributer[index2] = a;
        }
    }
    public class Rotor
    {
        public static string[] rotor = {
            "ZMXNCBVALSKDJFHGQPWOEIRUTY" ,
            "QAZWSXEDCRFVTGBYHNUJMIKOLP" ,
            "MNBVCXZLKJHGFDSAPOIUYTREWQ" ,
            "ZYXWVUTSRQPONMLKJIHGFEDCBA" ,
            "HATSUNEMIKYBOXRVGCFDNWQLJP"};
        public string[] ori_rotor = {
            "ZMXNCBVALSKDJFHGQPWOEIRUTY" ,
            "QAZWSXEDCRFVTGBYHNUJMIKOLP" ,
            "MNBVCXZLKJHGFDSAPOIUYTREWQ" ,
            "ZYXWVUTSRQPONMLKJIHGFEDCBA" ,
            "HATSUNEMIKYBOXRVGCFDNWQLJP"};
        public void turn(char front, int index)
        {
            index--;
            while (true)
            {
                if (Rotor.rotor[index][0] == front) return;
                char temp = Rotor.rotor[index][0];
                Rotor.rotor[index] = Rotor.rotor[index].Replace(Rotor.rotor[index].Substring(0, 25), Rotor.rotor[index].Substring(1, 25));
                Rotor.rotor[index] = Rotor.rotor[index].Remove(25, 1);
                Rotor.rotor[index] = Rotor.rotor[index] + temp;
            }
        }
        public char downRot(char c, int index)
        {
            return Rotor.rotor[index - 1][c - 'A'];
        }
        public int upRot(char c, int index)
        {
            return Distributer.findc(c, Rotor.rotor[index - 1]);
        }
    }
    public class Enigma
    {
        static public int[] Selected_rot = { 0, 1, 2, 3 };
        int i, j, rot1cnt = 0, rot2cnt = 0;
        public StringBuilder reflector = new StringBuilder("NZOYPXQWRVSUTACEGIKMLJHFDB");
        Distributer dis1 = new Distributer();
        Rotor rot1 = new Rotor();
        char encChar;
        public string oriStr, resStr="";
        public void setValue(string oriSt)
        {
            oriStr = oriSt.ToUpper();
        }
        public void setRstr()
        {
            resStr = "";
        }
        public string getValue()
        {
            return resStr;
        }
        public void encrypt()
        {
            for (i = 0; i < oriStr.Length; i++)
            {
                encChar = oriStr[i];
                if (encChar == ' ') continue;
                encChar = dis1.distributer[encChar - 'A'];
                for (j = 0; j < 3; j++)
                {
                    encChar = rot1.downRot(encChar, Selected_rot[j]);
                }
                encChar = reflector[encChar - 'A'];
                for (j = 2; j >= 0; j--)
                {
                    encChar = (char)(rot1.upRot(encChar, Selected_rot[j]) + 'A');
                }
                encChar = dis1.distributer[encChar - 'A'];

                resStr = resStr + encChar;
                rot1cnt++;
                rot1.turn(Rotor.rotor[Selected_rot[0]][1], Selected_rot[0]);
                if (rot1cnt == 27)
                {
                    rot1.turn(Rotor.rotor[Selected_rot[1]][1], Selected_rot[1]);
                    rot2cnt++;
                    rot1cnt = 0;
                }
                if (rot2cnt == 26)
                {
                    rot1.turn(Rotor.rotor[Selected_rot[2]][1], Selected_rot[2]);
                    rot2cnt = 0;
                }
            }
        }
    }
    public partial class MainWindow : Window
    {
        Rotor rotors = new Rotor();
        Distributer distr = new Distributer();
        public const int ALPHA = 26;
        int[] rotor_num = { 1, 2, 3, 4, 5 };
        Enigma enigma = new Enigma();

        public MainWindow()
        {
            InitializeComponent();
            reset();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            enigma.setValue(input_box.Text);

            distr.distribute(Char.Parse(distributer1.Text.ToUpper()), Char.Parse(to_distributer1.Text.ToUpper()));
            distr.distribute(Char.Parse(distributer2.Text.ToUpper()), Char.Parse(to_distributer2.Text.ToUpper()));
            distr.distribute(Char.Parse(distributer3.Text.ToUpper()), Char.Parse(to_distributer3.Text.ToUpper()));
            distr.distribute(Char.Parse(distributer4.Text.ToUpper()), Char.Parse(to_distributer4.Text.ToUpper()));
            distr.distribute(Char.Parse(distributer5.Text.ToUpper()), Char.Parse(to_distributer5.Text.ToUpper()));

            rotors.turn(Char.Parse(StartingChar1.Text.ToUpper()), Enigma.Selected_rot[0]);
            rotors.turn(Char.Parse(StartingChar2.Text.ToUpper()), Enigma.Selected_rot[1]);
            rotors.turn(Char.Parse(StartingChar3.Text.ToUpper()), Enigma.Selected_rot[2]);

            enigma.encrypt();
            output_box.Text = " ";
            output_box.Text = enigma.getValue();
            enigma.setRstr();
        }

        private void Rotor1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enigma.Selected_rot[0] = Rotor1.SelectedIndex+1;
        }

        private void Rotor2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enigma.Selected_rot[1] = Rotor2.SelectedIndex+1;
        }

        private void Rotor3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Enigma.Selected_rot[2] = Rotor3.SelectedIndex+1;
        }
        void reset()
        {
            StartingChar1.Text = "A";
            StartingChar2.Text = "A";
            StartingChar3.Text = "A";

            distributer1.Text = "A";
            distributer2.Text = "A";
            distributer3.Text = "A";
            distributer4.Text = "A";
            distributer5.Text = "A";

            to_distributer1.Text = "A";
            to_distributer2.Text = "A";
            to_distributer3.Text = "A";
            to_distributer4.Text = "A";
            to_distributer5.Text = "A";

            distr.distributer = distr.distributer.Replace(distr.distributer.ToString(), "ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            int i;
            for (i = 0; i < 3; i++)
            {
                Rotor.rotor[Enigma.Selected_rot[i]] = rotors.ori_rotor[Enigma.Selected_rot[i]];
            }
            output_box.Text = "Output";
            input_box.Text = "Input";
            enigma.oriStr = "";
            enigma.resStr = "";
            Rotor1.Items.Clear();
            Rotor2.Items.Clear();
            Rotor3.Items.Clear();
            rotor_num.ToList().ForEach(item => Rotor1.Items.Add(item));
            rotor_num.ToList().ForEach(item => Rotor2.Items.Add(item));
            rotor_num.ToList().ForEach(item => Rotor3.Items.Add(item));
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        string textbox_base;
        object list;
        private void Text_MouseDown(object sender, RoutedEventArgs e)
        {
            if (list != null && ((TextBox)list).Text == "") ((TextBox)list).Text = textbox_base;
            textbox_base = ((TextBox)sender).Text;
            ((TextBox)sender).Text = "";
            list = sender;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (list != null && ((TextBox)list).Text == "") ((TextBox)list).Text = textbox_base;
        }

        private void setout_Click(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            window.Owner = this;
            window.Show();
            window.setOut();
        }
    }
}
