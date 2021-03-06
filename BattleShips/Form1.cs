﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShips
{
    public partial class Form1 : Form
    {
        #region variables
        private static int columns = 10;
        private static int rows = 10;
        private Button[,] buttonArray = new Button[rows, columns];
        private Button[,] buttonArrayEnemy = new Button[rows, columns];
        private int[] shipsPlayer1 = new int[5];
        private int[] shipsPlayer2 = new int[5];
        private Button[] possibleMoves = new Button[4]{null,null,null,null};
        private Button startButton;
        private Button shootButton;
        private Button AIturn;
        private int numberOfPlayers = 2;
        private int turn = 0;
        private bool lastShoot = false;
        private bool shoot = false;
        private bool move = true;
        private bool setup = true;
        private Random rnd = new Random();
        private string destroyedPath = @"bin\destroyed.png";
        private string targetPath = @"bin\target.png";
        private Point lastButtonPressed;
        private bool firstClick = false;
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        private void testPlace(int shipTag)
        {
            Random randomNumber = new Random();
            int initialPointX = randomNumber.Next(0, 9);
            int initialPointY = randomNumber.Next(0, 9);
            int orientation = randomNumber.Next(1, 3);
            int endPointX;
            int endPointY;
            int numberOfTheShips = shipTag % 10;
            int shipSize = shipTag / 10;
            bool placed = true;
            if (orientation == 1)
            {
                endPointX = initialPointX + (shipSize - 1);
                endPointY = initialPointY;

                while (true)
                {

                    while (endPointX >= 10 || placed == false)
                    {
                        initialPointX = randomNumber.Next(0, 9);
                        endPointX = initialPointX + (shipSize - 1);
                        if (endPointX < 10) placed = true;
                    }
                    placed = true;
                    for (int i = initialPointX; i <= endPointX; i++)
                    {
                        if (((dynamic)buttonArrayEnemy[i, initialPointY].Tag).value != 0)
                        {
                            placed = false;
                            initialPointY = randomNumber.Next(0, 9);
                            break;
                        }
                    }
                    if (placed == true)
                    {

                        if (shipSize == 3) shipSize = shipSize * 10 + randomNumber.Next(1, 3);
                        for (int i = initialPointX; i <= endPointX; i++)
                        {
                            ((dynamic)buttonArrayEnemy[i, initialPointY].Tag).value = shipTag;
                        }
                        break;
                    }

                }
            }
            else if (orientation == 2)
            {
                endPointX = initialPointX;
                endPointY = initialPointY + (shipSize - 1);
                while (true)
                {

                    while (endPointY >= 10 || placed == false)
                    {
                        initialPointY = randomNumber.Next(0, 9);
                        endPointY = initialPointY + (shipSize - 1);
                        if (endPointY < 10) placed = true;
                    }

                    for (int i = initialPointY; i <= endPointY; i++)
                    {
                        if (((dynamic)buttonArrayEnemy[initialPointX, i].Tag).value != 0)
                        {
                            placed = false;
                            initialPointX = randomNumber.Next(0, 9);
                            break;
                        }
                    }

                    if (placed == true)
                    {
                        for (int i = initialPointY; i <= endPointY; i++)
                        {
                            ((dynamic)buttonArrayEnemy[initialPointX, i].Tag).value = shipTag;
                        }
                        break;
                    }

                }
            }
        }

        private void AIShoot(object s, EventArgs e)
        {
            TestShoot();
            ((System.Timers.Timer)s).Enabled = false; 
        }
        private void TestShoot()
        {
            

            shoot = false;
            
            int i;
            int j;
            int x;
            i = rnd.Next(0, 9);
            j = rnd.Next(0, 9);
            if (lastShoot == false)
            {
                while (((dynamic)buttonArray[i, j].Tag).state != true)
                {
                    i = rnd.Next(0, 10);
                    j = rnd.Next(0, 10);
                }
                if ((((dynamic)buttonArray[i, j].Tag).name.Equals("boardButton") && ((dynamic)buttonArray[i, j].Tag).state == true))
                {
                    buttonArray[i, j].Image = Image.FromFile(targetPath);
                    ((dynamic)buttonArray[i, j].Tag).state = false;
                }
                if (((dynamic)buttonArray[i, j].Tag).state == false && ((dynamic)buttonArray[i, j].Tag).hit == false)
                {
                    if (((dynamic)buttonArray[i, j].Tag).value == 0)
                    {
                        buttonArray[i, j].BackColor = Color.Gray;
                        ((dynamic)buttonArray[i, j].Tag).hit = true;
                    }
                    else if (((dynamic)buttonArray[i, j].Tag).value != 0)
                    {
                        buttonArray[i, j].BackColor = Color.Red;
                        ((dynamic)buttonArray[i, j].Tag).hit = true;
                        shoot = true;
                        lastShoot = true;
                            
                        #region possibleMoves
                            switch (i)
                            {
                                case 0:
                                    switch (j)
                                    {
                                        case 0:
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            if (((dynamic)buttonArray[i+1, j].Tag).hit == false && ((dynamic)buttonArray[i+1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                        case 9:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i + 1, j].Tag).hit == false && ((dynamic)buttonArray[i + 1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                        default:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            if (((dynamic)buttonArray[i + 1, j].Tag).hit == false && ((dynamic)buttonArray[i + 1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                    }
                                    break;
                                case 9:
                                    switch (j)
                                    {
                                        case 0:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            break;
                                        case 9:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i - 1, j].Tag).hit == false && ((dynamic)buttonArray[i - 1, j].Tag).state == true) possibleMoves[1] = buttonArray[i - 1, j];
                                            break;
                                        default:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i - 1, j].Tag).hit == false && ((dynamic)buttonArray[i - 1, j].Tag).state == true) possibleMoves[1] = buttonArray[i - 1, j];
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            break;
                                    }
                                    break;
                                default:
                                    switch (j)
                                    {
                                        case 0:
                                            if (((dynamic)buttonArray[i - 1, j].Tag).hit == false && ((dynamic)buttonArray[i - 1, j].Tag).state == true) possibleMoves[1] = buttonArray[i - 1, j];
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            if (((dynamic)buttonArray[i + 1, j].Tag).hit == false && ((dynamic)buttonArray[i + 1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                        case 9:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i - 1, j].Tag).hit == false && ((dynamic)buttonArray[i - 1, j].Tag).state == true) possibleMoves[1] = buttonArray[i - 1, j];
                                            if (((dynamic)buttonArray[i + 1, j].Tag).hit == false && ((dynamic)buttonArray[i + 1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                        default:
                                            if (((dynamic)buttonArray[i, j - 1].Tag).hit == false && ((dynamic)buttonArray[i, j - 1].Tag).state == true) possibleMoves[0] = buttonArray[i, j - 1];
                                            if (((dynamic)buttonArray[i - 1, j].Tag).hit == false && ((dynamic)buttonArray[i - 1, j].Tag).state == true) possibleMoves[1] = buttonArray[i - 1, j];
                                            if (((dynamic)buttonArray[i, j + 1].Tag).hit == false && ((dynamic)buttonArray[i, j + 1].Tag).state == true) possibleMoves[2] = buttonArray[i, j + 1];
                                            if (((dynamic)buttonArray[i + 1, j].Tag).hit == false && ((dynamic)buttonArray[i + 1, j].Tag).state == true) possibleMoves[3] = buttonArray[i + 1, j];
                                            break;
                                    }
                                    
                                    break;

                            }
                        #endregion
                    }
                }
            }
            else if (lastShoot == true)
            {
                int counter = 0; 
                x = rnd.Next(0,4);
                while(possibleMoves[x]==null)
                {
                    x = rnd.Next(0, 4);
                }
                possibleMoves[x].Image = Image.FromFile(targetPath);
                ((dynamic)possibleMoves[x].Tag).state = false;
                if (((dynamic)possibleMoves[x].Tag).state == false && ((dynamic)possibleMoves[x].Tag).hit == false)
                {
                    if (((dynamic)possibleMoves[x].Tag).value == 0)
                    {
                        possibleMoves[x].BackColor = Color.Gray;
                        ((dynamic)possibleMoves[x].Tag).hit = true;
                        possibleMoves[x] = null;

                    }
                    else if (((dynamic)possibleMoves[x].Tag).value != 0)
                    {
                        possibleMoves[x].BackColor = Color.Red;
                        ((dynamic)possibleMoves[x].Tag).hit = true;
                        shoot = true;
                        switch (x)
                        {
                            case 0:
                                Console.WriteLine("0");
                                if (((dynamic)possibleMoves[x].Tag).col > 0) possibleMoves[x] = buttonArray[((dynamic)possibleMoves[x].Tag).row, ((dynamic)possibleMoves[x].Tag).col - 1];
                                else possibleMoves[x]=null;
                                break;
                            case 1:
                                Console.WriteLine("1");
                                if (((dynamic)possibleMoves[x].Tag).row >0) possibleMoves[x] = buttonArray[((dynamic)possibleMoves[x].Tag).row - 1, ((dynamic)possibleMoves[x].Tag).col];
                                else possibleMoves[x]=null;
                                break;
                            case 2:
                                Console.WriteLine("2");
                                if(((dynamic)possibleMoves[x].Tag).col < 9) possibleMoves[x] = buttonArray[((dynamic) possibleMoves[x].Tag).row, ((dynamic)possibleMoves[x].Tag).col+1];
                                else possibleMoves[x]=null;
                                break;
                            case 3:
                                Console.WriteLine("3");
                                if (((dynamic)possibleMoves[x].Tag).row < 9) possibleMoves[x] = buttonArray[((dynamic)possibleMoves[x].Tag).row + 1, ((dynamic)possibleMoves[x].Tag).col];
                                else possibleMoves[x]=null;
                                break;
                        }
                    }
                }
                    for (i = 0; i < 4; i++)
                    {
                        if (possibleMoves[i] == null) counter++;
                    }
                
                if(counter == 4) lastShoot = false;
            
            }
            if (shoot == true) TestShoot();
            else turn = 1;                                    
            }
        private void debug()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if (((dynamic)buttonArray[i, z].Tag).value != 0)
                    {
                        Console.WriteLine(((dynamic)buttonArray[i, z].Tag).value);
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int horizotal = 10;
            int vertical = 40;
            int horizotal2 = 550;
            int vertical2 = 40;
            startButton = new Button();
            startButton.Size = new Size(104, 52);
            startButton.Location = new Point(1200, 60);
            startButton.Text = "Start Button";
            startButton.Tag = new TagInfo { name = "startButton", value = 0, state = true };
            this.Controls.Add(startButton);
            shootButton = new Button();
            shootButton.Size = new Size(104, 52);
            shootButton.Location = new Point(1200, 115);
            shootButton.Text = "Shoot";
            shootButton.Tag = new TagInfo { name = "ShootButton", value = 0, state = true, hit = false };
            this.Controls.Add(shootButton);
            AIturn = new Button();
            AIturn.Size = new Size(104, 52);
            AIturn.Location = new Point(1200, 115);
            AIturn.Text = "Compter's turn";
            AIturn.Tag = new TagInfo { name = "Computer", value = 0, state = true, hit = false };
            AIturn.Hide();
            this.Controls.Add(AIturn);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    buttonArray[i, j] = new Button();
                    buttonArray[i, j].Size = new Size(52, 52);
                    buttonArray[i, j].Location = new Point(horizotal, vertical);
                    buttonArray[i, j].Tag = new TagInfo { name = "boardButton", value = 0, state = true, row = i, col = j };

                    if ((j + 1) % 10 == 0)
                    {
                        vertical += 50;
                        horizotal = 9;
                    }
                    else horizotal += 51;
                    this.Controls.Add(buttonArray[i, j]);

                    buttonArrayEnemy[i, j] = new Button();
                    buttonArrayEnemy[i, j].Size = new Size(52, 52);
                    buttonArrayEnemy[i, j].Location = new Point(horizotal2, vertical2);
                    buttonArrayEnemy[i, j].Tag = new TagInfo { name = "boardButtonEnemy", value = 0, state = true };
                    if ((j + 1) % 10 == 0)
                    {
                        vertical2 += 50;
                        horizotal2 = 549;
                    }
                    else horizotal2 += 51;
                    buttonArray[i, j].Click += new EventHandler(buttonArrayClick);
                    buttonArrayEnemy[i, j].Click += new EventHandler(buttonArrayClick);
                    startButton.Click += new EventHandler(buttonArrayClick);
                    shootButton.Click += new EventHandler(buttonArrayClick);
                    AIturn.Click += new EventHandler(buttonArrayClick);

                    this.Controls.Add(buttonArrayEnemy[i, j]);
                }

            }

        }

        private void DestroyedDetection()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    //Player
                    if (((dynamic)buttonArray[i, j].Tag).value == 51 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer1[4]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 41 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer1[3]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 32 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer1[2]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 31 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer1[1]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 21 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer1[0]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    //Enemy
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 51 && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == true && buttonArrayEnemy[i, j].BackColor == Color.Red && ((dynamic)buttonArrayEnemy[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[4]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 41 && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == true && buttonArrayEnemy[i, j].BackColor == Color.Red && ((dynamic)buttonArrayEnemy[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[3]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 32 && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == true && buttonArrayEnemy[i, j].BackColor == Color.Red && ((dynamic)buttonArrayEnemy[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[2]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 31 && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == true && buttonArrayEnemy[i, j].BackColor == Color.Red && ((dynamic)buttonArrayEnemy[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[1]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 21 && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == true && buttonArrayEnemy[i, j].BackColor == Color.Red && ((dynamic)buttonArrayEnemy[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[0]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
                    }
                }
            }
            //Player2
            if (shipsPlayer2[4] == 5)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 51) buttonArrayEnemy[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer2[3] == 4)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 41) buttonArrayEnemy[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer2[2] == 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 32) buttonArrayEnemy[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer2[1] == 3)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 31) buttonArrayEnemy[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer2[0] == 2)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 21) buttonArrayEnemy[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            //Player 1
            if (shipsPlayer1[4] == 5)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArray[i, j].Tag).value == 51) buttonArray[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer1[3] == 4)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArray[i, j].Tag).value == 41) buttonArray[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer1[2] == 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArray[i, j].Tag).value == 32) buttonArray[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer1[1] == 3)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArray[i, j].Tag).value == 31) buttonArray[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
            if (shipsPlayer1[0] == 2)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArray[i, j].Tag).value == 21) buttonArray[i, j].Image = Image.FromFile(destroyedPath);
                    }
                }
            }
        }

        private void playerShips(int iStart, int jStart, int iEnd, int jEnd, int shipValue)
        {
            int changeValue = 0;
            int secondShipsNeeded = 0;
            if (iStart > iEnd)
            {
                changeValue = iEnd;
                iEnd = iStart;
                iStart = changeValue;
                changeValue = 0;
            }
            if (jStart > jEnd)
            {
                changeValue = jEnd;
                jEnd = jStart;
                jStart = changeValue;
                changeValue = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if (((dynamic)buttonArray[i, z].Tag).value == 31)
                    {
                        secondShipsNeeded++;
                    }
                }
            }
            if (iStart == iEnd)
                for (int j = jStart; j <= jEnd; j++)
                {

                    switch (shipValue)
                    {
                        case 5:
                            buttonArray[iStart, j].BackColor = Color.Black;
                            ((dynamic)buttonArray[iStart, j].Tag).value = 51;
                            break;
                        case 4:
                            buttonArray[iStart, j].BackColor = Color.Black;
                            ((dynamic)buttonArray[iStart, j].Tag).value = 41;
                            break;
                        case 3:

                            if (secondShipsNeeded == 3)
                            {
                                buttonArray[iStart, j].BackColor = Color.Black;
                                ((dynamic)buttonArray[iStart, j].Tag).value = 32;
                            }
                            else
                            {
                                buttonArray[iStart, j].BackColor = Color.Black;
                                ((dynamic)buttonArray[iStart, j].Tag).value = 31;
                            }

                            break;
                        case 2:
                            buttonArray[iStart, j].BackColor = Color.Black;
                            ((dynamic)buttonArray[iStart, j].Tag).value = 21;
                            break;
                    }
                }
            if (jStart == jEnd)
            {
                for (int i = iStart; i <= iEnd; i++)
                {

                    buttonArray[i, jStart].BackColor = Color.Black;
                    switch (shipValue)
                    {
                        case 5:
                            ((dynamic)buttonArray[i, jStart].Tag).value = 51;
                            break;
                        case 4:
                            ((dynamic)buttonArray[i, jStart].Tag).value = 41;
                            break;
                        case 3:

                            if (secondShipsNeeded == 3)
                            {
                                buttonArray[i, jStart].BackColor = Color.Black;
                                ((dynamic)buttonArray[i, jStart].Tag).value = 32;
                            }
                            else
                            {
                                buttonArray[i, jStart].BackColor = Color.Black;
                                ((dynamic)buttonArray[i, jStart].Tag).value = 31;
                            }
                            break;
                        case 2:
                            ((dynamic)buttonArray[i, jStart].Tag).value = 21;
                            break;
                    }
                }
            }
        }

        private void buttonArrayClick(object sender, EventArgs e)
        {
            bool winner1 = false;
            bool winner2 = false;
            var current = sender as Button;
            shoot = false;
            move = true;
            if (setup == false)
            {
                winner1 = true;
                winner2 = true;
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (((dynamic)buttonArrayEnemy[i, j].Tag).value != 0 && ((dynamic)buttonArray[i, j].Tag).hit == false)
                        winner1 = false;
                    if (((dynamic)buttonArray[i, j].Tag).value != 0 && ((dynamic)buttonArray[i, j].Tag).hit == false)
                        winner2 = false;
                }
            }
            #region startButton
            if (((dynamic)current.Tag).name.Equals("startButton") && setup == true)
            {
                setup = false;
                for (int i = 0; i < 5; i++)
                {
                    shipsPlayer1[i] = 0;
                    shipsPlayer2[i] = 0;
                }
                if (numberOfPlayers == 2)
                {
                    testPlace(51);
                    testPlace(41);
                    testPlace(31);
                    testPlace(32);
                    testPlace(21);

                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (((dynamic)buttonArrayEnemy[i, j].Tag).value != 0)
                            {
                                buttonArrayEnemy[i, j].BackColor = Color.Black;
                            }
                        }
                    }
                }
                turn = rnd.Next(1, 3);
                Console.WriteLine(turn);
                if (turn == 2) TestShoot();
            }
            #endregion
            #region shootButton
            if (((dynamic)current.Tag).name.Equals("ShootButton") && ((dynamic)current.Tag).state == true)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (turn == 1)
                        {
                            if (((dynamic)buttonArrayEnemy[i, j].Tag).state == false && ((dynamic)buttonArrayEnemy[i, j].Tag).hit == false)
                            {
                                if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 0)
                                {
                                    buttonArrayEnemy[i, j].BackColor = Color.Gray;
                                    ((dynamic)buttonArrayEnemy[i, j].Tag).hit = true;
                                }
                                else if (((dynamic)buttonArrayEnemy[i, j].Tag).value != 0)
                                {
                                    buttonArrayEnemy[i, j].BackColor = Color.Red;
                                    ((dynamic)buttonArrayEnemy[i, j].Tag).hit = true;
                                    shoot = true;

                                }
                            }
                        }
                        else
                        {
                            if (((dynamic)buttonArray[i, j].Tag).state == false && ((dynamic)buttonArray[i, j].Tag).hit == false)
                            {
                                if (((dynamic)buttonArray[i, j].Tag).value == 0)
                                {
                                    buttonArray[i, j].BackColor = Color.Gray;
                                    ((dynamic)buttonArray[i, j].Tag).hit = true;
                                }
                                else if (((dynamic)buttonArray[i, j].Tag).value != 0)
                                {
                                    buttonArray[i, j].BackColor = Color.Red;
                                    ((dynamic)buttonArray[i, j].Tag).hit = true;
                                    shoot = true;
                                }
                            }

                        }
                    }
                }
                if (turn == 1 && shoot == false)
                {
                    turn = 2;
                    System.Timers.Timer timer = new System.Timers.Timer(500);
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(AIShoot);
                    timer.Enabled = true;
                }
                else if (turn == 2 && shoot == false)
                {
                    turn = 1;
                }
                if (shoot == true) move = true;
                ((dynamic)current.Tag).state = false;
                DestroyedDetection();
                
            }
            #endregion

            if (move == true && setup == false && numberOfPlayers == 2 && ((((dynamic)current.Tag).name.Equals("boardButton") && ((dynamic)current.Tag).state == true && turn == 2) || ((dynamic)current.Tag).name.Equals("boardButtonEnemy") && ((dynamic)current.Tag).state == true && turn == 1))
            {
                current.Image = Image.FromFile(targetPath);
                ((dynamic)current.Tag).state = false;
                ((dynamic)shootButton.Tag).state = true;
                if (shoot == true) move = true;
                else move = false;
            }
            if (move == true && setup == false && numberOfPlayers == 1)
            {
                if (turn == 1 && ((dynamic)current.Tag).name.Equals("boardButtonEnemy") && ((dynamic)current.Tag).state == true)
                {
                    current.Image = Image.FromFile(targetPath);
                    ((dynamic)current.Tag).state = false;
                    ((dynamic)shootButton.Tag).state = true;
                    if (shoot == true) move = true;
                    else move = false;
                }
                else if(turn ==2)
                {
                    System.Timers.Timer timer = new System.Timers.Timer(500);
                    timer.Elapsed += new System.Timers.ElapsedEventHandler(AIShoot);
                    timer.Enabled = true;
                }
            }

            if (setup == true && numberOfPlayers == 2)
            {

                if (((dynamic)current.Tag).name.Equals("boardButton"))
                {
                    if (firstClick == false)
                    {
                        lastButtonPressed = new Point(((dynamic)current.Tag).row, ((dynamic)current.Tag).col);
                        firstClick = true;
                        return;
                    }
                    if (firstClick == true)
                    {

                        if ((Math.Abs(lastButtonPressed.X) - Math.Abs(((dynamic)current.Tag).row)) == 0)
                        {
                            if (lastButtonPressed.X > ((dynamic)current.Tag).row) playerShips(((dynamic)current.Tag).row, ((dynamic)current.Tag).col, lastButtonPressed.X, lastButtonPressed.Y, Math.Abs(((dynamic)current.Tag).col - lastButtonPressed.Y) + 1);
                            else playerShips(lastButtonPressed.X, lastButtonPressed.Y, ((dynamic)current.Tag).row, ((dynamic)current.Tag).col, Math.Abs(((dynamic)current.Tag).col - lastButtonPressed.Y) + 1);
                        }
                        else if ((Math.Abs(lastButtonPressed.Y) - Math.Abs(((dynamic)current.Tag).col)) == 0)
                        {
                            if (lastButtonPressed.Y > ((dynamic)current.Tag).col) playerShips(((dynamic)current.Tag).row, ((dynamic)current.Tag).col, lastButtonPressed.X, lastButtonPressed.Y, Math.Abs(((dynamic)current.Tag).row - lastButtonPressed.X) + 1);
                            else playerShips(lastButtonPressed.X, lastButtonPressed.Y, ((dynamic)current.Tag).row, ((dynamic)current.Tag).col, Math.Abs(((dynamic)current.Tag).row - lastButtonPressed.X) + 1);
                        }
                        firstClick = false;
                    }
                }
            }

            if (winner1) Console.WriteLine("Player 1 won");
            else if (winner2) Console.WriteLine("Player 2 won");
        }
    }
    
        public class TagInfo
        {
            public string name;
            public int value;
            public bool state;
            public bool hit;
            public bool counted;
            public int row;
            public int col;
        }
    }


