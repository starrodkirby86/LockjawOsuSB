/*

CLASS: Raincloud
    A raincloud. This is the controller for the raindrop
    object. Basically we can set and control the rain.

    TODO:
    X -- Port my test code over in here. (IE MAKE THE RAINCLOUD AYY LMAO)
    X -- Some sort of timer I guess
    X -- Current implementation takes too much space/resources, make it more efficient but still truly random
    X -- Implement a rotation controller.
      -- Implement a collision map controller.
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
        public CollisionMap NoNoRegion;

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
                    cloud[x2].drop(startTime + rng.Next(BeatmapConstants.DROP_VARIANCE * -1, BeatmapConstants.DROP_VARIANCE),
                        rng.Next(BeatmapConstants.SCREEN_LEFT, BeatmapConstants.SCREEN_RIGHT), 
                        BeatmapConstants.SCREEN_TOP-BeatmapConstants.SCREEN_TOP_OFFSET,
                        NoNoRegion);
                }

               // Queue for the next wrap.
                startTime += BeatmapConstants.RAINDROP_VELOCITY;
            }

        }

        public void createWind(int startTime, int endTime, int degInput)
        {
            // Simulates the effect of wind by rotating the raindrops a certain way and
            // having them move based off the angleOffset.
            // Assume that no wind is rad 0.

            // Begin the rotation
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                cloud[x2].rotate(startTime, degInput);
            }

            // End the rotation
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                cloud[x2].rotate(endTime, 0);
            }
        }

        public void importMap(string path)
        {
            // Imports a bitmap image into the collisionMap using string path.
            NoNoRegion.bmp2CollisionMap(path);
        }

        public void clearMap()
        {
            // Clears the map.
            NoNoRegion.clearMap();
        }

        // Instance Constructor
        public Raincloud()
        {
            // NEW CLOUD
            rng = new Random();
            cloud = new Raindrop[BeatmapConstants.MAX_RAINDROPS];
            NoNoRegion = new CollisionMap();
            clearMap();

            for(int x = 0; x < cloud.Length; x++)
            {
                cloud[x] = new Raindrop(BeatmapConstants.MIN_HEIGHT + rng.NextDouble() * BeatmapConstants.MAX_HEIGHT, 
                    rng.NextDouble() * BeatmapConstants.MAX_FADE);
            }
        }
    }
}
