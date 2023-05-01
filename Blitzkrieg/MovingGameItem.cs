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
    class MovingGameItem : GameItem//Inherits from GameItem
    {
        private int speed;//Declares an int variable for the speed field
        //Declares four Texture2D variables that hold the directional textures
        private Texture2D upTexture, downTexture, leftTexture, rightTexture;
        public Texture2D texture;
        public enum Direction//Declares an enumerated variable that determines the direction in which moving objects move
        {
            Up, Down, Left, Right
        }

        //"Getters and Setters" for all fields
        public int getSpeed()
        {
            return speed;
        }
        public void setSpeed(int aSpeed)
        {
            speed = aSpeed;
        }
        public Texture2D getUpTexture()
        {
            return upTexture;
        }
        public void setUpTexture(Texture2D aUpTexture)
        {
            upTexture = aUpTexture;
        }
        public Texture2D getDownTexture()
        {
            return downTexture;
        }
        public void setDownTexture(Texture2D aDownTexture)
        {
            downTexture = aDownTexture;
        }
        public Texture2D getLeftTexture()
        {
            return leftTexture;
        }
        public void setLeftTexture(Texture2D aLeftTexture)
        {
            leftTexture = aLeftTexture;
        }
        public Texture2D getRightTexture()
        {
            return rightTexture;
        }
        public void setRightTexture(Texture2D aRightTexture)
        {
            rightTexture = aRightTexture;
        }

        //Sets up the constructor for MovingGameItem
        public MovingGameItem(Rectangle Rectangle, Texture2D Texture, Color Colour, int Speed) : base(Rectangle, Texture, Colour)
        {
            speed = Speed;
        }

        //Does the hit-testing for all MovingGameItems (returns a bool)
        public bool HitTest(Rectangle thisRectangle, Rectangle otherRectangle)
        {
            if (thisRectangle.Intersects(otherRectangle))
                return true;
            else
                return false;
        }

        //Controls all movement
        public virtual void Move(Direction d, int speed)
        {
            //Does the same as Move(Direction d), but allows the user to manipulate the speed with which
            //the object was constructed
            int tempSpeed = this.speed;//Sends the constructed speed to a temporary int variable

            this.speed = speed;//Sets the constructed speed to the newly input speed

            this.Move(d);//Moves the object using the direction and the new speed

            this.speed = tempSpeed;//Sets the constructed speed back to its original speed
        }
        public virtual void Move(Direction d)
        {
            
            //Controls movement for four basic directions and changes textures accordingly
            if (d == Direction.Up)
            {
                this.rectangle.Y -= speed;

                texture = upTexture;
            }
            if (d == Direction.Down)
            {
                this.rectangle.Y += speed;

                texture = downTexture;
            }
            if (d == Direction.Right)
            {
                this.rectangle.X += speed;

                texture = rightTexture;
            }
            if (d == Direction.Left)
            {
                this.rectangle.X -= speed;

                texture = leftTexture;
            }
        }
    }
}
