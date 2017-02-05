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
        
        private bool setup = true;
        private Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
        }
        private void testPlace(int shipSize)
        {
            Random randomNumber = new Random();
            int initialPointX = randomNumber.Next(0,9);
            int initialPointY = randomNumber.Next(0,9);
            int orientation = randomNumber.Next(1,3);
            int endPointX;
            int endPointY;
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
                            ((dynamic)buttonArrayEnemy[i, initialPointY].Tag).value = shipSize;                          
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
                            ((dynamic)buttonArrayEnemy[initialPointX, i].Tag).value = shipSize;
                        }
                        break;
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

        void buttonArrayClick(object sender, EventArgs e)
        {
            bool winner1 = true;
            bool winner2 = true;
            var current = sender as Button;
            bool shoot = false;
            bool move = true;
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

                    if (numberOfPlayers == 2)
                    {
                        testPlace(5);
                        testPlace(4);
                        testPlace(3);
                        testPlace(3);
                        testPlace(2);

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
                            if (((dynamic)buttonArrayEnemy[i, j].Tag).state == false && ((dynamic)buttonArray[i, j].Tag).hit == false)
                        {
                            if (((dynamic)buttonArrayEnemy[i, j].Tag).value == 0)
                            {
                                buttonArrayEnemy[i, j].BackColor = Color.Gray;
                                ((dynamic)buttonArray[i, j].Tag).hit = true;
                            }
                            else if (((dynamic)buttonArrayEnemy[i, j].Tag).value != 0)
                            {
                                buttonArrayEnemy[i, j].BackColor = Color.Red;
                                ((dynamic)buttonArray[i, j].Tag).hit = true;
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
            Console.WriteLine(winner1);
            Console.WriteLine(winner2);
            }
        }
    
        public class TagInfo
        {
            public string name;
            public int value;
            public bool state;
            public bool hit;
        }

    }


