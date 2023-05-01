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
    class Tank : MovingGameItem
    {
        private int health;//Declares an int variable that determines the amount of health the tanks have
        private int meleeDamage;//Declares an int variable that determines the amount of melee damage the tanks do

        //"Getters and Setters" for all fields
        public int getHealth()
        {
            return health;
        }
        public void setHealth(int aHealth)
        {
            health = aHealth;
        }
        public int getMeleeDamage()
        {
            return meleeDamage;
        }
        public void setMeleeDamage(int aMeleeDamage)
        {
            meleeDamage = aMeleeDamage;
        }

        //Sets up the constructor for Tank
        public Tank(Rectangle Rectangle, Texture2D Texture, Color Colour, int Speed, int Health, int MeleeDamage) : base(Rectangle, Texture, Colour, Speed)
        {
            health = Health;
            meleeDamage = MeleeDamage;
        }

        //Does melee damage
        public void DoMeleeDamage(Rectangle thisRectangle, Rectangle otherRectangle, int otherHealth)
        {
            if (this.HitTest(thisRectangle, otherRectangle))
            {
                otherHealth -= meleeDamage;
            }
        }
    }
}
