/*

CLASS: Raincloud
    A raincloud. This is the controller for the raindrop
    object. Basically we can set and control the rain.

    TODO:
    X -- Port my test code over in here. (IE MAKE THE RAINCLOUD AYY LMAO)
    X -- Some sort of timer I guess
    X -- Current implementation takes too much space/resources, make it more efficient but still truly random
    X -- Implement a rotation controller.
    X -- Implement a collision map controller.
      -- Implement a controller to update the collision map based off an .osu's compisition. (NAH LET'S DO THAT LATER)
    X -- Implement a controller that can change the collision map based off a list imported.
    X -- Implement a controller that can shift the collision map during a given time.
    X -- Implement circular rain.
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
        public List<CollisionNode> regionList;
        public List<ShiftNode> shiftList;

        // Method

        public void clearMap()
        {
            // Clears the map.
            NoNoRegion.clearMap();
        }

        public void importMap(string path)
        {
            // Imports a bitmap image into the collisionMap using string path.
            clearMap();
            NoNoRegion.bmp2CollisionMap(path);
        }

        public void addRegion(string path, int startTime)
        {
            // Adds a region into the region list for the map to fetch.
            regionList.Add(new CollisionNode(path, startTime));
        }

        public void addShift(int startTime, int duration, int xInp, int yInp, bool wrappingFlag)
        {
            // Adds a shift into the shift list for the map to fetch.
            shiftList.Add(new ShiftNode(startTime, duration, xInp, yInp, wrappingFlag));
        }

        public System.Drawing.Point Polar2Cartesian(double radius, double radian)
        {
            return new System.Drawing.Point((int)(radius * Math.Cos(radian)), (int)(radius * Math.Sin(radian)));
        }

        public void makeItRain(int startTime, int iterations)
        {
            // MAKE IT RAIN, BOYS
            // startTime: When do you want it to start?
            // iterations: How many times do you want the drops to wrap?

            // Does the collision map need to be updated?
            // Check if the list is not empty. If it's not empty, check the head's time
            // to see if it needs to be updated. After a successful update, pop the head.

            int iterationCount = 0;
            int timeElapsed = 0;

            while (iterationCount < iterations)
            {
                // Something to consider for the regionList
                if (regionList.Count != 0)
                {
                    // Now check if we're at the correct time to trigger this.
                    if (startTime + timeElapsed >= regionList[0].startTime)
                    {
                        // Update the map with the given path...
                        importMap(regionList[0].path);
                        // And remove the head.
                        regionList.RemoveAt(0);
                    }
                }

                // Something to consider for the shifting
                // We can only update in RAINDROP_VELOCITY snapshots
                if (shiftList.Count != 0)
                {
                    // Now check if we're at the correct time to trigger this
                    if(startTime + timeElapsed >= shiftList[0].startTime)
                    {
                        // Update the map with the corresponding shift.
                        // However, duration goes one-by-one so...
                        NoNoRegion.shiftMap(shiftList[0].tweenDistX, shiftList[0].tweenDistY, shiftList[0].wrappingFlag);

                        // Then update the duration counter.
                        // The duration counter works as iterations of RAINDROP_VELOCITY
                        shiftList[0].duration -= 1;
                        shiftList[0].startTime += BeatmapConstants.RAINDROP_VELOCITY;
                        if(shiftList[0].duration <= 0)
                        {
                            // And if it's empty, remove the head.
                            shiftList.RemoveAt(0);
                        }
                    }
                }

                // Calling all droplets to duty.
                // The raindrop will only drop if our given time is an iteration based off RAINDROP_VELOCITY
                if (timeElapsed % BeatmapConstants.RAINDROP_VELOCITY == 0)
                {
                    for (int x2 = 0; x2 < cloud.Length; x2++)
                    {
                        cloud[x2].drop(startTime + timeElapsed + rng.Next(BeatmapConstants.DROP_VARIANCE * -1, BeatmapConstants.DROP_VARIANCE),
                            rng.Next(BeatmapConstants.SCREEN_LEFT, BeatmapConstants.SCREEN_RIGHT),
                            BeatmapConstants.SCREEN_TOP - BeatmapConstants.SCREEN_TOP_OFFSET,
                            NoNoRegion,
                            false);
                    }
                    // Increment the iteration count.
                    iterationCount++;
                }

               // Queue for the next wrap.
               timeElapsed++;
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
                cloud[x2].rotate(startTime, degInput, false);
            }

            // End the rotation
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                cloud[x2].rotate(endTime, 0, false);
            }
        }

        public void circularRain(int startTime, int iterations, double radius)
        {
            // Creates a special effect utilizing the raindrops to make a circular surrounding.
            // The raindrops go towards the center to create a focus on the center of the screen.
            // NOTE: Collision maps are ignored.
            int iterationCount = 0;
            int timeElapsed = 0;

            // Remove the current collision map.
            clearMap();

            // Generate a list of coordinates for the raindrops to behave by based on
            // trigonometry and the angle of a circle.
            // What we need:
            // > a collection of tuples for X and Y values

            while (iterationCount < iterations)
            {
                // Calling all droplets to duty.
                // The raindrop will only drop if our given time is an iteration based off RAINDROP_VELOCITY
                if (timeElapsed % BeatmapConstants.RAINDROP_VELOCITY == 0)
                {
                    double angleCounter = 0;
                    for (int x2 = 0; x2 < cloud.Length; x2++)
                    {
                        // Generate Values for raindrop movement
                        var dropletStartTime = startTime + timeElapsed + rng.Next(BeatmapConstants.DROP_VARIANCE * -1, BeatmapConstants.DROP_VARIANCE);
                        var outCirclePoint = Polar2Cartesian(BeatmapConstants.SCREEN_WIDTH / 2, angleCounter);
                        var inCirclePoint = Polar2Cartesian(radius, angleCounter);

                        // Offset to center screen
                        outCirclePoint.X += (int)(BeatmapConstants.PLAYFIELD_WIDTH / 1.58);
                        inCirclePoint.X += (int)(BeatmapConstants.PLAYFIELD_WIDTH / 1.58);
                        outCirclePoint.Y += (int)(BeatmapConstants.SCREEN_HEIGHT / 1.8);
                        inCirclePoint.Y += (int)(BeatmapConstants.SCREEN_HEIGHT / 1.8);

                        cloud[x2].rotate(dropletStartTime, angleCounter, true);

                        cloud[x2].droplet.move(0,
                            dropletStartTime,
                            dropletStartTime + BeatmapConstants.RAINDROP_VELOCITY,
                            outCirclePoint.X,
                            outCirclePoint.Y,
                            inCirclePoint.X,
                            inCirclePoint.Y
                            );

                        angleCounter += Math.PI / BeatmapConstants.MAX_RAINDROPS * 2;
                    }
                    // Increment the iteration count.
                    iterationCount++;
                }

                // Queue for the next wrap.
                timeElapsed++;
            }
        }

        // Instance Constructor
        public Raincloud()
        {
            // NEW CLOUD
            rng = new Random();
            cloud = new Raindrop[BeatmapConstants.MAX_RAINDROPS];
            NoNoRegion = new CollisionMap();
            regionList = new List<CollisionNode>();
            shiftList = new List<ShiftNode>();
            clearMap();

            for(int x = 0; x < cloud.Length; x++)
            {
                cloud[x] = new Raindrop(BeatmapConstants.MIN_HEIGHT + rng.NextDouble() * BeatmapConstants.MAX_HEIGHT, 
                    rng.NextDouble() * BeatmapConstants.MAX_FADE);
            }
        }
    }
}
