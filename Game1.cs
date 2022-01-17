using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spline;
using System.Collections.Generic;
using System.Linq;
using WinForm;

namespace TDG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        RenderTarget2D miniMap;
        Rectangle viewSize;

        SpriteFont Font;

        Engine engine;
        SimplePath path;
        STower sTower;
        FTower fTower;
        SEnemy sEnemy;
        FEnemy fEnemy;

        Bullets bullets;
        List<Bullets> bulletList;

        GameObjekt[] Epaths;

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


        public Texture2D particle, FT, ST, HE, LE, BG, Hit, Walkon;


        public bool RT, hurt, pressing = false;
        public bool BT = true;

        public int particletimer, hurttimer, fired = 0;
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
            graphics.PreferredBackBufferHeight = size;
            graphics.PreferredBackBufferWidth = size;
            graphics.ApplyChanges();
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            miniMap = new RenderTarget2D(GraphicsDevice, size, size);
            viewSize = new Rectangle(0, 0, 120, 120);

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

            bulletList = new List<Bullets>();
            Rectangle ImpZone = new Rectangle(-100, -100, Hit.Width, Hit.Height);
            bullets = new Bullets(Hit, new Vector2(-100, -100), ImpZone, 0, Vector2.Zero);
            bulletList.Add(bullets);
            bulletList.Add(bullets);
            bulletList.Add(bullets);
            bulletList.Add(bullets);

            path = new SimplePath(graphics.GraphicsDevice);
            path = new SimplePath(graphics.GraphicsDevice);
            path.Clean();
            path.AddPoint(new Vector2(0, 0));
            path.AddPoint(new Vector2(150, 150));
            path.AddPoint(new Vector2(250, 150));
            path.AddPoint(new Vector2(350, 150));
            path.AddPoint(new Vector2(450, 150));
            path.AddPoint(new Vector2(550, 150));
            path.AddPoint(new Vector2(650, 150));
            path.AddPoint(new Vector2(750, 150));
            path.AddPoint(new Vector2(850, 150));
            path.AddPoint(new Vector2(950, 150));
            path.AddPoint(new Vector2(1050, 150));
            path.AddPoint(new Vector2(1050, 225));
            path.AddPoint(new Vector2(1050, 300));
            path.AddPoint(new Vector2(1050, 375));
            path.AddPoint(new Vector2(1050, 450));
            path.AddPoint(new Vector2(1050, 525));
            path.AddPoint(new Vector2(1050, 600));
            path.AddPoint(new Vector2(950, 600));
            path.AddPoint(new Vector2(850, 600));
            path.AddPoint(new Vector2(750, 600));
            path.AddPoint(new Vector2(650, 600));
            path.AddPoint(new Vector2(550, 600));
            path.AddPoint(new Vector2(450, 600));
            path.AddPoint(new Vector2(350, 600));
            path.AddPoint(new Vector2(250, 600));
            path.AddPoint(new Vector2(150, 600));
            path.AddPoint(new Vector2(150, 675));
            path.AddPoint(new Vector2(150, 750));
            path.AddPoint(new Vector2(150, 825));
            path.AddPoint(new Vector2(150, 900));
            path.AddPoint(new Vector2(150, 975));
            path.AddPoint(new Vector2(150, 1050));
            path.AddPoint(new Vector2(250, 1050));
            path.AddPoint(new Vector2(350, 1050));
            path.AddPoint(new Vector2(450, 1050));
            path.AddPoint(new Vector2(550, 1050));
            path.AddPoint(new Vector2(650, 1050));
            path.AddPoint(new Vector2(750, 1050));
            path.AddPoint(new Vector2(850, 1050));
            path.AddPoint(new Vector2(950, 1050));
            path.AddPoint(new Vector2(1050, 1050));
            path.AddPoint(new Vector2(1050, 1125));
            path.AddPoint(new Vector2(1050, size));
            path.SetPos(0, new Vector2(0, 0));
            path.Compute();
            textPos = path.beginT;

            Epaths = new GameObjekt[43];
            for (int i = 0; i < 43; i++)
            {
                if (i == 0)
                {
                    Walkon = Content.Load<Texture2D>("BPath");
                    Vector2 posi = path.GetPos(i);
                    Rectangle area = new Rectangle((int)posi.X, (int)posi.Y, Walkon.Width, Walkon.Height);
                    GameObjekt AddObjekt = new GameObjekt(Walkon, posi, area);
                    Epaths[i] = AddObjekt;
                }
                else
                {
                    Walkon = Content.Load<Texture2D>("Path");
                    Vector2 posi = path.GetPos(i);
                    posi.X -= 50;
                    posi.Y -= 50;
                    Rectangle area = new Rectangle((int)posi.X, (int)posi.Y, Walkon.Width, Walkon.Height);
                    GameObjekt AddObjekt = new GameObjekt(Walkon, posi, area);
                    Epaths[i] = AddObjekt;

                }
            }

            // TODO: use this.Content to load your game content here
            engine = new Engine(particle, new Vector2(-100, -100));

            fEnemy = new FEnemy(LE);
            sEnemy = new SEnemy(HE);
            sTower = new STower(ST, new Vector2(-100, -100));
            fTower = new FTower(FT, new Vector2(-100, -100));


            DrawOnRenderTarget();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            sTower.Update(gameTime);
            fTower.Update(gameTime);
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
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

            switch (CurrentGameState)
            {
                case GameState.Start:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        pressing = true;
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
                        Vector2 place = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        int move = ST.Width / 2;
                        place.X = place.X - move;
                        place.Y = place.Y - move;
                        if (pressing == false)
                        {
                            Rectangle placecheck = new Rectangle((int)place.X, (int)place.Y, ST.Width, ST.Height);
                            int Pcount = 0;
                            for (int i = 0; i < 43; i++)
                            {
                                if (placecheck.Intersects(Epaths[i].HitBox()))
                                {
                                    Pcount++;
                                }
                            }
                            if (RT == true && Pcount == 0)
                            {
                                if (placecheck.Intersects(fTower.HitBox()) || sTower.HitBox().Contains(placecheck))
                                {

                                }
                                else
                                {
                                    if (Coins >= 40)
                                    {
                                        Coins -= 40;
                                        sTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                        pressing = false;
                                    }
                                }


                            }
                            if (BT == true && Pcount == 0)
                            {
                                if (placecheck.Intersects(sTower.HitBox()) || sTower.HitBox().Contains(placecheck))
                                {

                                }
                                else
                                {
                                }
                                if (true)
                                {

                                }
                                if (Coins >= 10)
                                {
                                    Coins -= 10;
                                    fTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                }

                            }

                            pressing = true;
                        }
                    }

                    if (fTower.AttackArea().Intersects(fEnemy.HitBox()) || fTower.AttackArea().Contains(fEnemy.HitBox()))
                    {
                        int attack = fTower.Attack();
                        if (attack != 0)
                        {
                            Vector2 TP = fTower.TWhere();
                            Vector2 EP = path.GetPos(textPos);
                            Vector2 BA = new Vector2(0, 0);
                            BA.X = EP.X - TP.X;
                            BA.Y = EP.Y - TP.Y;


                            Rectangle ImpZone = new Rectangle((int)TP.X - Hit.Width / 2, (int)TP.Y - Hit.Height / 2, Hit.Width, Hit.Height);
                            bullets = new Bullets(Hit, TP, ImpZone, 5, BA);
                            bulletList.Add(bullets);
                            System.Diagnostics.Debug.WriteLine("f1");
                        }
                        else
                        {
                        }
                    }

                    if (sTower.AttackArea().Intersects(fEnemy.HitBox()) || sTower.AttackArea().Contains(fEnemy.HitBox()))
                    {
                        int attack = fTower.Attack();
                        if (attack != 0)
                        {
                            Vector2 TP = sTower.TWhere();
                            Vector2 EP = path.GetPos(textPos);
                            Vector2 BA = EP - TP;

                            Rectangle ImpZone = new Rectangle((int)TP.X - Hit.Width / 2, (int)TP.Y - Hit.Height / 2, Hit.Width, Hit.Height);
                            bullets = new Bullets(Hit, TP, ImpZone, 15, BA);
                            bulletList.Add(bullets);
                            System.Diagnostics.Debug.WriteLine("s1");
                        }
                        else
                        {
                        }
                    }
                    foreach (Bullets bullets in bulletList.ToList())
                    {
                        Rectangle Hitting = bullets.HitBox();
                        if (Hitting.Intersects(fEnemy.HitBox()))
                        {
                            int dam = bullets.DamScr();
                            int health = fEnemy.Health;
                            int lifeleft = health - dam;
                            fEnemy.Health = lifeleft;
                            if (lifeleft == 0)
                            {
                                textPos = path.beginT;
                                CurrentGameState = GameState.Level2;
                                sEnemy.Health = 10;
                                Coins += 30;
                            }
                        else
                        {
                            hurt = true;
                            hurttimer = 0;
                                System.Diagnostics.Debug.WriteLine("hit");
                            }
                        }

                        bulletList.Remove(bullets);
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
                        Vector2 place = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                        int move = ST.Width / 2;
                        place.X = place.X - move;
                        place.Y = place.Y - move;
                        if (pressing == false)
                        {
                            Rectangle placecheck = new Rectangle((int)place.X, (int)place.Y, ST.Width, ST.Height);
                            int Pcount = 0;
                            for (int i = 0; i < 43; i++)
                            {
                                if (placecheck.Intersects(Epaths[i].HitBox()))
                                {
                                    Pcount++;
                                }
                            }
                            if (RT == true && Pcount == 0)
                            {
                                if (placecheck.Intersects(fTower.HitBox()) || fTower.HitBox().Contains(placecheck))
                                {

                                }
                                else
                                {
                                    if (Coins >= 40)
                                    {
                                        Coins -= 40;
                                        sTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                        pressing = false;
                                    }
                                }
                            }
                            if (BT == true && Pcount == 0)
                            {
                                if (placecheck.Intersects(sTower.HitBox()) || sTower.HitBox().Contains(placecheck))
                                {

                                }
                                else
                                {
                                    if (Coins >= 10)
                                    {
                                        Coins -= 10;
                                        fTower.Pos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                                    }
                                }

                            }

                            pressing = true;
                        }
                    }

                    if (fTower.AttackArea().Intersects(sEnemy.HitBox()) || fTower.AttackArea().Contains(sEnemy.HitBox()))
                    {
                        int attack = fTower.Attack();
                        if (attack != 0)
                        {
                            Vector2 TP = fTower.TWhere();
                            Vector2 EP = path.GetPos(textPos);
                            Vector2 BA = new Vector2(0, 0);
                            BA.X = EP.X - TP.X;
                            BA.Y = EP.Y - TP.Y;


                            Rectangle ImpZone = new Rectangle((int)TP.X - Hit.Width / 2, (int)TP.Y - Hit.Height / 2, Hit.Width, Hit.Height);
                            bullets = new Bullets(Hit, TP, ImpZone, 5, BA);
                            bulletList.Add(bullets);
                            System.Diagnostics.Debug.WriteLine("f2");
                        }
                        else
                        {
                        }
                    }

                    if (sTower.AttackArea().Intersects(sEnemy.HitBox()) || sTower.AttackArea().Contains(sEnemy.HitBox()))
                    {
                        int attack = fTower.Attack();
                        if (attack != 0)
                        {
                            Vector2 TP = sTower.TWhere();
                            Vector2 EP = path.GetPos(textPos);
                            Vector2 BA = new Vector2(0, 0);
                            BA.X = EP.X - TP.X;
                            BA.Y = EP.Y - TP.Y;


                            Rectangle ImpZone = new Rectangle((int)TP.X - Hit.Width / 2, (int)TP.Y - Hit.Height / 2, Hit.Width, Hit.Height);
                            bullets = new Bullets(Hit, TP, ImpZone, 15, BA);
                            bulletList.Add(bullets);
                            System.Diagnostics.Debug.WriteLine("s2");
                        }
                        else
                        {
                        }
                    }
                    foreach (Bullets bullets in bulletList.ToList())
                    {
                        Rectangle Hitting = bullets.HitBox();
                        if (Hitting.Intersects(sEnemy.HitBox()))
                        {
                            int dam = bullets.DamScr();
                            int health = sEnemy.Health;
                            int lifeleft = health - dam;
                            fEnemy.Health = lifeleft;
                            if (lifeleft == 0)
                            {
                                textPos = path.beginT;
                                CurrentGameState = GameState.Level2;
                                fEnemy.Health = 30;
                                Coins += 70;
                            }
                        }

                        bulletList.Remove(bullets);
                    }
                    break;

                case GameState.End:
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        textPos = path.beginT;
                        CurrentGameState = GameState.Level1;
                    }
                    if (mouseState.RightButton == ButtonState.Pressed)
                    {
                        Exit();
                    }
                    break;
            }

            foreach (Bullets bullets in bulletList.ToList())
            {
                this.bullets.Update(gameTime);
                Vector2 NPlace = this.bullets.BWhere();
                if (NPlace.X < 0 - Hit.Width || NPlace.X > size + Hit.Width || NPlace.Y < 0 - Hit.Height || NPlace.Y > size + Hit.Height)
                {
                    bulletList.Remove(this.bullets);
                }
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            // TODO: Add your drawing code here
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.White);
            foreach (Bullets bullets in bulletList)
            {
                bullets.Draw(spriteBatch);
            }


            switch (CurrentGameState)
            {
                case GameState.Start:
                    spriteBatch.DrawString(Font, "Press Left mouse button to start", new Vector2(200, 50), Color.Black);
                    break;

                case GameState.Level1:
                    spriteBatch.Draw(BG, Vector2.Zero, Color.White);

                    for (int i = 0; i < 43; i++)
                    {
                        Epaths[i].Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(Font, "Coins:" + Coins, new Vector2(450, 50), Color.Black);
                    sTower.Draw(spriteBatch);
                    fTower.Draw(spriteBatch);
                    if (textPos < path.endT)
                    {
                        if (hurttimer == hurtdelay)
                        {
                            hurt = false;
                        }
                        spriteBatch.Draw(LE, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                        if (hurt == true)
                        {
                            if (time != gameTime.ElapsedGameTime.Milliseconds)
                            {
                                time = gameTime.ElapsedGameTime.Milliseconds;
                                hurttimer++;
                                spriteBatch.Draw(Hit, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                            }
                        }
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        engine.Draw(spriteBatch);
                    }

                    spriteBatch.Draw(miniMap, viewSize, Color.White);
                    break;

                case GameState.Level2:
                    spriteBatch.Draw(BG, Vector2.Zero, Color.White);

                    for (int i = 0; i < 43; i++)
                    {
                        Epaths[i].Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(Font, "Coins:" + Coins, new Vector2(450, 50), Color.Black);
                    sTower.Draw(spriteBatch);
                    fTower.Draw(spriteBatch);
                    if (textPos < path.endT)
                    {
                        if (hurttimer == hurtdelay)
                        {
                            hurt = false;
                        }

                        spriteBatch.Draw(HE, path.GetPos(textPos), new Rectangle(0, 0, HE.Width, LE.Height), Color.White, 0f, new Vector2(HE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                        if (hurt == true)
                        {
                            if (time != gameTime.ElapsedGameTime.Milliseconds)
                            {
                                time = gameTime.ElapsedGameTime.Milliseconds;
                                hurttimer++;
                                spriteBatch.Draw(Hit, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

                            }
                        }
                    }

                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        engine.Draw(spriteBatch);
                    }

                    spriteBatch.Draw(miniMap, viewSize, Color.White);
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
            SpriteBatch sb = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.SetRenderTarget(miniMap);
            sb.Begin();
            GraphicsDevice.Clear(Color.Black); for (int i = 0; i < 43; i++)
            {
                Epaths[i].Draw(sb);
            }
            sb.Draw(ST, sTower.TWhere(), sTower.HitBox(), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sb.Draw(FT, fTower.TWhere(), fTower.HitBox(), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            sb.Draw(LE, path.GetPos(textPos), new Rectangle(0, 0, LE.Width, LE.Height), Color.White, 0f, new Vector2(LE.Width / 2, LE.Height / 2), 1f, SpriteEffects.None, 0f);

            sb.End();
            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
