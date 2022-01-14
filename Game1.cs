using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spline;
using WinForm;

namespace TDG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D Screen;

        SpriteFont Font;

        Engine engine;
        SimplePath path;
        //GameObjekt GameObjekt;
        Tower tower;
        STower sTower;
        FTower fTower;
        SEnemy sEnemy;
        FEnemy fEnemy;
        float textPos;

        Form1 myform;
        GameState CurrentGameState;
        enum GameState
        {
            Start,
            Level1,
            Level2,
            End,
        }


        public Texture2D particle, FT, ST, HE, LE, BG, Hit;


        public bool RT, hurt, pressing = false;
        public bool BT = true;

        public int particletimer, hurttimer = 0;
        public int Particletime = 30;

        int hurtdelay = 10;

        int time;

        public int Coins = 100;

        int size = 1200;
        MouseState mouseState, previousMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Screen = new RenderTarget2D(GraphicsDevice, size, size);
            graphics.PreferredBackBufferHeight = size;
            graphics.PreferredBackBufferWidth = size;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            myform = new Form1();
            myform.Show();

            particle = Content.Load<Texture2D>("Particle");
            FT = Content.Load<Texture2D>("FastTower");
            ST = Content.Load<Texture2D>("StrongTower");
            HE = Content.Load<Texture2D>("HeavyEnemy");
            LE = Content.Load<Texture2D>("LightEnemy");
            BG = Content.Load<Texture2D>("Background");
            Hit = Content.Load<Texture2D>("Bullet");
            Font = Content.Load<SpriteFont>("Font");

            // TODO: use this.Content to load your game content here
            engine = new Engine(particle, new Vector2(-100, -100));

            fEnemy = new FEnemy(LE);
            sEnemy = new SEnemy(HE);
            sTower = new STower(ST);
            fTower = new FTower(FT);
            tower = new Tower(new Vector2(-100, -100));

            path = new SimplePath(graphics.GraphicsDevice);
            path.generateDefaultPath();
            textPos = path.beginT;
            path.SetPos(0, Vector2.Zero);
            path.AddPoint(new Vector2(size, size));

            DrawOnRenderTarget();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                engine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                engine.TimeReset();
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                pressing = false;
            }
            engine.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                if (BT == false)
                {
                    BT = true;
                    RT = false;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                if (RT == false)
                {
                    BT = false;
                    RT = true;
                }
            }
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            switch (CurrentGameState)
            {
                case GameState.Start:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        path.SetPos(0, Vector2.Zero);
                        textPos = path.beginT;
                        fEnemy.Health = 10;
                        CurrentGameState = GameState.Level1;
                    }
                    break;

                case GameState.Level1:
                    textPos++;
                    textPos++;
                    textPos++;
                    textPos++;
                    textPos++;
                    Vector2 pos = path.GetPos(textPos);
                    fEnemy.Getpos(pos);
                    if (textPos >= path.endT)
                    {
                        CurrentGameState = GameState.End;
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (pressing == false)
                        {
                            path.SetPos(0, Vector2.Zero);
                            if (RT == true)
                            {
                                if (Coins >= 40)
                                {
                                    Coins -= 40;
                                    sTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                    pressing = false;
                                }
                            }
                            if (BT == true)
                            {
                                if (Coins >= 10)
                                {
                                    Coins -= 10;
                                    fTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                }

                            }

                            pressing = true;
                        }
                    }
                    if (fTower.HitBox().Intersects(fEnemy.HitBox()))
                    {
                        int health = fEnemy.Health;
                        int attack = fTower.Damage;
                        int lifeleft = health - attack;
                        fEnemy.Health = lifeleft;
                        if (lifeleft <= 0)
                        {
                            textPos = path.beginT;
                            CurrentGameState = GameState.Level2;
                            sEnemy.Health = 30;
                            Coins += 30;
                        }
                        else
                        {
                            hurt = true;
                            hurttimer = 0;
                        }
                    }

                    if (sTower.HitBox().Intersects(fEnemy.HitBox()))
                    {
                        int health = fEnemy.Health;
                        int attack = fTower.Damage;
                        int lifeleft = health - attack;
                        fEnemy.Health = lifeleft;
                        if (lifeleft <= 0)
                        {
                            textPos = path.beginT;
                            CurrentGameState = GameState.Level2;
                            sEnemy.Health = 30;
                            Coins += 30;
                        }
                        else
                        {
                            hurt = true;
                            hurttimer = 0;
                        }
                    }
                    break;

                case GameState.Level2:
                    textPos++;
                    textPos++;
                    if (textPos >= path.endT)
                    {
                        CurrentGameState = GameState.End;
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (pressing == false)
                        {
                            path.SetPos(0, Vector2.Zero);
                            if (RT == true)
                            {
                                if (Coins >= 40)
                                {
                                    Coins -= 40;
                                    sTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                    pressing = false;
                                }
                            }
                            if (BT == true)
                            {
                                if (Coins >= 10)
                                {
                                    Coins -= 10;
                                    fTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                }

                            }

                            pressing = true;
                        }
                    }

                    if (fTower.HitBox().Intersects(sEnemy.HitBox()))
                    {
                        int health = sEnemy.Health;
                        int attack = fTower.Damage;
                        int lifeleft = health - attack;
                        sEnemy.Health = lifeleft;
                        if (lifeleft <= 0)
                        {
                            textPos = path.beginT;
                            CurrentGameState = GameState.Level1;
                            Coins += 60;
                            fEnemy.Health = 10;
                        }
                        else
                        {
                            hurt = true;
                            hurttimer = 0;
                        }
                    }

                    if (sTower.HitBox().Intersects(sEnemy.HitBox()))
                    {
                        int health = sEnemy.Health;
                        int attack = fTower.Damage;
                        int lifeleft = health - attack;
                        sEnemy.Health = lifeleft;
                        if (lifeleft <= 0)
                        {
                            textPos = path.beginT;
                            CurrentGameState = GameState.Level1;
                            Coins += 60;
                            fEnemy.Health = 10;
                        }
                        else
                        {
                            hurt = true;
                            hurttimer = 0;
                        }
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.Level1;
                    }
                    break;

                case GameState.End:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        CurrentGameState = GameState.Level1;
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        Exit();
                    }
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Draw(Screen, Vector2.Zero, Color.Transparent);


            switch (CurrentGameState)
            {
                case GameState.Start:
                    spriteBatch.DrawString(Font, "Press Left mouse button to start", new Vector2(200, 50), Color.Black);
                    break;

                case GameState.Level1:
                    spriteBatch.DrawString(Font, "Coins:" + Coins, new Vector2(450, 50), Color.Black);
                    path.Draw(spriteBatch);
                    if (textPos < path.endT)
                    {
                        if (hurt == false)
                        {
                            spriteBatch.Draw(LE, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);
                        }

                        if (hurt == true)
                        {
                            if (time != gameTime.ElapsedGameTime.Milliseconds)
                            {
                                time = gameTime.ElapsedGameTime.Milliseconds;
                                hurttimer++;
                                spriteBatch.Draw(Hit, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                            }
                            if (hurttimer == hurtdelay)
                            {
                                hurt = false;
                            }
                        }
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        engine.Draw(spriteBatch);
                    }
                    break;

                case GameState.Level2:

                    spriteBatch.DrawString(Font, "Coins:" + Coins, new Vector2(450, 50), Color.Black);
                    path.Draw(spriteBatch);
                    sTower.Draw(spriteBatch);
                    fTower.Draw(spriteBatch);
                    if (textPos < path.endT)
                    {
                        if (hurt == false)
                        {
                            spriteBatch.Draw(HE, path.GetPos(textPos), new Rectangle(0, 0, HE.Width, LE.Height), Color.White, 0f, new Vector2(HE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);
                        }

                        if (hurt == true)
                        {
                            if (time != gameTime.ElapsedGameTime.Milliseconds)
                            {
                                time = gameTime.ElapsedGameTime.Milliseconds;
                                hurttimer++;
                                spriteBatch.Draw(Hit, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                            }
                            if (hurttimer == hurtdelay)
                            {
                                hurt = false;
                            }
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        engine.Draw(spriteBatch);
                    }
                    break;

                case GameState.End:
                    spriteBatch.DrawString(Font, "You lost", new Vector2(200, 50), Color.Black);
                    spriteBatch.DrawString(Font, "Final amount of Coins:" + Coins, new Vector2(200, 250), Color.Black);
                    spriteBatch.DrawString(Font, "press right mouse button to try again and press left to quit the game", new Vector2(100, 450), Color.Black);

                    break;
            }
            spriteBatch.End();


            base.Draw(gameTime);
        }


        private void DrawOnRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(Screen);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            spriteBatch.Draw(BG, Vector2.Zero, Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
        }/*
        public bool CanPlace(GameObject g)
        {
            Color[] pixels = new Color[g.texture.Width * g.texture.Height];
            Color[] pixels2 = new Color[g.texture.Width * g.texture.Height];
            g.texture.GetData<Color>(pixels2);
            Screen.GetData(0, g.hitbox, pixels, 0, pixels.Length);
            for (int i = 0; i < pixels.Length; ++i)
            {
                if (pixels[i].A > 0.0f && pixels2[i].A > 0.0f)
                    return false;
            }
            return true;
        }*/
    }
}
