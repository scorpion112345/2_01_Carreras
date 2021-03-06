﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using TU_NAMESPACE;

namespace _2_01_Carreras
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random randRespawn = new Random();

        //1) Configurar Viewport
        const int ANCHO_VP = 500;
        const int ALTO_VP = 600;

        //2)Configurar escaneario
        const int ANCHO_BG = 500;
        const int ALTO_BG = 3000;
        Texture2D tBg, tBgSelec, tBgInicio, tBgWinLose;
        Rectangle rBg, rBgSelec, rBgInicio, rBgWinLose;

        //PLayer
        Texture2D tCar1, tCar2, tCar3, tCar4;
        Rectangle rCar, rCar1, rCar2, rCar3, rCar4;
        const int ANCHO_CAR = 35;
        const int ALTO_CAR = 70;
        int velCar = 3;

        //Effect
        SoundEffect Fx;

        //songs
        Song SongIntro;
        double TimeGameOver;

        //Enemies
        Texture2D tTaxi, tCamioneta, tCarBlue;
        Rectangle rTaxi1, rTaxi2, rCamioneta, rCarBlue;

        //Fonts
        private SpriteFont font, font2, font3;

        //Banderas
        bool Carro1 = false;
        bool Carro2 = false;
        bool Carro3 = false;
        bool Carro4 = false;
        bool lose = false;
        bool win = false;

        Camera camera;

        //Pantallas
        enum Niveles
        {
            Presentacion,
            Seleccion,
            EnJuego,
            GameOver
        }

        Niveles NivelActual = Niveles.Presentacion;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //3) Adecuar la pantalla al viewport
            graphics.PreferredBackBufferHeight = ALTO_VP;
            graphics.PreferredBackBufferWidth = ANCHO_VP;

            ReSetGame();

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            rBg = new Rectangle(0, -ALTO_BG + ALTO_VP, ANCHO_BG, ALTO_BG);
            rCar = new Rectangle(ANCHO_VP / 2, ALTO_VP / 2, ANCHO_CAR, ALTO_CAR);

            rBgSelec = new Rectangle(0, 0, ANCHO_VP, ALTO_VP);
            rBgInicio = new Rectangle(0, 0, ANCHO_VP, ALTO_VP);
            rBgWinLose = new Rectangle(0, 0, ANCHO_VP, ALTO_VP);


            rCamioneta = new Rectangle((int)(ANCHO_VP * .3), (int)(-ALTO_VP * .3), ANCHO_CAR, ALTO_CAR);
            rTaxi1 = new Rectangle((int)(ANCHO_VP * .65), (int)(-ALTO_VP * .6), ANCHO_CAR, ALTO_CAR);
            rCarBlue = new Rectangle((int)(ANCHO_VP * .40), (int)(-ALTO_VP * .9), ANCHO_CAR, ALTO_CAR);
            rTaxi2 = new Rectangle((int)(ANCHO_VP * .60), -(int)(2 * ALTO_VP * .1), ANCHO_CAR, ALTO_CAR);

            //Seleccion de Carros
            rCar1 = new Rectangle(30, 200, ANCHO_CAR, ALTO_CAR);
            rCar2 = new Rectangle(30, 300, ANCHO_CAR, ALTO_CAR);
            rCar3 = new Rectangle(30, 400, ANCHO_CAR, ALTO_CAR);
            rCar4 = new Rectangle(30, 500, ANCHO_CAR, ALTO_CAR);



            //Inicializar la camara
            camera = new Camera(graphics.GraphicsDevice.Viewport, ANCHO_BG, ALTO_BG - ALTO_VP);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            tBg = Content.Load<Texture2D>("bgPista");
            tBgInicio = Content.Load<Texture2D>("fondo2");
            tBgSelec = Content.Load<Texture2D>("fondospider");
            tBgWinLose = Content.Load<Texture2D>("BgGameWinCar2");

            tCar1 = Content.Load<Texture2D>("CPolicia");
            tCar2 = Content.Load<Texture2D>("CAzul");
            tCar3 = Content.Load<Texture2D>("CDepor");
            tCar4 = Content.Load<Texture2D>("camion");
            tCamioneta = Content.Load<Texture2D>("CCamioneta");
            tTaxi = Content.Load<Texture2D>("CTaxi");
            tCarBlue = Content.Load<Texture2D>("CCarBlue");
            font = Content.Load<SpriteFont>("titulo");
            font2 = Content.Load<SpriteFont>("FGeneral");

            Fx = Content.Load<SoundEffect>("choque");

            SongIntro = Content.Load<Song>("SongIntroCar");

           // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            switch (NivelActual)
            {
                case Niveles.Presentacion:
                    PresentacionUpdate();
                    break;
                case Niveles.Seleccion:
                    SeleccionUpdate();
                    break;
                case Niveles.EnJuego:
                    EnJuegoUpdate(gameTime);
                    break;
                case Niveles.GameOver:
                    GameOverUpdate();
                    break;
            }
            base.Update(gameTime);
        }

        private void PresentacionUpdate()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(SongIntro);
            }

            KeyboardState Kbs = Keyboard.GetState();
            if (Kbs.IsKeyDown(Keys.Space))
            {
                NivelActual = Niveles.Seleccion;
            }
        }

        private void ReSetGame()
        {
            win = false;
            lose = false;
            Carro1 = false;
            Carro2 = false;
            Carro3 = false;
            Carro4 = false;
            rCar.Y = ALTO_VP / 2;
            rCar.X = ANCHO_VP / 2;
            rCamioneta.Y = (int)(-ALTO_VP * .3);
            rCamioneta.X = (int)(ANCHO_VP * .3);
            rTaxi1.Y = (int)(-ALTO_VP * .6);
            rTaxi1.X = (int)(ANCHO_VP * .65);
            rCarBlue.Y = (int)(-ALTO_VP * .9);
            rCarBlue.X = (int)(ANCHO_VP * .40);
            rTaxi2.Y = -(int)(2 * ALTO_VP * .1);
            rTaxi2.X = (int)(ANCHO_VP * .60);
        }

        private void SeleccionUpdate()
        {
            KeyboardState Kbs = Keyboard.GetState();
            if (Kbs.IsKeyDown(Keys.D1))
            {
                Carro1 = true;
            }
            if (Kbs.IsKeyDown(Keys.D2))
            {
                Carro2 = true;
            }
            if (Kbs.IsKeyDown(Keys.D3))
            {
                Carro3 = true;
            }
            if (Kbs.IsKeyDown(Keys.D4))
            {
                Carro4 = true;
            }
            if (Kbs.IsKeyDown(Keys.D1) || Kbs.IsKeyDown(Keys.D2) || Kbs.IsKeyDown(Keys.D3) || Kbs.IsKeyDown(Keys.D4))
            {
                NivelActual = Niveles.EnJuego;
                MediaPlayer.Stop();

            }
        }

        private void EnJuegoUpdate(GameTime gameTime)
        {
            // controles

            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Left) && rCar.X > 100)
                rCar.X -= 3;
            if (kbs.IsKeyDown(Keys.Right) && rCar.X + ANCHO_CAR < 400)
                rCar.X += 3;

            if (kbs.IsKeyDown(Keys.Up))
                rCar.Y -= 1;
            if (kbs.IsKeyDown(Keys.Down))
                velCar = 1;
            else
                velCar = 3;

            rCar.Y -= velCar;

            PosCar(ref rTaxi1);
            PosCar(ref rTaxi2);
            PosCar(ref rCamioneta);
            PosCar(ref rCarBlue);

                //Lose game
                if (rCar.Intersects(rTaxi1) || rCar.Intersects(rTaxi2) || rCar.Intersects(rCamioneta) || rCar.Intersects(rCarBlue))
                {
                    lose = true;
                    Fx.Play();
                    NivelActual = Niveles.GameOver;

                }

                //Win game
                if (rCar.Y <= -2500)
                {
                    win = true;
                    NivelActual = Niveles.GameOver;
                }
                //6)Actualizar la camara
                camera.Update(new Vector2(rCar.X, rCar.Y));

            }// FinEnJUego



            private void PosCar(ref Rectangle coche)
            {
                if (coche.Y > rCar.Y + ALTO_VP / 2 && rCar.Y > -ALTO_BG + ALTO_VP * 2)
                {
                    // Posicionar aleatoriamente en el sig bloque de carretera
                    coche.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP) - ALTO_CAR);
                    coche.X = randRespawn.Next(120, 380 - ANCHO_CAR);
                }
                 if ((coche.Intersects(rTaxi1) || coche.Intersects(rCamioneta) || coche.Intersects(rCarBlue) || coche.Intersects(rTaxi2)) && !coche.Intersects(coche))
                {
                    coche.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP) - ALTO_CAR);
                    coche.X = randRespawn.Next(120, 380 - ANCHO_CAR);
                }
            }

        private void GameOverUpdate()
        {
            KeyboardState Kbs = Keyboard.GetState();
            if (Kbs.IsKeyDown(Keys.Enter))
            {
                NivelActual = Niveles.Presentacion;
                ReSetGame();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Chocolate);

            switch (NivelActual)
            {
                case Niveles.Presentacion:
                    PresentacionDraw(spriteBatch);
                    break;
                case Niveles.Seleccion:
                    SeleccionDraw(spriteBatch);
                    break;
                case Niveles.EnJuego:
                    EnJuegoDraw(spriteBatch);
                    break;
                case Niveles.GameOver:
                    GameOverDraw(spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }

        private void PresentacionDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tBgInicio, rBgInicio, Color.White);
            spriteBatch.DrawString(font, "EXTREME", new Vector2(90, 190), Color.White);
            spriteBatch.DrawString(font, "RACING", new Vector2(130, 250), Color.White);
            spriteBatch.DrawString(font2, "PRESS SPACE TO START", new Vector2(140, 350), Color.White);
            spriteBatch.End();
        }

        private void SeleccionDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tBgSelec, rBgSelec, Color.White);
            spriteBatch.DrawString(font2, "SELECT YOUR CAR", new Vector2(170, 115), Color.White);
            spriteBatch.DrawString(font2, "PRESS 1 TO SELECT", new Vector2(80, 215), Color.White);
            spriteBatch.Draw(tCar1, rCar1, Color.White);
            spriteBatch.DrawString(font2, "PRESS 2 TO SELECT", new Vector2(80, 315), Color.White);
            spriteBatch.Draw(tCar2, rCar2, Color.White);
            spriteBatch.DrawString(font2, "PRESS 3 TO SELECT", new Vector2(80, 415), Color.White);
            spriteBatch.Draw(tCar3, rCar3, Color.White);
            spriteBatch.DrawString(font2, "PRESS 4 TO SELECT", new Vector2(80, 515), Color.White);
            spriteBatch.Draw(tCar4, rCar4, Color.White);
            spriteBatch.End();
        }

        private void EnJuegoDraw(SpriteBatch spriteBatch)
        {
            //5) Indicar al spritebach que soporte transformaciones
            spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                null, null, null, null,
                                camera.Transform);
            spriteBatch.Draw(tBg, rBg, Color.White);
            if (Carro1)
            {
                spriteBatch.Draw(tCar1, rCar, Color.White);
            }
            if (Carro2)
            {
                spriteBatch.Draw(tCar2, rCar, Color.White);
            }
            if (Carro3)
            {
                spriteBatch.Draw(tCar3, rCar, Color.White);
            }
            if (Carro4)
            {
                spriteBatch.Draw(tCar4, rCar, Color.White);
            }
            spriteBatch.Draw(tTaxi, rTaxi1, Color.White);
            spriteBatch.Draw(tTaxi, rTaxi2, Color.White);
            spriteBatch.Draw(tCarBlue, rCarBlue, Color.White);
            spriteBatch.Draw(tCamioneta, rCamioneta, Color.White);


            spriteBatch.End();
        }

        private void GameOverDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (win)
            {
                spriteBatch.Draw(tBgWinLose, rBgWinLose, Color.White);
                spriteBatch.DrawString(font, "YOU WIN", new Vector2(105, 200), Color.White);
                spriteBatch.DrawString(font2, "PRESS ENTER TO CONTINUE", new Vector2(115, 330), Color.White);


            }
            if (lose)
            {
                spriteBatch.Draw(tBgWinLose, rBgWinLose, Color.White);
                spriteBatch.DrawString(font, "YOU LOSE", new Vector2(100, 200), Color.White);
                spriteBatch.DrawString(font2, "PRESS ENTER TO CONTINUE", new Vector2(115, 330), Color.White);

            }
            spriteBatch.End();
        }
    }
}
