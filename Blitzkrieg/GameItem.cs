using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Blitzkrieg
{
    class GameItem
    {
        //creats a new public rectangle variable
        public Rectangle rectangle;
        //makes a new protected texture variable
        protected Texture2D sprite;
        //creates a new protected color variable
        protected Color color;

        //constructs the object based on the given variables
        public GameItem(Rectangle Rectangle, Texture2D Sprite, Color Color)
        {

            //calls the set rectangle method and gives the input rectangle
            this.SetRectangle(Rectangle);
            //calls the set texture method and gives the input texture
            this.SetTexture(Sprite);
            //calls the set color method and gives it the input color
            this.SetColor(Color);
        }

        //sets the rectangle
        public void SetRectangle(Rectangle Rec)
        {
            //sets the rectangle equal to the input rectangle
            rectangle = Rec;
        }

        //sets the texture of the gameitem 
        public void SetTexture(Texture2D Sprite)
        {
            //sets the texture to the given texture
            sprite = Sprite;
        }

        //sets the color of the gameitem
        public void SetColor(Color Color)
        {
            //sets the color equal to the input color
            color = Color;
        }

        //gets the sprite of the game item
        public Texture2D GetSprite()
        {
            //returns the sprite of the gameitem 
            return sprite;
        }

        //gets the color of the gameitem
        public Color GetColor()
        {
            //returns the color
            return color;
        }

        //gets the rectangle of the gameitem
        public Rectangle GetRectangle()
        {
            //returns the rectangle
            return rectangle;
        }

        //draws the gameitem
        public void DrawGameItem(SpriteBatch spriteBatch)
        {
            //draws the gameitem using the given sprite batch
            spriteBatch.Draw(sprite, rectangle, color);
        }
    }
}
