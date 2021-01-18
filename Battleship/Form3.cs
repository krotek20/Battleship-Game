using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics; // clasa prin care AI va face propriile sale alegeri
namespace Battleship_game
{
    public partial class Form3 : Form
    {
        List<Button> playerPosition; // lista pozitiilor playerului
        List<Button> enemyPosition; // list pozitiilor adversarului
        Random rand = new Random();
        int totalPlayer = 3; // numarul total de barci ale playerului
        int totalEnemy = 3; // numarul total de barci ale adversarului
        int rounds = 10; // numarul total de runde
        int playerTotalScore = 0; // scorul playerului
        int enemyTotalScore = 0; // scorul adversarului
        public Form3()
        {
            InitializeComponent();
            loadbuttons(); // incarca toate butoanele din forma
            attackButton.Enabled = false; // butonul de atac nu este pornit pana cand playerul nu alege pozitiile 
            enemyLocationList.Text = null; // drop down boxul este initial gol
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void playerPicksPosition(object sender, EventArgs e)
        {
            // Playerul alege 3 pozitii pe harta pentru barcile sale
            if (totalPlayer > 0)
            {
                var button = (Button)sender; // numele butonului apasat
                button.Enabled = false; // acesta va fi oprit
                button.Tag = "playerShip"; // tine mine ca aici este barca
                button.BackColor = System.Drawing.Color.Blue;
                totalPlayer--; // scade numarul de barci ale playerului
            }
            if (totalPlayer == 0)
            {
                attackButton.Enabled = true;
                attackButton.BackColor = System.Drawing.Color.Red;
                helpText.Top = 55;
                helpText.Left = 230;
                helpText.Text = "2) Alege o pozitie de atac din Combo Box";
            }
        }

        private void attackEnemyPosition(object sender, EventArgs e)
        {
            if (enemyLocationList.Text != "")
            {
                var attackPos = enemyLocationList.Text; // pozitia pe care vrea playerul sa o atace
                attackPos = attackPos.ToLower(); // schimbare in caractere mici (lower case)
                int index = enemyPosition.FindIndex(a => a.Name == attackPos);
                // gasirea indexului pozitiei atacate de catre player
                // functia preia fiecare index si-l compara cu cel din attackPos
                // pana cand gaseste indexul cautat (matched)

                // acum putem modifica pozitia pe care a atacat-o playerul
                if (enemyPosition[index].Enabled && rounds > 0)
                {
                    rounds--;
                    roundsText.Text = "Rounds " + rounds;
                    if (enemyPosition[index].Tag == "enemyShip")
                    {
                        // daca era barca in pozitia atacata...
                        enemyPosition[index].Enabled = false;
                        enemyPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                        enemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        playerTotalScore++;
                        playerScore.Text = "" + playerTotalScore;
                        enemyPlayTimer.Start(); // incepe timerul, asa ca adversarul isi poate face mutarea
                    }
                    else
                    {
                        enemyPosition[index].Enabled = false;
                        enemyPosition[index].BackgroundImage = Properties.Resources.missIcon;
                        enemyPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                        enemyPlayTimer.Start();
                    }
                }
            }
            else
            {
                MessageBox.Show("Alege o locatie din drop down box!");
            }
        }

        private void enemyAttackPlayer(object sender, EventArgs e)
        {
            // AI
            if (playerPosition.Count > 0 && rounds > 0)
            {
                rounds--;
                roundsText.Text = "Rounds " + rounds;
                int index = rand.Next(playerPosition.Count); // index pentru o pozitie aleatoare a playerului
                if (playerPosition[index].Tag == "playerShip")
                {
                    playerPosition[index].BackgroundImage = Properties.Resources.fireIcon;
                    enemyMoves.Text = "" + playerPosition[index].Text;
                    playerPosition[index].Enabled = false;
                    playerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    playerPosition.RemoveAt(index);
                    // sterge pozitia atacata acum (din lista) pentru a nu mai fii atacata in viitor de catre CPU
                    enemyTotalScore++;
                    enemyScore.Text = "" + enemyTotalScore;
                    enemyPlayTimer.Stop(); // se opreste timpul pentru CPU
                }
                else
                {
                    playerPosition[index].BackgroundImage = Properties.Resources.missIcon;
                    enemyMoves.Text = "" + playerPosition[index].Text;
                    playerPosition[index].Enabled = false;
                    playerPosition[index].BackColor = System.Drawing.Color.DarkBlue;
                    playerPosition.RemoveAt(index);
                    enemyPlayTimer.Stop();
                }
            }
            // Sa vedem daca ai castigat sau nu :?
            if (rounds < 1 || playerTotalScore > 2 || enemyTotalScore > 2)
            {
                if (playerTotalScore > enemyTotalScore)
                {
                    MessageBox.Show("Ai castigat!", "Victorie");
                }
                if (playerTotalScore == enemyTotalScore)
                {
                    MessageBox.Show("Niciun castigator!", "Egal");
                }
                if (playerTotalScore < enemyTotalScore)
                {
                    MessageBox.Show("Ai pierdut!", "Infrangere");
                }
            }
        }

        private void enemyPicksPosition(object sender, EventArgs e)
        {
            // AI
            // CPU isi alege 3 pozitii pe harta pentru barcile sale
            int index = rand.Next(enemyPosition.Count); // alege pozitie aleatoare
            if (enemyPosition[index].Enabled == true && enemyPosition[index].Tag == null)
            {
                enemyPosition[index].Tag = "enemyShip";
                totalEnemy--;
                Debug.WriteLine("Enemy Position " + enemyPosition[index].Text);
                // ne apare un debug window cu ce pozitii a ales adversarul
                // putem vedea astfel daca totul merge cum trebuie
            }
            else
            {
                index = rand.Next(enemyPosition.Count);
            }
            if (totalEnemy < 1)
            {
                // cand adversarul alege 3 pozitii, oprim timerul
                enemyPositionPicker.Stop();
            }
        }

        private void loadbuttons()
        {
            // Incarcam toate butoanele atat pentru player cat si pentru adversar
            playerPosition = new List<Button> { w1, w2, w3, w4, x1, x2, x3, x4, y1, y2, y3, y4, z1, z2, z3, z4 };
            enemyPosition = new List<Button> { a1, a2, a3, a4, b1, b2, b3, b4, c1, c2, c3, c4, d1, d2, d3, d4 };
            // Trecem prin fiecare pozitie a adversarului
            // si le adaugam in drop down box
            for (int i = 0; i < enemyPosition.Count; i++)
            {
                enemyPosition[i].Tag = null;
                enemyLocationList.Items.Add(enemyPosition[i].Text);
            }
        }
    }
}
