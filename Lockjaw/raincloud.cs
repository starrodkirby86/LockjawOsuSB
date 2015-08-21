/*

CLASS: Raincloud
    A raincloud. This is the controller for the raindrop
    object. Basically we can set and control the rain.

    TODO:
    X -- Port my test code over in here. (IE MAKE THE RAINCLOUD AYY LMAO)
    X -- Some sort of timer I guess
    -- Current implementation takes too much space/resources, make it more efficient but still truly random
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockjaw
{
    public class Raincloud
    {
        // Class members:

        // Property
        Random rng;
        Raindrop[] cloud;

        // Method
        public void makeItRain(int startTime, int iterations)
        {
            // MAKE IT RAIN, BOYS
            // startTime: When do you want it to start?
            // iterations: How many times do you want the drops to wrap?

            for (int x1 = 0; x1 < iterations; x1++)
            {
                // Calling all droplets to duty.
               for(int x2 = 0; x2 < cloud.Length; x2++)
                {
                    cloud[x2].drop(startTime + rng.Next(BeatmapConstants.DROP_VARIANCE * -1, BeatmapConstants.DROP_VARIANCE), rng.Next(0, 1367), BeatmapConstants.MIN_HEIGHT + rng.NextDouble() * BeatmapConstants.MAX_HEIGHT, rng.NextDouble()*BeatmapConstants.MAX_FADE);
                }

               // Queue for the next wrap.
                startTime += BeatmapConstants.RAINDROP_VELOCITY;
            }

        }

        // Instance Constructor
        public Raincloud()
        {
            // NEW CLOUD
            rng = new Random();
            cloud = new Raindrop[BeatmapConstants.MAX_RAINDROPS];
            for(int x = 0; x < cloud.Length; x++)
            {
                cloud[x] = new Raindrop();
            }
        }
    }
}
