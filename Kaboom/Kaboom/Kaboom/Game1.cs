///Author: Vibeesshanan Thevamanoharan
///Project Name: Kaboom
///File Name: Game 1
///Creation Date: November 26th 2017
///Modified Date: December 12th 2017
///Description: Reacreate the classic ACTIVISION game "Kaboom" with a twist from 
///V-Tech Gaming to add a Chicken and egg theme, music and gameplay, Presenting "EggBOOM!"
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Kaboom
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


       
        //The 3 different main screens,
        Texture2D background;
        Rectangle backgroundRec;
        Texture2D MenuScreen;
        Rectangle MenuScreenRec;
        Texture2D EndScreen;
        Rectangle EndScreenRec;

        //Chicken/Dropper
        Texture2D chicken;
        Rectangle chickenRec;

        //Whitelines
        Texture2D whiteline1;
        Rectangle whitelineRec1;
        Rectangle whitelineRec2;

        //Nests
        Texture2D nest;
        Rectangle[] nestRec = new Rectangle[3];

        //Mouse and Keyboard
        KeyboardState kb;
        MouseState mouse;

        //Fonts, and Texts Locations
        SpriteFont text;
        SpriteFont scoreText;
        Vector2 scoreLoc = new Vector2(700, 35);
        Vector2 levelLoc = new Vector2(100, 35);
        Vector2 EndscoreLoc = new Vector2(50, 40);
        Vector2 EndTextLoc = new Vector2(50, 525);

        //Eggs/Droppings
        Texture2D egg1;
        Rectangle[] egg1Rec = new Rectangle[5];
        Rectangle[] egg2Rec = new Rectangle[5];

        //Random chicken movement
        Random movement = new Random();
        bool walkDirect;
        bool chickenMovement = false;

        //Round bools
        bool Round1 = true;
        bool R1eggFall = false;
        bool R1egg2Fall = false;

        bool Round2 = false;
        bool R2eggFall = false;
        bool R2egg2Fall = false;

        bool Round3 = false;
        bool R3eggFall = false;
        bool R3egg2Fall = false;

        bool Round4 = false;
        bool R4eggFall = false;
        bool R4egg2Fall = false;

        //Determines number of eggs dropping
        int droppedEggs = 10;

        //Speeds
        float xSpeed1;
        float ySpeed1;
        //Speed Increase;
        float ySpeedInc;

        //Controls # of nests visible
        int NestLife = 0;

        //Menu Screen and Ending Screen
        bool isMenuScreen = true;
        bool isEndScreen = false;
        bool isStartButton = true;

        //Egg splat image and bool
        bool isSplatting = false;
        Texture2D eggSplat;
        Rectangle eggSplatRec;

        //Start Button
        Texture2D startButton;
        Rectangle startButtonRec;

        //Value of score and level;
        int score;
        int level;
        
        //Constants
        const int LeftBorder = 50;
        const int RightBorder = 920;
        const int TopBorder = 25;
        const int BottomBorder = 630;
        const int Midpoint = 320;
        const int Endpoint = 635;
        //Egg position in the center if the chicken
        const int chickenCenterX = 50;
        const int chickenCenterY = 70;

        //Audio, songs and effects
        SoundEffect rooster;
        SoundEffectInstance roosterInst;
        SoundEffect chickenBuck;
        SoundEffectInstance chickenBuckInst;
        Song CountryMusic;
        
        //Allows time change
        double totalTime;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Sets the screen size
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1100;
            graphics.ApplyChanges();

            //Sets the speeds
            xSpeed1 = 5.0f;
            ySpeed1 = 5.0f;
            ySpeedInc = 2.5f;

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Width and Height variables for display
            int width = (this.graphics.GraphicsDevice.Viewport.Width);
            int height = (this.graphics.GraphicsDevice.Viewport.Height);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Loading different backgrounds: Menu, Main, End
            MenuScreen = Content.Load<Texture2D>(@"Images\Backgrounds\KaboomMenuScreen");
            MenuScreenRec = new Rectangle(0, 0, (int)(width), (int)(height));
            background = Content.Load<Texture2D>(@"Images\Backgrounds\KaboomBackground");
            backgroundRec = new Rectangle(0, 0, (int)(width), (int)(height));
            EndScreen = Content.Load<Texture2D>(@"Images\Backgrounds\GameOverScreen");
            EndScreenRec = new Rectangle(0, 0, (int)(width), (int)(height));

            //Loads chicken/dropper image
            chicken = Content.Load<Texture2D>(@"Images\Pictures\FunnyChicken");
            chickenRec = new Rectangle(550, 25, (int)(width * 0.12), (int)(height * 0.20));

            //Loads the egg splat, when egg is dropped
            eggSplat = Content.Load<Texture2D>(@"Images\Pictures\EggSplat");
            eggSplatRec = new Rectangle(100, 70, (int)(width * 0.8), (int)(height * 0.8));

            //Loads identical image and location for 3 nests
            nest = Content.Load<Texture2D>(@"Images\Pictures\Nest");
            nestRec [0] = new Rectangle(200, 400, (int)(width * 0.12), (int)(height * 0.15));
            nestRec [1] = new Rectangle(200, 440, (int)(width * 0.12), (int)(height * 0.15));
            nestRec [2] = new Rectangle(200, 480, (int)(width * 0.12), (int)(height * 0.15));

            //Loads identical image and location for 2 whitelines
            whiteline1 = Content.Load<Texture2D>(@"Images\Pictures\WhiteLine");
            whitelineRec1 = new Rectangle(10, 270, (int)(width * 0.040), (int)(height * 0.005));
            whitelineRec2 = new Rectangle(1050, 270, (int)(width * 0.040), (int)(height * 0.005));

            //Loads image for start button
            startButton = Content.Load<Texture2D>(@"Images\Pictures\StartButton");
            startButtonRec = new Rectangle(60,520,(int)(width * 0.15),(int)(height * 0.15));

            //Loads text fonts for in game text, and game over text.
            text = Content.Load<SpriteFont>(@"Fonts\Text");
            scoreText = Content.Load<SpriteFont>(@"Fonts\scoreText");

            //Loads all audio, such as songs and effects
            rooster = Content.Load<SoundEffect>(@"Audio\Sound Effect\RoosterMorningCall");
            roosterInst = rooster.CreateInstance();
            chickenBuck = Content.Load<SoundEffect>(@"Audio\Sound Effect\ChickenBuck");
            chickenBuckInst = chickenBuck.CreateInstance();
            CountryMusic = Content.Load<Song>(@"Audio\Songs\CountryMusic");
            MediaPlayer.IsRepeating = true;

            //Loads the image of the eggs and loops all the rectangles used in the game
            egg1 = Content.Load<Texture2D>(@"Images\Pictures\WhiteEgg");

            for (int e = 0; e < egg1Rec.Length; ++e)
            {
                egg1Rec[e] = new Rectangle(chickenRec.X + 25, chickenRec.Y + 50, 30, 30);

            }

            for (int e = 0; e < egg2Rec.Length; ++e)
            {
                egg2Rec[e] = new Rectangle(chickenRec.X + 25, chickenRec.Y + 50, 30, 30);
            }

        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Sets keboard and mouse states
            kb = Keyboard.GetState();
            mouse = Mouse.GetState();

           //Allows the time to be reduced between each direction change
           totalTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (totalTime >= 2)
            {
                walkDirect = movement.Next(2) == 0;
                totalTime = 0;
            }

            Console.WriteLine(totalTime);

            // Allows the game to exit
            if (kb.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            //When "N" key is allows new game to begin
            if (kb.IsKeyDown(Keys.N))
            {
                Round1 = true;
                Round2 = false;
                Round3 = false;
                Round4 = false;
                score = 0;
                chickenMovement = false;
                R1eggFall = false;
                R1egg2Fall = false;
                R2eggFall = false;
                R2egg2Fall = false;
                R3eggFall = false;
                R3egg2Fall = false;
                R4eggFall = false;
                R4egg2Fall = false;
                NestLife = 0;
                isSplatting = false;
                //Sets the location of Nests and Chicken to original position when loaded
                nestRec[2].Y = 480;
                nestRec[1].Y = 440;
                chickenRec.X = 550;
                droppedEggs = 10;
                ySpeed1 = 5.0f;
                isEndScreen = false;
            }
            
            //Subprograms
            PlayerInput();
            ChickenDropMovement();
            Level1();
            Level2();
            Level3();
            Level4();

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //Background Image
            spriteBatch.Draw(background, backgroundRec, Color.White);

            //Start button Subprogram
            StartButtonShow();

            //Draws eggs
            spriteBatch.Draw(egg1, egg1Rec[0], Color.White);
            spriteBatch.Draw(egg1, egg2Rec[0], Color.White);
            spriteBatch.Draw(egg1, egg1Rec[1], Color.White);
            spriteBatch.Draw(egg1, egg2Rec[1], Color.White);
            spriteBatch.Draw(egg1, egg1Rec[2], Color.White);
            spriteBatch.Draw(egg1, egg2Rec[2], Color.White);
            spriteBatch.Draw(egg1, egg1Rec[3], Color.White);
            spriteBatch.Draw(egg1, egg2Rec[3], Color.White);

            //Main images: Chicken, Nests, Lines
            spriteBatch.Draw(chicken, chickenRec, Color.White);
            spriteBatch.Draw(nest, nestRec[2], Color.White);
            spriteBatch.Draw(nest, nestRec[1], Color.White); 
            spriteBatch.Draw(nest, nestRec[0], Color.White);
            spriteBatch.Draw(whiteline1, whitelineRec1, Color.White);
            spriteBatch.Draw(whiteline1, whitelineRec2, Color.White);

            //Outputs the users score and level
            spriteBatch.DrawString(text, " SCORE: " + score, scoreLoc, Color.White);
            spriteBatch.DrawString(text, "Level: " + level, levelLoc, Color.White);

            //Draws main menu and all functions related
            MenuScreenShow();
            //Draws eggsplat, and nest dissapearng
            NestLives();
            //Draws the game over screen
            EndScreenShow();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        //All the user controlled movement
        private void PlayerInput() 
        {
            // 3 bird nest follow the X position of the mouse
            nestRec[0].X = mouse.X;
            nestRec[1].X = mouse.X;
            nestRec[2].X = mouse.X;

            //2 white line follow the Y position of the mouse
            whitelineRec1.Y = mouse.Y;
            whitelineRec2.Y = mouse.Y;

            //Creates barriers on the left and right side
            if (mouse.X < LeftBorder)
            {
                nestRec[0].X = LeftBorder;
                nestRec[1].X = LeftBorder;
                nestRec[2].X = LeftBorder;
            }
            else if (mouse.X > RightBorder)
            {
                nestRec[0].X = RightBorder;
                nestRec[1].X = RightBorder;
                nestRec[2].X = RightBorder;
            }

            //Creates barriers on the top and bottom side
            if (mouse.Y < TopBorder)
            {
                whitelineRec1.Y = TopBorder;
                whitelineRec2.Y = TopBorder;
            }

            else if (mouse.Y > BottomBorder)
            {
                whitelineRec1.Y = BottomBorder;
                whitelineRec2.Y = BottomBorder;
            }
        }

        //All chicken/dropper related movement
        private void ChickenDropMovement()
        {
            //Alows the chicken to move once the button is clicked
             if (mouse.LeftButton == ButtonState.Pressed)
             {
                 if (mouse.X >= startButtonRec.X && mouse.X <= startButtonRec.X + startButtonRec.Width && mouse.Y >= startButtonRec.Y && mouse.Y <= startButtonRec.Y + startButtonRec.Height)
                 {
                     chickenMovement = true;
                     isSplatting = false;
                 }
             }
             
            if (chickenMovement == true)
            {
                //Moves to the Right
                if (walkDirect == true)
                {
                    chickenRec.X += (int)xSpeed1;

                    if (chickenRec.X > RightBorder)
                    {
                        walkDirect = false;
                    }

                }
                // Moves to the left
                else if (walkDirect == false)
                {
                    chickenRec.X -= (int)xSpeed1;

                    if (chickenRec.X < LeftBorder)
                    {
                        walkDirect = true;
                    }
                }
            }
            //Chicken Speed for Round1
            if (Round1 == true)
            {
                xSpeed1 = 5.0f;
            }

            //Chicken Speed for Round2
            if (Round2 == true)
            {
                xSpeed1 = 8.0f;
            }

            //Chicken Speed for Round3
            if (Round3 == true)
            {
                xSpeed1 = 11.0f;
            }

            //Chicken Speed for Round4;
            if (Round4 == true)
            {
                xSpeed1 = 14.0f;
            } 
        }

        //ALL GAMEPLAY INVOLVING ROUND 1
        private void Level1()
        {
                // Sets location of the egg 1 behind the chicken
                if (R1eggFall == false)
                {
                    egg1Rec[0].X = chickenRec.X + 50;
                    egg1Rec[0].Y = chickenRec.Y + 70;

                }
                // Sets location of the egg 2 behind the chicken
                if (R1egg2Fall == false)
                {
                    egg2Rec[0].X = chickenRec.X + 50;
                    egg2Rec[0].Y = chickenRec.Y + 70;
                }
                if (Round1 == true)
                {
                    level = 1;
                    //Starts the dropping of egg 1 when start is clicked
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        //if (mouse.X >= startButtonRec.X && mouse.X <= startButtonRec.X + startButtonRec.Width && mouse.Y >= startButtonRec.Y && mouse.Y <= startButtonRec.Y + startButtonRec.Height)
                        //{
                        R1eggFall = true;
                            
                        //}
                    }

                    if (R1eggFall == true)
                    {

                        egg1Rec[0].Y += (int)ySpeed1;
                    }

                    // Starts the dropping of egg 2
                    if (egg1Rec[0].Y >= Midpoint)
                    {
                        R1egg2Fall = true;
                    }

                    if (R1egg2Fall == true)
                    {
                        egg2Rec[0].Y += (int)ySpeed1;
                    }

                    //Collision detection of the 1st egg and the nests
                    if (egg1Rec[0].X >= nestRec[0].X && egg1Rec[0].X <= nestRec[0].X + nestRec[0].Width && egg1Rec[0].Y >= nestRec[0].Y && egg1Rec[0].Y <= nestRec[0].Y + nestRec[0].Height ||
                        egg1Rec[0].X >= nestRec[1].X && egg1Rec[0].X <= nestRec[1].X + nestRec[1].Width && egg1Rec[0].Y >= nestRec[1].Y && egg1Rec[0].Y <= nestRec[1].Y + nestRec[1].Height ||
                        egg1Rec[0].X >= nestRec[2].X && egg1Rec[0].X <= nestRec[2].X + nestRec[2].Width && egg1Rec[0].Y >= nestRec[2].Y && egg1Rec[0].Y <= nestRec[2].Y + nestRec[2].Height)
                    {
                        score = score + 10;
                        R1eggFall = false;
                        droppedEggs--;
                    }

                    //Collision detection of the 2nd egg and the nests
                    if (egg2Rec[0].X >= nestRec[0].X && egg2Rec[0].X <= nestRec[0].X + nestRec[0].Width && egg2Rec[0].Y >= nestRec[0].Y && egg2Rec[0].Y <= nestRec[0].Y + nestRec[0].Height ||
                        egg2Rec[0].X >= nestRec[1].X && egg2Rec[0].X <= nestRec[1].X + nestRec[1].Width && egg2Rec[0].Y >= nestRec[1].Y && egg2Rec[0].Y <= nestRec[1].Y + nestRec[1].Height ||
                        egg2Rec[0].X >= nestRec[2].X && egg2Rec[0].X <= nestRec[2].X + nestRec[2].Width && egg2Rec[0].Y >= nestRec[2].Y && egg2Rec[0].Y <= nestRec[2].Y + nestRec[2].Height)
                    {
                        R1egg2Fall = false;
                        R1eggFall = true;
                        score = score + 10;
                        droppedEggs--;
                    }

                    //Allows egg 1 to drop again
                    if (egg2Rec[0].Y >= Midpoint)
                    {
                        R1eggFall = true;
                    }

                    //Allows round 1 to end and round 2 to begin once all the eggs have been dropped
                    if (droppedEggs == 0)
                    {
                        R1eggFall = false;
                        R1egg2Fall = false;
                        chickenMovement = false;
                        Round1 = false;
                        Round2 = true;
                        droppedEggs = 20;
                    }
                }
        }

        //ALL GAMEPLAY INVOLVING ROUND 2
        private void Level2()
        {
            
            // Sets location of the egg 1 behind the chicken
            if (R2eggFall == false)
            {
                egg1Rec[1].X = chickenRec.X + 50;
                egg1Rec[1].Y = chickenRec.Y + 70;

            }
            // Sets location of the egg 2 behind the chicken
            if (R2egg2Fall == false)
            {
                egg2Rec[1].X = chickenRec.X + 50;
                egg2Rec[1].Y = chickenRec.Y + 70;
            }

            if (Round2 == true)
            {
                level = 2;
                //Starts the dropping of egg 1 when start is clicked
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (mouse.X >= startButtonRec.X && mouse.X <= startButtonRec.X + startButtonRec.Width && mouse.Y >= startButtonRec.Y && mouse.Y <= startButtonRec.Y + startButtonRec.Height)
                    {
                        R2eggFall = true;
                    }
                }

                if (R2eggFall == true)
                {

                    egg1Rec[1].Y += (int)(ySpeed1 + ySpeedInc);
                }

                // Starts the dropping of egg 2
                if (egg1Rec[1].Y >= Midpoint)
                {
                    R2egg2Fall = true;
                }

                if (R2egg2Fall == true)
                {
                    egg2Rec[1].Y += (int)(ySpeed1 + ySpeedInc);
                }

                //Collision detection of the 1st egg and the nests
                if (egg1Rec[1].X >= nestRec[0].X && egg1Rec[1].X <= nestRec[0].X + nestRec[0].Width && egg1Rec[1].Y >= nestRec[0].Y && egg1Rec[1].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg1Rec[1].X >= nestRec[1].X && egg1Rec[1].X <= nestRec[1].X + nestRec[1].Width && egg1Rec[1].Y >= nestRec[1].Y && egg1Rec[1].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg1Rec[1].X >= nestRec[2].X && egg1Rec[1].X <= nestRec[2].X + nestRec[2].Width && egg1Rec[1].Y >= nestRec[2].Y && egg1Rec[1].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    score = score + 20;
                    droppedEggs--;
                    R2eggFall = false;
                }

                //Collision detection of the 2nd egg and the nests
                if (egg2Rec[1].X >= nestRec[0].X && egg2Rec[1].X <= nestRec[0].X + nestRec[0].Width && egg2Rec[1].Y >= nestRec[0].Y && egg2Rec[1].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg2Rec[1].X >= nestRec[1].X && egg2Rec[1].X <= nestRec[1].X + nestRec[1].Width && egg2Rec[1].Y >= nestRec[1].Y && egg2Rec[1].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg2Rec[1].X >= nestRec[2].X && egg2Rec[1].X <= nestRec[2].X + nestRec[2].Width && egg2Rec[1].Y >= nestRec[2].Y && egg2Rec[1].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    R2egg2Fall = false;
                    R2eggFall = true;
                    droppedEggs--;
                    score = score + 20;
                }

                //Allows egg 1 to drop again
                if (egg2Rec[1].Y >= Midpoint)
                {
                    R2eggFall = true;
                    
                }

                if (egg1Rec[1].Y >= Endpoint || egg2Rec[1].Y >= Endpoint)
                {
                    R2eggFall = false;
                    R2egg2Fall = false;
                    chickenMovement = false;
                }
               
                if (droppedEggs == 0)
                {
                    R2eggFall = false;
                    R2egg2Fall = false;
                    chickenMovement = false;
                    
                    Round2 = false;
                    Round3 = true;
                    droppedEggs = 30;
                }
            }

        }

        //ALL GAMEPLAY INVOLVING ROUND 3
        private void Level3()
        {
            // Sets location of the egg 1 behind the chicken
            if (R3eggFall == false)
            {
                egg1Rec[2].X = chickenRec.X + 50;
                egg1Rec[2].Y = chickenRec.Y + 70;

            }
            // Sets location of the egg 2 behind the chicken
            if (R3egg2Fall == false)
            {
                egg2Rec[2].X = chickenRec.X + 50;
                egg2Rec[2].Y = chickenRec.Y + 70;
            }

            if (Round3 == true)
            {
                level = 3;
                //Starts the dropping of egg 1 when start is clicked
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (mouse.X >= startButtonRec.X && mouse.X <= startButtonRec.X + startButtonRec.Width && mouse.Y >= startButtonRec.Y && mouse.Y <= startButtonRec.Y + startButtonRec.Height)
                    {
                        R3eggFall = true;
                    }
                }

                if (R3eggFall == true)
                {

                    egg1Rec[2].Y += (int)(ySpeed1 + ySpeedInc * 2);
                    }

                // Starts the dropping of egg 2
                if (egg1Rec[2].Y >= Midpoint)
                {
                    R3egg2Fall = true;
                }

                if (R3egg2Fall == true)
                {
                    egg2Rec[2].Y += (int)(ySpeed1 + ySpeedInc*2);
                }

                //Collision detection of the 1st egg and the nests
                if (egg1Rec[2].X >= nestRec[0].X && egg1Rec[2].X <= nestRec[0].X + nestRec[0].Width && egg1Rec[2].Y >= nestRec[0].Y && egg1Rec[2].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg1Rec[2].X >= nestRec[1].X && egg1Rec[2].X <= nestRec[1].X + nestRec[1].Width && egg1Rec[2].Y >= nestRec[1].Y && egg1Rec[2].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg1Rec[2].X >= nestRec[2].X && egg1Rec[2].X <= nestRec[2].X + nestRec[2].Width && egg1Rec[2].Y >= nestRec[2].Y && egg1Rec[2].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    score = score + 30;
                    droppedEggs--;
                    R3eggFall = false;
                }

                //Collision detection of the 2nd egg and the nests
                if (egg2Rec[2].X >= nestRec[0].X && egg2Rec[2].X <= nestRec[0].X + nestRec[0].Width && egg2Rec[2].Y >= nestRec[0].Y && egg2Rec[2].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg2Rec[2].X >= nestRec[1].X && egg2Rec[2].X <= nestRec[1].X + nestRec[1].Width && egg2Rec[2].Y >= nestRec[1].Y && egg2Rec[2].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg2Rec[2].X >= nestRec[2].X && egg2Rec[2].X <= nestRec[2].X + nestRec[2].Width && egg2Rec[2].Y >= nestRec[2].Y && egg2Rec[2].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    R3egg2Fall = false;
                    R3eggFall = true;
                    droppedEggs--;
                    score = score + 30;
                }
                //Allows egg 1 to drop again
                if (egg2Rec[2].Y >= Midpoint)
                {
                    R3eggFall = true;
                }

                if (droppedEggs == 0)
                {
                    R3eggFall = false;
                    R3egg2Fall = false;
                    chickenMovement = false;
                    Round3 = false;
                    Round4 = true;
                }
            }
        }

        //ALL GAMEPLAY INVOLVING ROUND 4
        private void Level4()
        {
            // Sets location of the egg 1 behind the chicken
            if (R4eggFall == false)
            {
                egg1Rec[3].X = chickenRec.X + 50;
                egg1Rec[3].Y = chickenRec.Y + 70;

            }
            // Sets location of the egg 2 behind the chicken
            if (R4egg2Fall == false)
            {
                egg2Rec[3].X = chickenRec.X + 50;
                egg2Rec[3].Y = chickenRec.Y + 70;
            }

            if (Round4 == true)
            {
                level = 4;
                //Starts the dropping of egg 1
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (mouse.X >= startButtonRec.X && mouse.X <= startButtonRec.X + startButtonRec.Width && mouse.Y >= startButtonRec.Y && mouse.Y <= startButtonRec.Y + startButtonRec.Height)
                    {
                        R4eggFall = true;
                    }
                }

                if (R4eggFall == true)
                {

                    egg1Rec[3].Y += (int)(ySpeed1 + ySpeedInc*3);
                }

                // Starts the dropping of egg 2
                if (egg1Rec[3].Y >= Midpoint)
                {
                    R4egg2Fall = true;
                }

                if (R4egg2Fall == true)
                {
                    egg2Rec[3].Y += (int)(ySpeed1 + ySpeedInc*3);
                }

                //Collision detection of the 1st egg and the nests
                if (egg1Rec[3].X >= nestRec[0].X && egg1Rec[3].X <= nestRec[0].X + nestRec[0].Width && egg1Rec[3].Y >= nestRec[0].Y && egg1Rec[3].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg1Rec[3].X >= nestRec[1].X && egg1Rec[3].X <= nestRec[1].X + nestRec[1].Width && egg1Rec[3].Y >= nestRec[1].Y && egg1Rec[3].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg1Rec[3].X >= nestRec[2].X && egg1Rec[3].X <= nestRec[2].X + nestRec[2].Width && egg1Rec[3].Y >= nestRec[2].Y && egg1Rec[3].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    score = score + 40;
                    R4eggFall = false;
                }

                //Collision detection of the 2nd egg and the nests
                if (egg2Rec[3].X >= nestRec[0].X && egg2Rec[3].X <= nestRec[0].X + nestRec[0].Width && egg2Rec[3].Y >= nestRec[0].Y && egg2Rec[3].Y <= nestRec[0].Y + nestRec[0].Height ||
                    egg2Rec[3].X >= nestRec[1].X && egg2Rec[3].X <= nestRec[1].X + nestRec[1].Width && egg2Rec[3].Y >= nestRec[1].Y && egg2Rec[3].Y <= nestRec[1].Y + nestRec[1].Height ||
                    egg2Rec[3].X >= nestRec[2].X && egg2Rec[3].X <= nestRec[2].X + nestRec[2].Width && egg2Rec[3].Y >= nestRec[2].Y && egg2Rec[3].Y <= nestRec[2].Y + nestRec[2].Height)
                {
                    R4egg2Fall = false;
                    R4eggFall = true;
                    score = score + 40;
                }

                //Allows egg 1 to drop again
                if (egg2Rec[3].Y >= Midpoint)
                {
                    R4eggFall = true;
                }
                //Allows speeds to get faster and faster as score increases
                if (score == 3000)
                {
                    xSpeed1 = 17.0f;
                    ySpeed1 = 7.5f;
                }
                if (score == 4500)
                {
                    xSpeed1 = 20.0f;
                    ySpeed1 = 10.0f;
                }
                if (score == 5250)
                {
                    xSpeed1 = 21.5f;
                    ySpeed1 = 11.25f;
                }
                if (score == 6000)
                {
                    xSpeed1 = 23.0f;
                    ySpeed1 = 12.5f;
                }
                if (score == 10000)
                {
                    xSpeed1 = 28.0f;
                    ySpeed1 = 15.0f;
                }
            }
        }
         
        private void NestLives()
        {
            //Round 1 Egg Break
            if (egg1Rec[0].Y >= Endpoint || egg2Rec[0].Y >= Endpoint)
            {
                //Pauses game
                R1eggFall = false;
                R1egg2Fall = false;
                chickenMovement = false;

                //5 more eggs drop
                droppedEggs = 5;
                //One nest dissapears
                NestLife++;
                //Egg splat appears
                isSplatting = true;
                //Chicken Bucking sound effect
                chickenBuckInst.Play();
            }
           
            //Round 2 Egg Break
            if (egg1Rec[1].Y >= Endpoint || egg2Rec[1].Y >= Endpoint)
            {
                //Pauses Game
                R2eggFall = false;
                R2egg2Fall = false;
                chickenMovement = false;
                //5 more eggs are dropped
                droppedEggs = 5;
                NestLife++;
                isSplatting = true;
                chickenBuckInst.Play();
            }
            
            //Round 3 Egg Break
            if (egg1Rec[2].Y >= Endpoint || egg2Rec[2].Y >= Endpoint)
            {
                //Pauses game
                R3eggFall = false;
                R3egg2Fall = false;
                chickenMovement = false;

                isSplatting = true;
                NestLife++;
                chickenBuckInst.Play();
            }
            if (egg1Rec[2].Y >= Endpoint && NestLife == 1 || egg2Rec[2].Y >= Endpoint && NestLife == 1)
            {
                //10 more eggs drop 1st time egg breaks
                droppedEggs = 10;
            }
            if (egg1Rec[2].Y >= Endpoint && NestLife == 2 || egg2Rec[2].Y >= Endpoint && NestLife == 2)
            {
                //5 more eggs drop 2nd time egg breaks
                droppedEggs = 5;
            }
            
            //Round 4 Egg break
            if (egg1Rec[3].Y >= Endpoint || egg2Rec[3].Y >= Endpoint)
            {
                //Pauses game
                R4eggFall = false;
                R4egg2Fall = false;
                chickenMovement = false;

                isSplatting = true;
                NestLife++;
                chickenBuckInst.Play();
            }
            //Sets nest 3 to nest 1 position
            if (NestLife == 1)
            {
                nestRec[2].Y = nestRec[0].Y;
            }
            //Sets nest 2 to nest 1 position
            if (NestLife == 2)
            {
                nestRec[1].Y = nestRec[0].Y;
            }
            //When egg falls for the third time ends game, end screen is shown
            if (NestLife == 3)
            {
                Round1 = false;
                Round2 = false;
                Round3 = false;
                Round4 = false;

                isEndScreen = true;
            }

            //Shows egg splat when egg hits bottom
            if (isSplatting == true)
            {
                spriteBatch.Draw(eggSplat, eggSplatRec, Color.White);
            }
        }

        private void MenuScreenShow()
        {
            if (isMenuScreen == true)
            {
                //Allows Menu screen to be drawn
                spriteBatch.Draw(MenuScreen, MenuScreenRec, Color.White);
                //While Menu screen is aNctive all gameplay is disabled
                Round1 = false;
                Round2 = false;
                Round3 = false;
                Round4 = false;
                chickenMovement = false;
                R1eggFall = false;
                R1egg2Fall = false;
                R2eggFall = false;
                R2egg2Fall = false;
                R3eggFall = false;
                R3egg2Fall = false;
                R4eggFall = false;
                R4egg2Fall = false;

                if (mouse.LeftButton == ButtonState.Pressed && mouse.X >= 1030 && mouse.X <= 1100 && mouse.Y >= 620 && mouse.Y <= 700)
                {
                    //Removes Menu screen
                    isMenuScreen = false;
                    
                    //When Menuscreen is removed a new game begins, same functions as the "N" button
                    Round1 = true;
                    Round2 = false;
                    Round3 = false;
                    Round4 = false;
                    score = 0;
                    chickenMovement = false;
                    R1eggFall = false;
                    R1egg2Fall = false;
                    R2eggFall = false;
                    R2egg2Fall = false;
                    R3eggFall = false;
                    R3egg2Fall = false;
                    R4eggFall = false;
                    R4egg2Fall = false;
                    NestLife = 0;
                    isSplatting = false;
                    isEndScreen = false;
                    //Sets the location of Nests and Chicken to original position when loaded
                    nestRec[2].Y = 480;
                    nestRec[1].Y = 440;
                    chickenRec.X = 550;
                    droppedEggs = 10;

                    //Allows sound effect and main music to be played when gameplay begins
                    roosterInst.Play();
                    if (!(MediaPlayer.State == MediaState.Playing))
                    {
                        MediaPlayer.Play(CountryMusic);
                    }

                }
            }
        }

        private void EndScreenShow()
        {
            //Draws end screen when game is over
            if (isEndScreen == true)
            {
                spriteBatch.Draw(EndScreen, EndScreenRec, Color.White);
                spriteBatch.DrawString(scoreText, "Your Final Score is: " + score, EndscoreLoc, Color.SkyBlue);
                spriteBatch.DrawString(scoreText, "Press \"N\" \nto start a new game", EndTextLoc, Color.White);
            }

        }
        private void StartButtonShow()
        {
            //Start button dissapears when gameplay begins
            if (chickenMovement == true)
            {
                isStartButton = false;
            }
            //Start button appears when gameplay stops
            if (chickenMovement == false)
            {
                isStartButton = true;
            } 
            //Draws start button
            if (isStartButton == true)
            {
                spriteBatch.Draw(startButton, startButtonRec, Color.White);
            }

        }
    }
}
