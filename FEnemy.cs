using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDG
{
    public class FEnemy
    {
        public int Health { get; set; }
        Vector2 pos;
        Texture2D tex;

        public FEnemy(Texture2D texture)
        {
            this.tex = texture;
        }
        public void Getpos(Vector2 texpos)
        {
            this.pos = texpos;
        }
        public Rectangle HitBox()
        {
            int tey = tex.Height / 2;
            int tew = tex.Width / 2;
            return new Rectangle((int)pos.X-tew, (int)pos.Y-tey, tex.Width, tex.Height);
        }
    }
}
