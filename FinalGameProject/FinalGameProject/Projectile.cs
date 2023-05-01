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
    class Projectile : MovingGameItem
    {
        private int damage;//Declares an int variable that determines the amount of damage dealt to other players
        private int direction; //makes a direction int to make life make sense
        private int speed; //makes a speed temporarily so i can make a move function

        //"Getters and Setters" for all fields
        public int getDamage()
        {
            return damage;
        }
        public void setDamage(int aDamage)
        {
            damage = aDamage;
        }
        public int getSpeed()
        {
            return speed;
        }
        public void setSpeed(int Speed)
        {
            speed = Speed;
        }
        public int getDirection()
        {
            return direction;
        }
        public void setDirection(int Direction)
        {
            direction = Direction;
        }

        public void Move()
        {
            //Controls movement for four basic directions and changes textures accordingly
            if (direction == 0)
            {
                this.rectangle.Y -= speed;                
            }
            if (direction == 2)
            {
                this.rectangle.Y += speed;                
            }
            if (direction == 1)
            {
                this.rectangle.X += speed;
            }
            if (direction == 3)
            {
                this.rectangle.X -= speed;
            }
        }

        //Sets up the constructor for Projectile
        public Projectile( Rectangle Rectangle, Texture2D Texture, Color Colour, int Speed, int Damage, int Direction) : base(Rectangle, Texture, Colour, Speed)
        {
            setSpeed(Speed);
            setDamage(Damage);
            setDirection(Direction);
        }

        //Does damage to intersecting Tanks
        private bool DoDamage(Rectangle bulletRec, Rectangle otherRec)
        {
            if (this.HitTest(bulletRec, otherRec))
                return true;
            else
                return false;
        }
    }
}
