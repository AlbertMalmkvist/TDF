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
        Texture2D Texture;
        Vector2 Position;
        Rectangle Hitbox; 
        public GameObjekt(Texture2D texture, Vector2 position, Rectangle hitbox)
        {
            Texture = texture;
            Position = position;
            Hitbox = hitbox;
        }
    }
}
