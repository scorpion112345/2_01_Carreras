using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        Texture2D tBg;
        Rectangle rBg;

        //PLayer
        Texture2D tCar;
        Rectangle rCar;
        const int ANCHO_CAR = 35;
        const int ALTO_CAR = 70;


        //Enemies
        Texture2D tTaxi, tCamioneta, tCarBlue;
        Rectangle rTaxi1 , rTaxi2, rCamioneta , rCarBlue;


        Camera camera;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //3) Adecuar la pantalla al viewport
            graphics.PreferredBackBufferHeight = ALTO_VP;
            graphics.PreferredBackBufferWidth = ANCHO_VP;
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
            rCar = new Rectangle(ANCHO_VP / 2, ALTO_VP/ 2, ANCHO_CAR, ALTO_CAR);

            rCamioneta = new Rectangle((int)(ANCHO_VP *.3),  (int)(-ALTO_VP *.3), ANCHO_CAR, ALTO_CAR);
            rTaxi1 = new Rectangle((int)(ANCHO_VP * .65), (int)(-ALTO_VP* .6), ANCHO_CAR, ALTO_CAR);
            rCarBlue = new Rectangle((int)(ANCHO_VP * .40), (int)(-ALTO_VP * .9), ANCHO_CAR, ALTO_CAR);
            rTaxi2 = new Rectangle((int)(ANCHO_VP * .60), -(int)(2*ALTO_VP * .1), ANCHO_CAR, ALTO_CAR);





            //Inicializar la camara
            camera = new Camera(graphics.GraphicsDevice.Viewport, ANCHO_BG , ALTO_BG - ALTO_VP );
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
            tCar = Content.Load<Texture2D>("CPolicia");
            tCamioneta = Content.Load<Texture2D>("CCamioneta");
            tTaxi = Content.Load<Texture2D>("CTaxi");
            tCarBlue = Content.Load<Texture2D>("CCarBlue");




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

            // controles
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Left) && rCar.X > 100 )
                rCar.X -= 3;
            if (kbs.IsKeyDown(Keys.Right) && rCar.X + ANCHO_CAR < 400 )
                rCar.X += 3;

                rCar.Y -= 3;

            // Si el coche sale de la pantalla
            if(rTaxi2.Y > rCar.Y + ALTO_VP / 2)
            {

                // Posicionar aleatoriamente en el sig bloque de carretera
                rTaxi2.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP / 2 + ALTO_VP)- ALTO_CAR);
                rTaxi2.X = randRespawn.Next(100, 400 - ANCHO_CAR);
                //  intercecta con otro coche posicionado, 
                /*if(rTaxi2.Intersects(rTaxi1) || rTaxi2.Intersects(rCamioneta) || rTaxi2.Intersects(rCarBlue))
                {
                    //posicionar con base en el espacio reglamentario 
                    rTaxi2.Y = -randRespawn.Next(-rCar.Y + ALTO_VP / 2, -rCar.Y + ALTO_VP);
                    rTaxi2.X = randRespawn.Next(100, 400 - ANCHO_CAR);
                }*/
            }

            if (rTaxi1.Y > rCar.Y + ALTO_VP / 2)
            {

                // Posicionar aleatoriamente en el sig bloque de carretera
                rTaxi1.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP / 2 + ALTO_VP) - ALTO_CAR);
                rTaxi1.X = randRespawn.Next(100, 400 - ANCHO_CAR);
                //  intercecta con otro coche posicionado, 
                    //posicionar con base en el espacio reglamentario 

                
            }

            if (rCamioneta.Y > rCar.Y + ALTO_VP / 2)
            {

                // Posicionar aleatoriamente en el sig bloque de carretera
                rCamioneta.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP / 2 + ALTO_VP)- ALTO_CAR);
                rCamioneta.X = randRespawn.Next(100, 400 - ANCHO_CAR);
                //  intercecta con otro coche posicionado, 
                    //posicionar con base en el espacio reglamentario 
              
            }

            if (rCarBlue.Y > rCar.Y + ALTO_VP / 2)
            {

                // Posicionar aleatoriamente en el sig bloque de carretera
                rCarBlue.Y = -randRespawn.Next(-rCar.Y + ALTO_CAR + (ALTO_VP / 2), -rCar.Y + (ALTO_VP / 2 + ALTO_VP) - ALTO_CAR);
                rCarBlue.X = randRespawn.Next(100, 400 - ANCHO_CAR);
                //  intercecta con otro coche posicionado, //aafadfasdfadsfads
                    //posicionar con base en el espacio reglamentario jhAGSJhasgJASGajsg
                    //asdjasgdjasdg
            }
            if (true)
            {

            }

            //6)Actualizar la camara
            camera.Update(new Vector2(rCar.X, rCar.Y));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //5) Indicar al spritebach que soporte transformaciones
            spriteBatch.Begin(SpriteSortMode.Deferred,
                                BlendState.AlphaBlend,
                                null, null ,null , null,
                                camera.Transform);
            spriteBatch.Draw(tBg, rBg, Color.White);

            spriteBatch.Draw(tCar, rCar, Color.White);

            spriteBatch.Draw(tTaxi, rTaxi1, Color.White);
            spriteBatch.Draw(tTaxi, rTaxi2, Color.White);
            spriteBatch.Draw(tCarBlue,rCarBlue, Color.White);
            spriteBatch.Draw(tCamioneta, rCamioneta , Color.White);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
