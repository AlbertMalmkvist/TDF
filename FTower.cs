using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDG
{
    class FTower : Tower
    {
        public int Damage = 3;
        Texture2D tex;
        int delay = 5;
        int attack = 0;
        int Timer;
        public FTower(Texture2D texture, Vector2 Pos) : base(Pos)
        {
            this.tex = texture;
        }
        public void Update(GameTime gametime)
        {
            if (Timer <= gametime.ElapsedGameTime.Milliseconds)
            {
                Timer = gametime.ElapsedGameTime.Milliseconds;
                attack++;
            }

        }

        public int Attack()
        {
            if (attack > delay)
            {
                attack = 0;
                return Damage;
            }
            return 0;
        }

        public Vector2 TWhere()
        {
            return Pos;
        }

        public Rectangle HitBox()
        {
            int tey = tex.Height;
            tey = tey / 2;

            int tew = tex.Height;
            tew = tew / 2;
            return new Rectangle((int)Pos.X - tew, (int)Pos.Y - tey, tex.Width, tex.Height);
        }

        public Rectangle AttackArea()
        {
            int tey = tex.Height * 11;
            tey = tey / 2;

            int tew = tex.Height * 11;
            tew = tew / 2;
            return new Rectangle((int)Pos.X - tew, (int)Pos.Y - tey, tex.Width * 11, tex.Height * 11);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle((int)Pos.X - tex.Width / 2, (int)Pos.Y - tex.Height / 2, tex.Width, tex.Height);
            Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 2);

            spriteBatch.Draw(tex, Pos, sourceRectangle, Color.White,
                0, origin, 1f, SpriteEffects.None, 0f);
        }
    }
}
