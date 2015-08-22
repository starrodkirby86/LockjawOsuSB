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
        public SGL.Storyboard.Generators.Visual.SpriteGenerator droplet;

        // Method
        public void testDrop(int t0)
        {
            // Move command from top to bottom.
            droplet.move(0, t0, (int)(t0 + BeatmapConstants.BEAT_QUARTER), 320, 0, 320, BeatmapConstants.SCREEN_HEIGHT + (BeatmapConstants.RAINDROP_HEIGHT / 2));
        }

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

        public void drop(int t0, int inpX, int inpY)
        {
            // Drop function.
            // Currently has the droplet from top to bottom.
            x = inpX;
            y = inpY;

            // Height offset is used to compensate for the height ratio scale.
            var heightOffset = (int)(Math.Round((BeatmapConstants.RAINDROP_HEIGHT * heightRatio / 2)));

            // Fire the cannons
            droplet.move(0, t0, t0 + BeatmapConstants.RAINDROP_VELOCITY, x, y, x, BeatmapConstants.SCREEN_HEIGHT + heightOffset);
        }


        // Instance Constructor
        public Raindrop()
        {
            // Give information.
            imagePath = "sb\\rd.png";
            heightRatio = 1;
            fadeSetting = 1;
            x = 0;
            y = 0;
            
            // Make sprite.
            droplet = SB.Sprite(imagePath, SB.Foreground, SB.TopCentre);
        }

        public Raindrop(double heightInp, double fadeInp)
        {
            // Height ratio and fade setting are defined
            // upon declaration of the raindrop.
            imagePath = "sb\\rd.png";
            heightRatio = heightInp;
            fadeSetting = fadeInp;
            x = 0;
            y = 0;

            // Make sprite.
            droplet = SB.Sprite(imagePath, SB.Foreground, SB.TopCentre);

            // Apply settings
            applyRoll(0, heightRatio, fadeSetting);
        }

    }
}
