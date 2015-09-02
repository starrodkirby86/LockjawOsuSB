/*

CLASS: Raindrop
    A single raindrop object. Each raindrop is its own sprite.
    Once spawned, a raindrop will "drop" from the top of the screen
    to the bottom of the screen. This drop is actually a move command.
    Once it reaches the bottom of the screen, it will wrap back to the top.

    The x-location is randomized and is re-rolled every screen wrap.

    When you want the raindrop to stop spawning, use the stop command.

    TODO:
    X -- Make raindrop, lmao
    X -- Successful screen wrap and loop.
    X -- Randomized x-location and height ratio.
    X -- Create rotation and angleOffset in droplet movement.
    X -- Develop algorithm for the NoNo region.

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGL.Framework;

namespace Lockjaw
{
    public class Raindrop
    {
        // Class members:

        // Property
        public string imagePath;
        public double heightRatio;
        public double fadeSetting;
        public int x;
        public int y;
        public double currentAngle;
        public int angleOffset;
        public SGL.Storyboard.Generators.Visual.SpriteGenerator droplet;

        // Method

       // DUMMY TEST COMMAND
        public void testDrop(int t0)
        {
            // Move command from top to bottom.
            droplet.move(0, t0, (int)(t0 + BeatmapConstants.BEAT_QUARTER), 320, 0, 320, BeatmapConstants.SCREEN_HEIGHT + (BeatmapConstants.RAINDROP_HEIGHT / 2));
        }

        // EDIT SETTINGS OF RAINDROP COMMANDS
        public void applyRoll(int t0, double inpHeight, double fadeInp)
        {
            // Apply roll inputs into droplet.

            // Rolls a random x-location and height @ t0,
            // then makes the droplet move from top to bottom. (FOR NOW)

            // Ro-kyu-bu! Rolling rolling rolling (has been done)
            heightRatio = inpHeight;
            fadeSetting = fadeInp;
         
            // Apply settings.
            droplet.fade(0, t0, t0, fadeSetting, fadeSetting);
            droplet.scaleVec(0, t0, t0, 1, heightRatio, 1, heightRatio);
        }
        

        public void rotate(int t0, double rotInput, bool RadFlag)
        {
            // Rotates the raindrop particle to the desired angle.
            // Boolean determines whether the second parameter is degrees or radians.
            // The angle is converted to radians, then the angleOffset is calculated.
            // Finally, the raindrop gets updated.

            double radInput = rotInput;

            // Conversion!
            if(!RadFlag)
                radInput = Math.PI * rotInput / 180.0;

            // Calculate angleOffset
            angleOffset = BeatmapConstants.MAX_ROTATION_DISTANCE * ((int)rotInput / 90);

            // Now the raindrop will rotate
            droplet.rotate(0, t0, t0 + BeatmapConstants.RAINDROP_VELOCITY * 2, currentAngle, radInput);

            // Update radian
            currentAngle = radInput;

        }

        // RAINDROP METHOD
        public void drop(int t0, int inpX, int inpY, CollisionMap NoNoRegion, bool underCollisionFlag)
        {
            // Drop function.
            // Currently has the droplet from top to bottom.

            // Initializations
            x = inpX;
            y = inpY;
            int t1;

            // Height offset is used to compensate for the height ratio scale.
            var heightOffset = (int)(Math.Round((BeatmapConstants.RAINDROP_HEIGHT * heightRatio / 2)));

            // Calculate the actual pixel velocity (rate) using constant RAINDROP_VELOCITY (time) and SCREEN_HEIGHT + heightOffset - y (distance)
            // This is used to help find the endtime of a droplet's movement in the event that it hits a NoNo region and prematurely stops.
            var endDistance = BeatmapConstants.SCREEN_HEIGHT + heightOffset;
            double pixelVelocity = ((double)endDistance - (double)y) / ((double)BeatmapConstants.RAINDROP_VELOCITY);

            // TOTALLY NEW IMPLEMENTATION ALGORITHM

            // Use proper indexing of X and Y
            // At this point, Y should be screen top.
            var indexX = x + (BeatmapConstants.SCREEN_LEFT * -1);
            var indexY = y;
            // To prevent Out of Range exceptions
            if (indexY > NoNoRegion.map.GetLength(1))
            {
                indexY = NoNoRegion.map.GetLength(1);
            }
            else if(indexY < 0)
            {
                indexY = 0;
            }

            //var indexY = y + ((BeatmapConstants.SCREEN_TOP + BeatmapConstants.SCREEN_TOP_OFFSET) * -1);

            // Initialize a ySlave, which is where y will drop to (Y -> YSLAVE)
            int ySlave;

            // NEWER IMPLEMENTATION
            while (indexY < NoNoRegion.map.GetLength(1))
            {
                // Initialize.
                // Update spot.
                ySlave = y;

                // Check for the closest NoNo region hit by iterating through the collision map until we hit a space or we hit the end of the map
                // Traverse the NoNo Region until we hit a NoNo space or the end of the map.
                while ((indexY < NoNoRegion.map.GetLength(1) && !NoNoRegion.map[indexX, indexY]))
                {
                    indexY++;
                }

                // Then send a droplet to whatever that space is.
                // First, use indexY to make the secondary y location

                ySlave += indexY;

                // Find time t1 given rate (pixelVelocity) and distance (ySlave - y)
                t1 = (int)((ySlave - y) / (pixelVelocity));

                // Send a droplet to that location.
                droplet.move(0, t0, t0 + t1, x, y, x + angleOffset, ySlave);


                if (underCollisionFlag)
                {
                    // Check if we have to create another droplet:
                    // The only condition to create another droplet
                    // would be if the NoNo Region gives way to an open spot

                    // So let's run a while loop to check for the next available spot.
                    while ((indexY < NoNoRegion.map.GetLength(1) && NoNoRegion.map[indexX, indexY]))
                    {
                        indexY++;
                    }

                    // So now that we're here, if the droplet is at the end, then we can finish our loop.
                    // But now we have our new location updated as this is the next available spot
                    // from the last NoNo Region hit. This spot CAN be the bottom of the screen, to which
                    // then this is our last iteration.
                    y += indexY;
                }
                else
                {
                    break;
                }
            }
        }


        // Instance Constructor
        public Raindrop(double heightInp = 1, double fadeInp = 1)
        {
            // Height ratio and fade setting are defined
            // upon declaration of the raindrop.
            imagePath = "sb\\rd.png";
            heightRatio = heightInp;
            fadeSetting = fadeInp;
            x = 0;
            y = 0;
            currentAngle = 0;
            angleOffset = 0;

            // Make sprite.
            droplet = SB.Sprite(imagePath, SB.Foreground, SB.TopCentre);

            // Apply settings
            if(heightInp != 1 && fadeInp != 1)
                applyRoll(0, heightRatio, fadeSetting);
        }

    }
}
