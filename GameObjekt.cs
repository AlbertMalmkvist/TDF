using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TDG
{
    class GameObjekt
    {
        Texture2D tex;
        Vector2 Pos;
        Rectangle hitbox; 
        public GameObjekt(Texture2D texture, Vector2 position, Rectangle hitboxs)
        {
            tex = texture;
            Pos = position;
            hitbox = hitboxs;
        }

        public Rectangle HitBox()
        {
            return hitbox;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, Pos, null, Color.White,
                0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
