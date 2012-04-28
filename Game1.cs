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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background, meteoro, nave;
        Vector2 posicaoNave;
        
        int largura, altura, vidas;
        const int deslocaNave = 8;
        const int deslocaMeteoro = 8;

        float veloCriaMeteoro = 0.035f;
        Random random = new Random();

        List<Vector2> listaPosicaoMeteoro = new List<Vector2>();

        SpriteFont fontePlacar;

        SoundEffect soundColisao;

        bool colisao = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 640;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "Título do Jogo";
            fontePlacar = Content.Load<SpriteFont>("SpriteFont1");
            soundColisao = Content.Load<SoundEffect>("Lixeira do Windows");
            MediaPlayer.IsRepeating = true;

            base.Initialize();

            posicaoNave.X = (Window.ClientBounds.Width - nave.Width) / 2;
            posicaoNave.Y = Window.ClientBounds.Height - nave.Height;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Spacebackground");
            nave = Content.Load<Texture2D>("nave");
            meteoro = Content.Load<Texture2D>("meteoro");
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
            altura = Window.ClientBounds.Height;
            largura = Window.ClientBounds.Width;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState teclado = Keyboard.GetState();
            if (teclado.IsKeyDown(Keys.Left))
            {
                posicaoNave.X -= deslocaNave;
                /*if (posicaoNave.X < 0)
                {
                    posicaoNave.X = 0;
                }*/
            }

            if (teclado.IsKeyDown(Keys.Right))
            {
                posicaoNave.X += deslocaNave;
                /*if (posicaoNave.X > Window.ClientBounds.Width - nave.Width)
                {
                    posicaoNave.X = Window.ClientBounds.Width - nave.Width;
                }*/
            }

            if (teclado.IsKeyDown(Keys.Up))
            {
                posicaoNave.Y -= deslocaNave;
                /*if (posicaoNave.Y < 0)
                {
                    posicaoNave.Y = 0;
                }*/
            }

            if (teclado.IsKeyDown(Keys.Down))
            {
                posicaoNave.Y += deslocaNave;
                /*if (posicaoNave.Y > Window.ClientBounds.Height-nave.Height)
                {
                    posicaoNave.Y = Window.ClientBounds.Height - nave.Height;
                }*/
            }

            posicaoNave.X = MathHelper.Clamp(posicaoNave.X, 0, Window.ClientBounds.Width - nave.Width);
            posicaoNave.Y = MathHelper.Clamp(posicaoNave.Y, 0, Window.ClientBounds.Height - nave.Height);

            if(random.NextDouble() < veloCriaMeteoro)
            {
                float x = (float)random.NextDouble() * (Window.ClientBounds.Width - meteoro.Width);
                listaPosicaoMeteoro.Add(new Vector2(x,-meteoro.Height));
            }

            Rectangle retanguloNave = new Rectangle((int)posicaoNave.X, (int)posicaoNave.Y, nave.Width, nave.Height);

            colisao = false;

            for (int i = 0; i < listaPosicaoMeteoro.Count; i++)
            {
                listaPosicaoMeteoro[i] = new Vector2(listaPosicaoMeteoro[i].X, listaPosicaoMeteoro[i].Y + deslocaMeteoro);

                Rectangle retanguloMeteoro = new Rectangle((int)listaPosicaoMeteoro[i].X,
                    (int)listaPosicaoMeteoro[i].Y,meteoro.Width,meteoro.Height);

                if (retanguloNave.Intersects(retanguloMeteoro))
                {
                    colisao = true;
                    vidas--;
                    soundColisao.Play();
                }

                if (listaPosicaoMeteoro[i].Y > Window.ClientBounds.Height)
                {
                    listaPosicaoMeteoro.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (colisao)
            {
                GraphicsDevice.Clear(Color.Red);
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            //spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
            //spriteBatch.Draw(background, new Rectangle(0,0,largura,altura), Color.White);
            
            //spriteBatch.Draw(meteoro, new Rectangle(100,100,meteoro.Width,meteoro.Height), Color.White);
            //spriteBatch.Draw(nave, new Rectangle(100,180, nave.Width, nave.Height), Color.White);
            
            //spriteBatch.Draw(meteoro, Vector2.Zero, Color.White);
            spriteBatch.Draw(nave, posicaoNave, Color.White);

            foreach ( Vector2 posicaoTextura in listaPosicaoMeteoro)
            {
                spriteBatch.Draw(meteoro, posicaoTextura, Color.White);
            }

            spriteBatch.DrawString(fontePlacar, "Life: " + vidas, Vector2.Zero,Color.Black);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
