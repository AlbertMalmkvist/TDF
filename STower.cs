using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDG
{
    class STower : Tower
    {
        public int Damage = 3;
        Texture2D tex;
        int delay = 30;
        int Attack = 0;
        int Timer;

        public STower(Texture2D texture) : base(pos: Pos)
        {
            this.tex = texture;
        }

        public void Update(GameTime gametime)
        {
            if (Timer <= gametime.ElapsedGameTime.Milliseconds)
            {
                Timer = gametime.ElapsedGameTime.Milliseconds;
                Attack++;
            }

        }

        public int attack()
        {
            if (Attack > delay)
            {
                Attack = 0;
                return Damage;
            }
            return 0;
        }

        public Rectangle HitBox()
        {
            int tey = tex.Height * 3;
            tey = tey / 2;

            int tew = tex.Height * 3;
            tew = tew / 2;
            return new Rectangle((int)Pos.X - tew, (int)Pos.Y - tey, tex.Width * 3, tex.Height * 3);
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
