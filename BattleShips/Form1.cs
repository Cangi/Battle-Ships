using System;
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
        private static int columns = 10;
        private static int rows = 10;
        private Button[,] buttonArray = new Button[rows, columns];
        private Button[,] buttonArrayEnemy = new Button[rows, columns];
        private int[] shipsPlayer1 = new int[5];
        private int[] shipsPlayer2 = new int[5];
        private Button startButton;
        private Button shootButton;
        private int numberOfPlayers = 2;
        private int turn = 0;
        private bool lastShoot = false;
        private bool shoot = false;
        private bool move = true;
        private bool setup = true;
        private Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
        }
        private void testPlace(int shipTag)
        {
            Random randomNumber = new Random();
            int initialPointX = randomNumber.Next(0,9);
            int initialPointY = randomNumber.Next(0,9);
            int orientation = randomNumber.Next(1,3);
            int endPointX;
            int endPointY;
            int numberOfTheShips = shipTag%10;
            int shipSize = shipTag / 10;
            bool placed=true;
            if (orientation == 1)
            {
                endPointX = initialPointX + (shipSize - 1);
                endPointY = initialPointY;

                while (true)
                {

                    while (endPointX >= 10 || placed  ==false)
                    {
                        initialPointX = randomNumber.Next(0,9);
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
                        initialPointY = randomNumber.Next(0,9);
                        endPointY = initialPointY + (shipSize - 1);
                        if (endPointY < 10) placed = true;
                    }
                    
                   for (int i = initialPointY; i <= endPointY; i++)
                    {
                        if (((dynamic)buttonArrayEnemy[initialPointX, i].Tag).value != 0)
                        {
                            placed = false;
                            initialPointX=randomNumber.Next(0,9);
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

        private void TestShoot() 
        {
            int[] possibleMoves = new int[4];
            int i= rnd.Next(0,9);
            int j= rnd.Next(0,9);

            if (lastShoot == false)
            {
                if((((dynamic)buttonArray[i,j].Tag).name.Equals("boardButton") && ((dynamic)buttonArray[i,j].Tag).state == true && turn == 2))
                {
                    buttonArray[i,j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\target.png");
                   ((dynamic)buttonArray[i,j].Tag).state = false;
                   ((dynamic)shootButton.Tag).state = true;
                   if (shoot == true) move = true;
                   else move = false;
                }

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
                                }
                            }
            if(shoot!= true) turn = 1;

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
            shootButton.Tag = new TagInfo { name = "ShootButton", value = 0, state = true,hit = false };
            this.Controls.Add(shootButton);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    buttonArray[i, j] = new Button();
                    buttonArray[i, j].Size = new Size(52, 52);
                    buttonArray[i, j].Location = new Point(horizotal, vertical);
                    buttonArray[i, j].Tag = new TagInfo { name = "boardButton", value = 0, state = true };

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
                        shipsPlayer2[4]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 41 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[3]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 32 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[2]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 31 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[1]++;
                        ((dynamic)buttonArray[i, j].Tag).counted = true;
                    }
                    if (((dynamic)buttonArray[i, j].Tag).value == 21 && ((dynamic)buttonArray[i, j].Tag).hit == true && buttonArray[i, j].BackColor == Color.Red && ((dynamic)buttonArray[i, j].Tag).counted != true)
                    {
                        shipsPlayer2[0]++;
                        ((dynamic)buttonArrayEnemy[i, j].Tag).counted = true;
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

            if (shipsPlayer2[4] == 5)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 51) buttonArrayEnemy[i, j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\destroyed.png");
                    }
                }
            }
            if (shipsPlayer2[3] == 4)
            {
                Console.WriteLine("Pidaras41");
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 41) buttonArrayEnemy[i, j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\destroyed.png");
                    }
                }
            }
            if (shipsPlayer2[2] == 3)
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 32) buttonArrayEnemy[i, j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\destroyed.png");
                    }
                }
            }
            if (shipsPlayer2[1] == 3)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 31) buttonArrayEnemy[i, j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\destroyed.png");
                    }
                }
            }
            if (shipsPlayer2[0] == 2)
            {

                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {

                        if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 21) buttonArrayEnemy[i, j].Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\destroyed.png");
                    }
                }
            }
        }

        private void playerShips(int iStart,int jStart,int iEnd,int jEnd)
        { 
            
        }

        void buttonArrayClick(object sender, EventArgs e)
        {
            bool winner1 = false;
            bool winner2 = false;
            var current = sender as Button;
            shoot = false;
            move = true;
            if(setup ==false)
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
                }
            if (((dynamic)current.Tag).name.Equals("ShootButton") && ((dynamic)current.Tag).state==true)
            {
               
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if(turn==1)
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
                                Console.WriteLine(((dynamic)buttonArrayEnemy[i, j].Tag).value);
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
                }
                else if (turn == 2 && shoot == false)
                {
                    turn = 1;
                }
                if (shoot == true) move = true;
                ((dynamic)current.Tag).state = false;                
                Console.WriteLine(shipsPlayer2[0]);
                DestroyedDetection();
            }
            if (move == true && setup == false && numberOfPlayers == 2 && ((((dynamic)current.Tag).name.Equals("boardButton") && ((dynamic)current.Tag).state == true && turn == 2) || ((dynamic)current.Tag).name.Equals("boardButtonEnemy") && ((dynamic)current.Tag).state == true && turn == 1))
               {
                   current.Image = Image.FromFile(@"U:\BattleShips\BattleShips\BattleShips\bin\target.png");
                   ((dynamic)current.Tag).state = false;
                   ((dynamic)shootButton.Tag).state = true;
                   if (shoot == true) move = true;
                   else move = false;
               }
            
            if (setup == true && numberOfPlayers == 2)
               {
                   if (((dynamic)current.Tag).name.Equals("boardButton") || ((dynamic)current.Tag).name.Equals("boardButtonEnemy"))
                   {
                       current.BackColor = Color.Black;
                       ((dynamic)current.Tag).value = 1;
                   }
                }
                else if (setup == true && numberOfPlayers == 1)
                {
                    if (((dynamic)current.Tag).name.Equals("boardButton"))
                    {
                        current.BackColor = Color.Black;
                        ((dynamic)current.Tag).value = 1;                       
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
        }
    }


