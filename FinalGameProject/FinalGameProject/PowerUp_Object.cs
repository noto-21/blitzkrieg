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
    class PowerUp : GameItem 
    {
        //Declare a protected byte and bool variable
        protected byte powerType;
        protected bool helpTank;

        //Construct the class
        public PowerUp(Texture2D Sprite, Rectangle Rectangle, Color Color, byte PowerType, bool HelpTank) : base(Rectangle,Sprite,Color)
        {
            //Set powerType to the byte given
            powerType = PowerType;
            //Set helpTank to the bool given
            helpTank = HelpTank;
        }

        //Get powerType value
        public byte getPowerType()
        {
            return powerType;
        }
        //Set powerType value
        public void setPowerType(byte aPowerType)
        {
            powerType = aPowerType;
        }

        //Get helpTank value
        public bool getHelpTank()
        {
            return helpTank;
        }
        //Set helpTank value
        public void setHelpTank(bool aHelpTank)
        {
            helpTank = aHelpTank;
        }

        //Run an Assist method if called and given a tank
        public void Assist(Tank player)
        {
            //Check to value of powerType and determine what power up to give
            if (powerType <= 1)
            {
                //Will shrink the player's dimensions
                player.SetRectangle(new Rectangle(player.GetRectangle().X, player.GetRectangle().Y, player.GetRectangle().Width - 10, player.GetRectangle().Height - 10));
            }
            else if (powerType == 2)
            {
                //Will increase tank speed
                player.setSpeed(player.getSpeed()*2);
            }
            else if (powerType >= 3)
            {
                //Will reset health of player
                player.setHealth(player.getHealth() + 50);
            }
        }
        //Run an Cripple method if called and given a tank
        public void Cripple(Tank player)
        {
            //Check to value of powerType and determine what power up to give
            if (powerType <= 1)
            {
                //Will increase the player's dimensions
                player.SetRectangle(new Rectangle(player.GetRectangle().X, player.GetRectangle().Y, player.GetRectangle().Width + 10, player.GetRectangle().Height + 10));
            }
            else if (powerType == 2)
            {
                //Will decrease tank speed
                player.setSpeed(player.getSpeed()/2);
            }
            else if (powerType >= 3)
            {
                //Will half the health of the enemies
                player.setHealth(player.getHealth() / 2);
            }
        }
    }
}
