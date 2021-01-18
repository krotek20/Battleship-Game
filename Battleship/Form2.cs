using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Battleship_game
{
    public partial class Form2 : Form
    {
        private int secondsToWait = 6;
        private DateTime startTime;
        public Form2()
        {
            InitializeComponent();
            timer1.Start(); // Temporizatorul incepe
            startTime = DateTime.Now; // Temporizatorul retine ca valoare de referinta Data & Ora de incepere
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Retine timpul scurs de la initializarea componentei
            int elapsedSeconds = (int)(DateTime.Now - startTime).TotalSeconds;
            // Retine numarul de secunde pana la terminarea programului
            int remainingSeconds = secondsToWait - elapsedSeconds;
            if (remainingSeconds <= 0)
            {
                this.Hide();
                Form3 form3 = new Form3();
                form3.Show();
                timer1.Stop();
            }
            if(remainingSeconds==1)
                label1.Text = String.Format("Preparing for battle...GO!");
            else label1.Text = String.Format("Preparing for battle...{0}", remainingSeconds-1);
        }
    }
}
