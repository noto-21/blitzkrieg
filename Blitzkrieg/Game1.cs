//Zachary Lowrie :), Noah Toma, Owen Woolley
//November 10th, 2020
//ICS 4U

///0000   0    000  00000  00000  0  0  000   000  0000   000
///0   0  0     0     0       0   0 0   0  0   0   0     0
///0000   0     0     0      0    00    000    0   000   0  00
///0   0  0     0     0     0     0 0   0 0    0   0     0   0 
///0000   000  000    0    00000  0  0  0  0  000  0000   000
 
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

namespace Blitzkrieg
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        //obstacle variables
        static List<Obstacle> Obstacles = new List<Obstacle>();
        Texture2D blockSprite;

        //gamepad array variable
        GamePadState[] gamepad = new GamePadState[4];
        GamePadState[] oldgamepad = new GamePadState[4];

        //tank array variables
        bool[] playerAlive = new bool[4];
        Tank[] player = new Tank[4];
        Texture2D[] currentTankSprite = new Texture2D[4];
        Texture2D tankUp;
        Texture2D tankDown;
        Texture2D tankLeft;
        Texture2D tankRight;

        //Bullet variables
        List<Projectile>[] bullets = new List<Projectile>[4];
        int[] currentDirection = new int[4];
        Projectile defaultbullet;

        //Menu variables***
        bool gameStart = false;
        Message title;
        Message start;
        Message musicChoice;
        Message volumeChange;
        Message colourChange;
        SpriteFont titleFont;
        SpriteFont messageFont;
        byte musicNow = 0;
        byte colourNow = 0;

        //Power up variables***
        List<PowerUp> powerUp = new List<PowerUp>();
        Random rand = new Random();
        byte powerUpType;
        byte helpTank;

        //Sound and Music variables***
        float volume = 1;
        Song[] song;
        Song mainSong;

        //Winning message variable
        Message winner;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            //sets the height of the window to 800
            graphics.PreferredBackBufferHeight = 1000;
            //sets the width of the window to 1200
            graphics.PreferredBackBufferWidth = 1200;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //picture variables assignment 
            blockSprite = Content.Load<Texture2D>("Square_Li");
            tankDown = Content.Load<Texture2D>("TankDown");
            tankLeft = Content.Load<Texture2D>("TankLeft");
            tankRight = Content.Load<Texture2D>("TankRight");
            tankUp = Content.Load<Texture2D>("TankUp");

            //Load the fonts into the spriteFont variables
            titleFont = Content.Load<SpriteFont>("titleFont");
            messageFont = Content.Load<SpriteFont>("normalFont");

            //constructing the object list for loop
            for (int i = 0; i < 214; i++)
            {
                //adds new obstacles to the list with the parameters entered
                Obstacles.Add(new Obstacle(new Rectangle(0, 0, 40, 40), blockSprite, Color.White, 100));
            }

            //sets each tanks starting sprite (which way they start facing)
            currentTankSprite[0] = tankDown;
            currentTankSprite[1] = tankDown;
            currentTankSprite[2] = tankUp;
            currentTankSprite[3] = tankUp;

            //constructs all the tanks individually and sets the to their starting corners
            player[0] = new Tank(new Rectangle(0, 0, 40, 40), currentTankSprite[0], Color.LightBlue, 4, 50, 10);
            player[1] = new Tank(new Rectangle(GraphicsDevice.Viewport.Width - 40, 0, 40, 40), currentTankSprite[1], Color.LightGreen, 4, 50, 10);
            player[2] = new Tank(new Rectangle(0, GraphicsDevice.Viewport.Height - 40, 40, 40), currentTankSprite[2], Color.MediumPurple, 4, 50, 10);
            player[3] = new Tank(new Rectangle(GraphicsDevice.Viewport.Width - 40, GraphicsDevice.Viewport.Height-40, 40, 40), currentTankSprite[3], Color.PaleVioletRed, 4, 50, 10);

            //makes a loop to set all the players to alive at the beginning of the game
            for (int i = 0; i < playerAlive.Length; i++)
            {
                playerAlive[i] = true;
            }

            //defines the lists inside the bullet array
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = new List<Projectile>();
            }

            //Construct power ups that will be contained in the power up list in a for loop***
            for (int i = 0; i < 6; i++)
            {
                powerUpType = Convert.ToByte(rand.Next(1, 4));
                helpTank = Convert.ToByte(rand.Next(1, 3));

                //Determine what kind of power up it is
                if (powerUpType == 1)
                {
                    //Determine if it will hurt or help players
                    if (helpTank == 1)
                        //Add a shrink power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("sizePow"), new Rectangle(0, 0, 40, 40), Color.Red, powerUpType, true));
                    else if (helpTank == 2)
                        //Add a grow power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("sizePow"), new Rectangle(0, 0, 40, 40), Color.Blue, powerUpType, false));
                }
                else if (powerUpType == 2)
                {
                    //Determine if it will hurt or help players
                    if (helpTank == 1)
                        //Add a speed power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("speedPow"), new Rectangle(0, 0, 40, 40), Color.Red, powerUpType, true));
                    else if (helpTank == 2)
                        //Add a slow power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("speedPow"), new Rectangle(0, 0, 40, 40), Color.Blue, powerUpType, false));
                }
                else if (powerUpType == 3)
                {
                    //Determine if it will hurt or help players
                    if (helpTank == 1)
                        //Add a heal power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("healthPow"), new Rectangle(0, 0, 40, 40), Color.Red, powerUpType, true));
                    else if (helpTank == 2)
                        //Add a hurt power up
                        powerUp.Add(new PowerUp(Content.Load<Texture2D>("healthPow"), new Rectangle(0, 0, 40, 40), Color.Blue, powerUpType, false));
                }
            }

            //calls the map set up method to move all the rectangles to their specified positions
            MapSetUp();

            //Construct all of the message variables for the menu***
            title = new Message("BLITZKRIEG", new Vector2((1200 / 2) - (titleFont.MeasureString("BLITZKRIEG").X / 2), 100), titleFont, Color.Black, false);
            start = new Message("Start Game", new Vector2((1200 / 2) - (messageFont.MeasureString("Start Game").X / 2), 300), messageFont, Color.DarkRed, true);
            musicChoice = new Message("Music: Track 1", new Vector2((1200 / 2) - (messageFont.MeasureString("Music: Track 1").X / 2), 350), messageFont, Color.Black, false);
            volumeChange = new Message("Volume 100%", new Vector2((1200 / 2) - (messageFont.MeasureString("Volume 100%").X / 2), 400), messageFont, Color.Black, false);
            colourChange = new Message("Tank Colour: Set 1", new Vector2((1200 / 2) - (messageFont.MeasureString("Tank Colour: Set 1").X / 2), 450), messageFont, Color.Black, false);

            //Construct the winner message***
            winner = new Message("You Are Winner", new Vector2((1200 / 2) - (titleFont.MeasureString("You Are Winner").X / 2), (1000 / 2) - (titleFont.MeasureString("You Are Winner").Y / 2)), titleFont, Color.Green, false);

            //Set up and load the mp3 files into song array***
            song = new Song[3];
            song[0] = Content.Load<Song>("GameMusic1");
            song[1] = Content.Load<Song>("GameMusic2");
            song[2] = Content.Load<Song>("GameMusic3");
            mainSong = song[0];

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            //makes a variable that holds the button states of the keyboard so we can check which keys are pressed
            KeyboardState kb = Keyboard.GetState();

            //--------------------------------REFRESHES THE GAMEPADS CODE------------------------------------------------
            //makes gamepad number 1 into a variable that holds its button states so we can check what buttons are pressed
            gamepad[0] = GamePad.GetState(PlayerIndex.One);
            //makes gamepad number 1 into a variable that holds its button states so we can check what buttons are pressed
            gamepad[1] = GamePad.GetState(PlayerIndex.Two);
            //makes gamepad number 1 into a variable that holds its button states so we can check what buttons are pressed
            gamepad[2] = GamePad.GetState(PlayerIndex.Three);
            //makes gamepad number 1 into a variable that holds its button states so we can check what buttons are pressed
            gamepad[3] = GamePad.GetState(PlayerIndex.Four);

            //Check if the game has started***
            if (!gameStart)
            {
                //------------------------------SHIFT BETWEEN OPTIONS CODE-------------------------------------***
                //Check if down on dPad is pressed once on player 1 and colourChange isn't selected
                if (gamepad[0].DPad.Down == ButtonState.Pressed && oldgamepad[0].DPad.Down == ButtonState.Released && colourChange.getSelected() == false)
                {
                    //Check which option is currectly selected
                    if (start.getSelected() == true)
                    {
                        //Set selected for start to false and make the text black
                        start.setSelected(false);
                        start.setColor(Color.Black);
                        //Set selected for musicChoice to true and make the text dark red
                        musicChoice.setSelected(true);
                        musicChoice.setColor(Color.DarkRed);
                    }
                    else if (musicChoice.getSelected() == true)
                    {
                        //Set selected for musicChoice to false and make the text black
                        musicChoice.setSelected(false);
                        musicChoice.setColor(Color.Black);
                        //Set selected for volumeChange to true and make the text dark red
                        volumeChange.setSelected(true);
                        volumeChange.setColor(Color.DarkRed);
                    }
                    else if (volumeChange.getSelected() == true)
                    {
                        //Set selected for volumeChange to false and make the text black
                        volumeChange.setSelected(false);
                        volumeChange.setColor(Color.Black);
                        //Set selected for colourChange to true and make the text dark red
                        colourChange.setSelected(true);
                        colourChange.setColor(Color.DarkRed);
                    }
                }
                //Check if up on dPad is pressed once on player 1 and start isn't selected
                else if (gamepad[0].DPad.Up == ButtonState.Pressed && oldgamepad[0].DPad.Up == ButtonState.Released && start.getSelected() == false)
                {
                    //Check which option is currectly selected
                    if (musicChoice.getSelected() == true)
                    {
                        //Set selected for start to true and make the text dark red
                        start.setSelected(true);
                        start.setColor(Color.DarkRed);
                        //Set selected for musicChoice to false and make the text black
                        musicChoice.setSelected(false);
                        musicChoice.setColor(Color.Black);
                    }
                    else if (volumeChange.getSelected() == true)
                    {
                        //Set selected for musicChoice to true and make the text dark red
                        musicChoice.setSelected(true);
                        musicChoice.setColor(Color.DarkRed);
                        //Set selected for volumeChange to false and make the text black
                        volumeChange.setSelected(false);
                        volumeChange.setColor(Color.Black);
                    }
                    else if (colourChange.getSelected() == true)
                    {
                        //Set selected for volumeChange to true and make the text dark red
                        volumeChange.setSelected(true);
                        volumeChange.setColor(Color.DarkRed);
                        //Set selected for colourChange to false and make the text black
                        colourChange.setSelected(false);
                        colourChange.setColor(Color.Black);
                    }
                }

                //---------------------------SELECTING OPTIONS CODE--------------------------------***
                //Check which option is selected
                if (start.getSelected() == true)
                {
                    //Check if start button is pressed on player 1
                    if (gamepad[0].Buttons.Start == ButtonState.Pressed)
                    {
                        //Play main song
                        MediaPlayer.Play(mainSong);
                        MediaPlayer.Volume = volume;
                        //Change gameStart to true
                        gameStart = true;
                    }
                }
                else if (musicChoice.getSelected() == true)
                {
                    //Check if right or left on dPad is pressed once on player 1
                    if (gamepad[0].DPad.Right == ButtonState.Pressed && oldgamepad[0].DPad.Right == ButtonState.Released || gamepad[0].DPad.Left == ButtonState.Pressed && oldgamepad[0].DPad.Left == ButtonState.Released)
                    {
                        //Check what music is selected currently
                        if (musicNow == 0 && gamepad[0].DPad.Right == ButtonState.Pressed || musicNow == 2 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the music
                            mainSong = song[1];
                            //Set musicNow to 1
                            musicNow = 1;
                            //Change text for musicChoice
                            musicChoice.setText("Music: Track 2");
                        }
                        else if (musicNow == 1 && gamepad[0].DPad.Right == ButtonState.Pressed || musicNow == 0 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the music
                            mainSong = song[2];
                            //Set musicNow to 2
                            musicNow = 2;
                            //Change text for musicChoice
                            musicChoice.setText("Music: Track 3");
                        }
                        else if (musicNow == 2 && gamepad[0].DPad.Right == ButtonState.Pressed || musicNow == 1 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the music
                            mainSong = song[0];
                            //Set musicNow to 0
                            musicNow = 0;
                            //Change text for musicChoice
                            musicChoice.setText("Music: Track 1");
                        }
                    }
                }
                else if (volumeChange.getSelected() == true)
                {
                    //Check if right on dPad is pressed once on player 1 and volume is less than 1
                    if (gamepad[0].DPad.Right == ButtonState.Pressed && oldgamepad[0].DPad.Right == ButtonState.Released)
                        //Increase volume by 0.1
                        volume = volume + 0.1f;
                    //Check if left on dPad is pressed once on player 1 and volume is greater than 0
                    else if (gamepad[0].DPad.Left == ButtonState.Pressed && oldgamepad[0].DPad.Left == ButtonState.Released)
                        //Decrease volume by 0.1
                        volume = volume - 0.1f;

                    //Check if volume is greater than 1 or less than 0
                    if (volume > 1f)
                        volume = 1f;
                    else if (volume < 0f)
                        volume = 0f;
                }
                else if (colourChange.getSelected() == true)
                {
                    //Check if right or left on dPad is pressed once on player 1
                    if (gamepad[0].DPad.Right == ButtonState.Pressed && oldgamepad[0].DPad.Right == ButtonState.Released || gamepad[0].DPad.Left == ButtonState.Pressed && oldgamepad[0].DPad.Left == ButtonState.Released)
                    {
                        //Check what colour is selected currently
                        if (colourNow == 0 && gamepad[0].DPad.Right == ButtonState.Pressed || colourNow == 2 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the colours of all the tanks
                            player[0].SetColor(Color.Blue);
                            player[1].SetColor(Color.Red);
                            player[2].SetColor(Color.Yellow);
                            player[3].SetColor(Color.Green);
                            //Set colourNow to 1
                            colourNow = 1;
                            //Change text for colourChange
                            colourChange.setText("Tank Colour: Set 2");
                        }
                        else if (colourNow == 1 && gamepad[0].DPad.Right == ButtonState.Pressed || colourNow == 0 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the colours of all the tanks
                            player[0].SetColor(Color.White);
                            player[1].SetColor(Color.LightGray);
                            player[2].SetColor(Color.Gray);
                            player[3].SetColor(Color.DarkGray);
                            //Set colourNow to 2
                            colourNow = 2;
                            //Change text for colourChange
                            colourChange.setText("Tank Colour: Set 3");
                        }
                        else if (colourNow == 2 && gamepad[0].DPad.Right == ButtonState.Pressed || colourNow == 1 && gamepad[0].DPad.Left == ButtonState.Pressed)
                        {
                            //Change the colours of all the tanks
                            player[0].SetColor(Color.LightBlue);
                            player[1].SetColor(Color.LightGreen);
                            player[2].SetColor(Color.MediumPurple);
                            player[3].SetColor(Color.PaleVioletRed);
                            //Set colourNow to 0
                            colourNow = 0;
                            //Change text for colourChange
                            colourChange.setText("Tank Colour: Set 1");
                        }
                    }
                }

                //--------------------------CHANGING VOLUME TEXT CODE------------------------------***
                //Check the volume value and change the volumeChange text accordingly
                if (volume == 1f)
                    volumeChange.setText("Volume: 100%");
                else if (volume >= 0.9f)
                    volumeChange.setText("Volume: 90%");
                else if (volume >= 0.8f)
                    volumeChange.setText("Volume: 80%");
                else if (volume >= 0.7f)
                    volumeChange.setText("Volume: 70%");
                else if (volume >= 0.6f)
                    volumeChange.setText("Volume: 60%");
                else if (volume >= 0.5f)
                    volumeChange.setText("Volume: 50%");
                else if (volume >= 0.4f)
                    volumeChange.setText("Volume: 40%");
                else if (volume >= 0.3f)
                    volumeChange.setText("Volume: 30%");
                else if (volume >= 0.2f)
                    volumeChange.setText("Volume: 20%");
                else if (volume >= 0.1f)
                    volumeChange.setText("Volume: 10%");
                else if (volume >= 0f)
                    volumeChange.setText("Volume: 0%");

                //---------------------------CENTERING OPTIONS CODE--------------------------------***
                //Set new vectors for the options to ensure they stay centered
                start.setPosition(new Vector2((1200 / 2) - (messageFont.MeasureString(start.getText()).X / 2), 300));
                musicChoice.setPosition(new Vector2((1200 / 2) - (messageFont.MeasureString(musicChoice.getText()).X / 2), 350));
                volumeChange.setPosition(new Vector2((1200 / 2) - (messageFont.MeasureString(volumeChange.getText()).X / 2), 400));
                colourChange.setPosition(new Vector2((1200 / 2) - (messageFont.MeasureString(colourChange.getText()).X / 2), 450));
            }
            else
            {
                //-------------------------------CALL THE INPUT IF THE GAMEPAD IS CONNECTED AND THE PLAYER IS STILL ALIVE CODE-------------------
                //makes a for loop to run through the gamepads
                for (int i = 0; i < gamepad.Length; i++)
                {
                    //checks if the gamepad is connected
                    if (gamepad[i].IsConnected && playerAlive[i])
                    {
                        //if the gamepad is connected it calls the gamepad input method and gives it the pads current state and the player number
                        GamePadInput(gamepad[i], i);
                    }
                }

                //------------------------------BULLET MOVING CODE------------------------
                //makes a for loop to sort through the bullets
                for (int i = 0; i < bullets.Length; i++)
                {
                    for (int j = 0; j < bullets[i].Count; j++)
                    {
                        bullets[i][j].Move();
                    }
                }

                //=---------------------------HITTEST AND REMOVE METHOD CALL CODE-----------------------

                //Calls the hittest method to set up all possible hit tests
                HitTestActions();

                //calls the remove method to remove dead items
                Remove();



                //----------------------------POWER UP PICK UP CODE----------------------------***
                //Use a for loop to go through all the tanks
                for (int i = 0; i < player.Length; i++)
                {
                    //Use another for loop to go through all the power ups
                    for (int j = 0; j < powerUp.Count; j++)
                    {
                        //Check if player[i] is hitting powerUp[j]
                        if (player[i].HitTest(player[i].GetRectangle(), powerUp[j].GetRectangle()))
                        {
                            //Check the value of helpTank for the power up
                            if (powerUp[j].getHelpTank() == true)
                            {
                                //Call the assist action and give it player[i]
                                powerUp[j].Assist(player[i]);
                            }
                            else if (powerUp[j].getHelpTank() == false)
                            {
                                //Create another for loop to go through all the tanks
                                for (int k = 0; k < player.Length; k++)
                                {
                                    //Check if k is not equal to i
                                    if (k != i)
                                    {
                                        //Call the cripple method and give it player[k]
                                        powerUp[j].Cripple(player[k]);
                                    }
                                }
                            }
                            //remove the power up
                            powerUp.RemoveAt(j);
                            //Subtract 1 from j
                            j--;
                        }
                    }
                }
            }

            //------------------------ASSIGN THE OLDGAMEPAD CODE-----------------------
            //makes a for loop to search through the gamepad array
            for (int i = 0; i < gamepad.Length; i++)
            {
                //sets each oldgamepad equal to each new gamepad
                oldgamepad[i] = gamepad[i];
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Declare an byte variable that hold the amount of players remaining
            byte playersLeft = 0;
            //makes the background color tan
            GraphicsDevice.Clear(Color.Tan);

            //starts the sprite batch
            spriteBatch.Begin();

            //Check if the game has started***
            if (!gameStart)
            {
                //-------------------MESSAGE DRAWING CODE-------------------------***
                //Draw all of the messages for the menu
                title.DrawText(spriteBatch);
                start.DrawText(spriteBatch);
                musicChoice.DrawText(spriteBatch);
                volumeChange.DrawText(spriteBatch);
                colourChange.DrawText(spriteBatch);
            }
            else
            {
                //--------------------OBSTACLE DRAWING CODE-----------------------
                //makes a for loop to count through all the obstacles
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //draws the obstacle to to the screen
                    Obstacles[i].Draw(spriteBatch);
                }

                //--------------------POWER UP DRAWING CODE------------------------***
                for (int i = 0; i < powerUp.Count; i++)
                {
                    //draws the obstacle to to the screen
                    powerUp[i].DrawGameItem(spriteBatch);
                }

                //-------------------BULLET DRAWING CODE---------------------------
                //makes a for loop to sort through the bullets arrays
                for (int i = 0; i < bullets.Length; i++)
                {
                    //makes a for loop to sort through each bullet list
                    for (int j = 0; j < bullets[i].Count; j++)
                    {
                        //draws the bullet onto the screen
                        bullets[i][j].DrawGameItem(spriteBatch);
                    }
                }

                //--------------------PLAYER DRAWING CODE--------------------------
                //makes a for loop to count through the players
                for (int i = 0; i < player.Length; i++)
                {
                    //checks if the gamepad is connected
                    if (gamepad[i].IsConnected)
                        //draws the player onto the screen if they are there
                        player[i].DrawGameItem(spriteBatch);
                }

                //-----------------WIN MESSAGE DRAW CODE---------------------------
                //makes a for loop to count through the players
                for (int i = 0; i < player.Length; i++)
                {
                    //Check if player is both alive and the corresponding controller is connected
                    if (playerAlive[i] == true && gamepad[i].IsConnected)
                    {
                        //Add 1 to playersLeft
                        playersLeft++;
                    }
                }
                //Check to see if only one player is remaining
                if (playersLeft == 1)
                {
                    //sets the winner text to you are the winner
                    winner.setText("You Are Winner!");
                    //calls the draw method to draw the text
                    winner.DrawText(spriteBatch);
                }
                //checks to see if there are no players left
                if (playersLeft == 0)
                {
                    //sets the winner text (in this case the not winner) to german for NO Failures
                    winner.setText("NEIN! FEHLER!");
                    //calls the draw method to draw the text
                    winner.DrawText(spriteBatch);
                }

            }

            //ends the sprite batch
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void GamePadInput(GamePadState pad, int PlayerNumber)
        {
            //--------------------MOVING CODE----------------------------------------------------------------------------------------------------------

            //UP
            //checks if the left thumbstick is upwards with an error allowance of 20%
            if (pad.ThumbSticks.Left.X < 0.20 && pad.ThumbSticks.Left.X > -0.20 && pad.ThumbSticks.Left.Y > 0)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Y > 0)
                    //moves the character up according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if(player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());
                }
                //Assigns the proper direction facing texture
                player[PlayerNumber].SetTexture(tankUp);

            }


            //UP & RIGHT
            //checks if the thumbstick is aimed up and to the right
            if (pad.ThumbSticks.Left.X > 0.20 && pad.ThumbSticks.Left.Y > 0.20)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Y > 0)
                    //moves the character up according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());
                }

                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Right < GraphicsDevice.Viewport.Width)
                    //moves the character right according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());
                }
            }


            //RIGHT
            //checks if the left thumbstick is rightwards with an error allowance of 20%
            if (pad.ThumbSticks.Left.Y < 0.20 && pad.ThumbSticks.Left.Y > -0.20 && pad.ThumbSticks.Left.X > 0)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Right < GraphicsDevice.Viewport.Width)
                    //moves the character right according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());
                }

                //Assigns the proper direction facing texture
                player[PlayerNumber].SetTexture(tankRight);
            }


            //DOWN & RIGHT
            //checks if the thumbstick is aimed down and to the right
            if (pad.ThumbSticks.Left.X > 0.20 && pad.ThumbSticks.Left.Y < -0.20)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Bottom < GraphicsDevice.Viewport.Height)
                    //moves the character down according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());
                }

                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Right < GraphicsDevice.Viewport.Width)
                    //moves the character right according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());
                }
            }

            //DOWN
            //checks if the left thumbstick is downwards with an error allowance of 20%
            if (pad.ThumbSticks.Left.X < 0.20 && pad.ThumbSticks.Left.X > -0.20 && pad.ThumbSticks.Left.Y < 0)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Bottom < GraphicsDevice.Viewport.Height)
                    //moves the character down according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());
                }

                //Assigns the proper direction facing texture
                player[PlayerNumber].SetTexture(tankDown);
            }


            //DOWN & LEFT
            //checks if the thumbstick is aimed down and to the left
            if (pad.ThumbSticks.Left.X < -0.20 && pad.ThumbSticks.Left.Y < -0.20)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Bottom < GraphicsDevice.Viewport.Height)
                    //moves the character down according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());
                }

                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().X > 0)
                    //moves the character left according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());
                }
            }


            //LEFT
            //checks if the left thumbstick is leftwards with an error allowance of 20%
            if (pad.ThumbSticks.Left.Y < 0.20 && pad.ThumbSticks.Left.Y > -0.20 && pad.ThumbSticks.Left.X < 0)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().X > 0)
                    //moves the character left according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());
                }

                //Assigns the proper direction facing texture
                player[PlayerNumber].SetTexture(tankLeft);
            }


            //UP & LEFT
            //checks if the thumbstick is aimed up and to the left
            if (pad.ThumbSticks.Left.X < -0.20 && pad.ThumbSticks.Left.Y > 0.20)
            {
                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().Y > 0)
                    //moves the character up according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Up, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Down, player[PlayerNumber].getSpeed());
                }

                //Only lets the character move in this direction while its entirely on the screen
                if (player[PlayerNumber].GetRectangle().X > 0)
                    //moves the character left according to the speed
                    player[PlayerNumber].Move(MovingGameItem.Direction.Left, player[PlayerNumber].getSpeed());

                //makes a for loop to count through the obstacles to see if moving this way hits any of them
                for (int i = 0; i < Obstacles.Count; i++)
                {
                    //runs a hit test between the player and every obstacles
                    if (player[PlayerNumber].HitTest(player[PlayerNumber].GetRectangle(), Obstacles[i].GetRectangle()))
                        //Moves back from the way it just did, Un-does the previous movement
                        player[PlayerNumber].Move(MovingGameItem.Direction.Right, player[PlayerNumber].getSpeed());
                }
            }


            //---------------------------SHOOTING CODE--------------------------------------------------------------
            //checks what direction the right thumbstick is pointing and sets the currentdirection variable to that direction
            //UP
            //checks if the Right thumbstick is upwards with an error allowance of 20%
            if (pad.ThumbSticks.Right.X < 0.20 && pad.ThumbSticks.Right.X > -0.20 && pad.ThumbSticks.Right.Y > 0)
            {
                //sets the current direction to 0 (Up)
                currentDirection[PlayerNumber] = 0;
            }

            //RIGHT
            //checks if the Right thumbstick is rightwards with an error allowance of 20%
            if (pad.ThumbSticks.Right.Y < 0.20 && pad.ThumbSticks.Right.Y > -0.20 && pad.ThumbSticks.Right.X > 0)
            {
                //sets the current direction to right 1
                currentDirection[PlayerNumber] = 1;
            }

            //DOWN
            //checks if the Right thumbstick is downwards with an error allowance of 20%
            if (pad.ThumbSticks.Right.X < 0.20 && pad.ThumbSticks.Right.X > -0.20 && pad.ThumbSticks.Right.Y < 0)
            {
                //sets the current direction to down
                currentDirection[PlayerNumber] = 2;
            }

            //Right
            //checks if the Right thumbstick is Rightwards with an error allowance of 20%
            if (pad.ThumbSticks.Right.Y < 0.20 && pad.ThumbSticks.Right.Y > -0.20 && pad.ThumbSticks.Right.X < 0)
            {
                //sets the current direction to 3 (Right)
                currentDirection[PlayerNumber] = 3;
            }

            //FIRE           
            //checks if the right trigger is being pressed and wasnt before
            if(gamepad[PlayerNumber].Triggers.Right > 0 && oldgamepad[PlayerNumber].Triggers.Right == 0)
            {
                //makes a bullet and moves it to the character
                bullets[PlayerNumber].Add(new Projectile(new Rectangle(player[PlayerNumber].GetRectangle().X + 15, player[PlayerNumber].GetRectangle().Y + 15,10,10), blockSprite, Color.Black, 5, 10, currentDirection[PlayerNumber]));
            }

        }

        public void HitTestActions()//Controls events when objects intersect with other objects
        {
            //For obstacles and bullets
            //makes a for loop to sort through the bullets array (0-3)
            for (int i = 0; i < bullets.Length; i++)
            {
                //makes a for loop to sort through the bullets[i] lists, all 4 of them
                for (int j = 0; j < bullets[i].Count; j++)
                {
                    //makes a for loop to sort through the obstacles list 
                    for (int m = 0; m < Obstacles.Count; m++)
                    {
                        //checks if the bullet that is specified by the i & j loops hits the obstacle specified by the m loop
                        if (bullets[i][j].HitTest(bullets[i][j].GetRectangle(), Obstacles[m].GetRectangle()))
                        {
                            //removes the bullet damage amount from the obstacles health, so the obstacle takes damage
                            Obstacles[m].SetHealth(Obstacles[m].GetHealth() - bullets[i][j].getDamage());
                            //moves the bullet offscreen
                            bullets[i][j].Move(MovingGameItem.Direction.Up, 1200);
                        }
                    }
                }
            }
            //For tanks and bullets
            //makes a for loop to sort through the bullets array (0-3)
            for (int i = 0; i < bullets.Length; i++)
            {
                //makes a for loop to sort through the bullets[i] lists, all 4 of them
                for (int j = 0; j < bullets[i].Count; j++)
                {
                    //makes a for loop to sort through the players array (0-3)
                    for (int m = 0; m < player.Length; m++)
                    {
                        //checks if the bullets are not from the same tank (so we dont hit test a tank and its own bullets
                        if (i != m)
                        {
                            //checks if the bullet specified by the i & j loops hits the player specified by the m loop
                            if (bullets[i][j].HitTest(bullets[i][j].GetRectangle(), player[m].GetRectangle()))
                            {
                                //takes away the bullet damage from the players health
                                player[m].setHealth(player[m].getHealth() - bullets[i][j].getDamage());
                                //moves the bullet offscreen so the remove function knows to remove it
                                bullets[i][j].Move(MovingGameItem.Direction.Up, 1200);
                            }
                        }
                    }
                }
            }
            //For tanks and tanks
            //makes a for loop to sort through the players array (0-3)
            for (int i = 0; i < player.Length; i++)
            {
                //makes a for loop to sort through the players array (0-3)
                for (int j = 0; j < player.Length; j++)
                {
                    //checks that the tank is not the tank, preventing a hittest with itself
                    if (i != j)
                    {
                        //checks if the 2 players specified by the i & j loops have hit each other
                        if (player[i].HitTest(player[i].GetRectangle(), player[j].GetRectangle()))
                        {
                            //removes the melee damage of the other tank from the first tanks health
                            player[i].setHealth(player[i].getHealth() - player[j].getMeleeDamage());
                        }
                    }
                }
            }
        }

        public void Remove()//Removes objects when needed
        {
            //Removes Obstacles
            //makes a for loop to run through the obstacles list
            for (int i = 0; i < Obstacles.Count; i++)
            {
                //checks if the obstacles health is below zero
                if (Obstacles[i].GetHealth() <= 0)
                {
                    //removes the obstacle that has no health
                    Obstacles.RemoveAt(i);
                    //because the list is now one shorter removes one from the i
                    i--;
                }
            }
            //Removes Bullets
            //makes a for loop to sort through the bullets array (0-3)
            for (int i = 0; i < bullets.Length; i++)
            {
                //makes a for loop to sort through the bullets[i] lists, all 4 of them
                for (int j = 0; j < bullets[i].Count; j++)
                {
                    //checks if the bullets have gone offscreen
                    if (bullets[i][j].GetRectangle().Bottom < 0 || bullets[i][j].GetRectangle().Y > 1000 || bullets[i][j].GetRectangle().X > 1200//+++++++++++++++++++++++++++++
                        || bullets[i][j].GetRectangle().Right < 0)
                    {
                        //removes the bullet that is offscreen
                        bullets[i].RemoveAt(j);
                        //removes one from the i
                        j--;
                    }
                }
            }
            //Removes Tanks
            //makes a for loop to sort through the tanks
            for (int i = 0; i < player.Length; i++)
            {
                //checks if the players health is below zero
                if (player[i].getHealth() <= 0)
                {
                    //moves the player offscreen
                    player[i].Move(MovingGameItem.Direction.Left, 1200);
                    //makes the player alive variable to false which cuts off its ability to receive commands
                    playerAlive[i] = false;
                }
            }

        }


        protected void MapSetUp()
        {
            //makes an int to count through the obstacles while i move them 
            int currentBlock = 0;
            /// moves all the obstacles to the predetermined positions to make up the map, uses the move to function.
            /// increases the current block by one each time so it moves all of the obstacles in order

            Obstacles[currentBlock].MoveTo(0,80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(0, 320);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(0, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(0, 640);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(0, 880);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(40, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(40, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(40, 680);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(80, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 240);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 720);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(80, 960);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(120, 480);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(160, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(160, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(160, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(160, 920);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(200, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(200, 920);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(240, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 400);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 560);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(240, 920);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(280, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(280, 920);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(320, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(320, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(320, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(320, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(320, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(320, 920);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(360, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 320);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 640);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(360, 680);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(400, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 920);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(400, 960);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(440, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 240);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 720);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(440, 960);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(480, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 920);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(480, 960);
            currentBlock++;
            
            Obstacles[currentBlock].MoveTo(520, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(520, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(520, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(520, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(520, 680);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(560, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 120);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 400);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 560);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 840);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(560, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(600, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 120);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 400);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 560);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 840);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(600, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(640, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(640, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(640, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(640, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(640, 680);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(680, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 920);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(680, 960);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(720, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 240);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 720);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(720, 960);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(760, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 680);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 920);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(760, 960);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(800, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 320);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 640);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(800, 680);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(840, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(840, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(840, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(840, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(840, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(840, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(880, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(880, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(920, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 400);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 440);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 520);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 560);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(920, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(960, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 160);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 200);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 360);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 600);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 760);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 800);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(960, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(1000, 40);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1000, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1000, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1000, 920);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(1040, 480);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(1080, 0);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 240);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 720);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 880);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1080, 960);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(1120, 280);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1120, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1120, 680);
            currentBlock++;

            Obstacles[currentBlock].MoveTo(1160, 80);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1160, 320);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1160, 480);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1160, 640);
            currentBlock++;
            Obstacles[currentBlock].MoveTo(1160, 880);
            currentBlock++;
            //       440, 40!!!         440, 920!!!        560, 480!!!        600, 480!!!                720, 40!!!        720, 920!!!
            //Add all of the power ups to the specified points
            powerUp[0].SetRectangle(new Rectangle(440, 40, 40, 40));
            powerUp[1].SetRectangle(new Rectangle(440, 920, 40, 40));
            powerUp[2].SetRectangle(new Rectangle(560, 480, 40, 40));
            powerUp[3].SetRectangle(new Rectangle(600, 480, 40, 40));
            powerUp[4].SetRectangle(new Rectangle(720, 40, 40, 40));
            powerUp[5].SetRectangle(new Rectangle(720, 920, 40, 40));
        }
    }
}
