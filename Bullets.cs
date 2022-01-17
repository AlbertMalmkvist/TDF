using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDG
{
    class Bullets
    {
        Texture2D tex;
        Vector2 Pos;
        Rectangle hitbox;
        Vector2 velo;
        int Damage;
        float speed = 100.0f;
        public Bullets(Texture2D texture, Vector2 position, Rectangle hitboxs, int dama, Vector2 velocity)
        {
            velo = velocity;
            tex = texture;
            Pos = position;
            hitbox = hitboxs;
            Damage = dama;

        }
        public void Update(GameTime gametime)
        {
            Pos += velo*speed*(float)gametime.ElapsedGameTime.TotalSeconds;
        }

        public Rectangle HitBox()
        {
            return hitbox;
        }
        public int DamScr()
        {
            return Damage;
        }

        public Vector2 BWhere()
        {
            return Pos;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, Pos, hitbox, Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
