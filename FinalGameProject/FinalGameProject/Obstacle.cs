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


namespace FinalGameProject
{
    class Obstacle : GameItem
    {
        //makes a new private health variable
        private int health;

        //constructs the object using the input
        public Obstacle(Rectangle rectangle, Texture2D sprite, Color color, int Health) : base(rectangle, sprite, color)
        {
            //calls the set health method
            this.SetHealth(Health);
        }

        //sets the health to the given
        public void SetHealth(int Health)
        {
            //sets the health equal to the given health
            health = Health;
        }

        //gets the health when called
        public int GetHealth()
        {

            //returns the health
            return health;
        }

        //causes the object to take the input amount of damage when called
        public void TakeDamage(int damage)
        {
            //subracts the given damage from the health
            health -= damage;
        }

        //draws the object when called
        public void Draw(SpriteBatch spriteBatch)
        {
            //draws the object using the given sprite batch
            spriteBatch.Draw(sprite, rectangle, color);
        }

        //moves the object to the given position when called
        public void MoveTo(int xPosition, int yPosition)
        {
            //sets the rectangle to a new rectangle in the given position
            base.SetRectangle(new Rectangle(xPosition, yPosition, rectangle.Width, rectangle.Height));
        }
    }
}
