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
    class Message
    {
        //Declare a protected string, vector, font, color, and bool variable
        protected string text;
        protected Vector2 position;
        protected SpriteFont font;
        protected Color color;
        protected bool selected;

        //Construct the class
        public Message(string Text, Vector2 Position, SpriteFont Font, Color Color, bool Selected)
        {
            //Set text to string given
            text = Text;
            //Set position to vector given
            position = Position;
            //Set font to sprite font given
            font = Font;
            //Set color to the color given
            color = Color;
            //Set selected to bool given
            selected = Selected;
        }

        //Get the text value
        public string getText()
        {
            return text;
        }
        //Set the text value
        public void setText(string aText)
        {
            text = aText;
        }

        //Get the position value
        public Vector2 getPosition()
        {
            return position;
        }
        //Set the position value
        public void setPosition(Vector2 aPosition)
        {
            position = aPosition;
        }

        //Get the font value
        public SpriteFont getFont()
        {
            return font;
        }
        //Set the font value
        public void setFont(SpriteFont aFont)
        {
            font = aFont;
        }

        //Get the color value
        public Color getColor()
        {
            return color;
        }
        //Set the color value
        public void setColor(Color aColor)
        {
            color = aColor;
        }

        //Get the "selected" value
        public bool getSelected()
        {
            return selected;
        }
        //Set the "selected" value
        public void setSelected(bool aSelected)
        {
            selected = aSelected;
        }

        public void DrawText(SpriteBatch spriteBatch)
        {
            //Draw the text by getting all required parts
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}
