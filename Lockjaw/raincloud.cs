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
      -- Implement a controller to update the collision map based off an .osu's composition. (NAH LET'S DO THAT LATER)
    X -- Implement a controller that can change the collision map based off a list imported.
    X -- Implement a controller that can shift the collision map during a given time.
    X -- Implement circular rain.
    X -- Implement rain that follows an Archimedes spiral.
      -- Better transition for when rain ends (that isn't the traditional controller)
    X -- Implement a lightning flash function.
    X -- Implement a smart controller for the lightning.
      -- Create a collisionMap and image correlator.
*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGL.Framework;
using SGL.Storyboard.Commands;

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
        SGL.Storyboard.Generators.Visual.SpriteGenerator strobeLight = SB.Sprite("sb\\foo.png", SB.Background, SB.Centre);

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

        public void hideUntil(int startTime)
        {
            // Sets the fade from 0 to the specified point
            // But then set the fade as visible AT that point
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                cloud[x2].droplet.fade(0, startTime - BeatmapConstants.RAINDROP_VELOCITY, 0, 0);
                cloud[x2].droplet.fade(startTime - BeatmapConstants.RAINDROP_VELOCITY, startTime, 0, cloud[x2].fadeSetting);
            }
        }

        public void hideAll(int startTime, bool tween)
        {
            // Sets the fade for all raindrops from
            // startTime to 0. The rate is based on
            // the RAINDROP_VELOCITY parameter.
            // If tween is false, it is an instantaneous hide.
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                if(tween)
                    cloud[x2].droplet.fade(0, startTime, startTime + BeatmapConstants.RAINDROP_VELOCITY, cloud[x2].fadeSetting, 0);
                else
                    cloud[x2].droplet.fade(0, startTime, startTime, 0, 0);
            }
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
            // having them move based off the angleOffset. Because they are in degrees,
            // the Boolean is set to false. (Otherwise, it's rads)
            // Note that this also needs to consider the raindrop's velocity
            // Assume that no wind is rad 0.

            // Begin the rotation
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                cloud[x2].rotate(startTime, degInput, false);
                cloud[x2].rotate(endTime, 0, false);
            }

        }

        public void circularRain(int startTime, int iterations, double radius, bool fadeEnding)
        {
            // Creates a special effect utilizing the raindrops to make a circular surrounding.
            // The raindrops go towards the center to create a focus on the center of the screen.
            // NOTE: Collision maps are ignored.
            int iterationCount = 0;
            int timeElapsed = 0;

            // Remove the current collision map.
            clearMap();

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
                        outCirclePoint.X += (int)(BeatmapConstants.PLAYFIELD_WIDTH / 1.6);
                        inCirclePoint.X += (int)(BeatmapConstants.PLAYFIELD_WIDTH / 1.6);
                        outCirclePoint.Y += (int)(BeatmapConstants.SCREEN_HEIGHT / 1.9);
                        inCirclePoint.Y += (int)(BeatmapConstants.SCREEN_HEIGHT / 1.9);

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

            //  Fade all droplets out as the raindrop is done.
            if(fadeEnding)
                hideAll(startTime + timeElapsed - BeatmapConstants.RAINDROP_VELOCITY, true);
        }

        public void spiralRain(int startTime, double rate)
        {
            // Raincloud controller that forces a raincloud to move in an
            // Archimedes spiral. Will execute one spiral.

            // Hide
            hideUntil(startTime + cloud.Length);

            // Remove the current collision map.
            clearMap();

            int delayCount = 0;

            double angleCounter = 0;

            // This outer loop navigates through all the rain droplets
            for (int x2 = 0; x2 < cloud.Length; x2++)
            {
                bool stateMachine = false;
                bool zeroState = false;
                int dropTime = startTime; 

                cloud[x2].rotate(startTime, angleCounter, true);

                // This inner loop handles the spiral algorithm
                for(double point = 0; point < BeatmapConstants.SCREEN_WIDTH && startTime < BeatmapConstants.SONG_END_OFFSET; point += rate)
                {
                    if(stateMachine)
                    {
                        // Run x-spiral
                        cloud[x2].droplet.moveX(SGL.Storyboard.Commands.EasingTypes.InSine, dropTime + delayCount, dropTime + BeatmapConstants.RAINDROP_VELOCITY + delayCount, cloud[x2].droplet.X, (2 * (Convert.ToInt32(zeroState) - 1) * point));
                        cloud[x2].droplet.moveY(SGL.Storyboard.Commands.EasingTypes.OutSine, dropTime + delayCount, dropTime + BeatmapConstants.RAINDROP_VELOCITY + delayCount, cloud[x2].droplet.Y, 240 + rng.Next( (int)(BeatmapConstants.DROP_VARIANCE * -1 * 0.25) , (int)(BeatmapConstants.DROP_VARIANCE * 0.25) ) );
                        zeroState = !zeroState;
                    }
                    else
                    {
                        // Run y-spiral
                        cloud[x2].droplet.moveY(SGL.Storyboard.Commands.EasingTypes.InSine, dropTime + delayCount, dropTime + BeatmapConstants.RAINDROP_VELOCITY + delayCount, cloud[x2].droplet.Y, (2 * (Convert.ToInt32(zeroState) - 1) * point));
                        cloud[x2].droplet.moveX(SGL.Storyboard.Commands.EasingTypes.OutSine, dropTime + delayCount, dropTime + BeatmapConstants.RAINDROP_VELOCITY + delayCount, cloud[x2].droplet.X, 640 + rng.Next((int)(BeatmapConstants.DROP_VARIANCE * -1 * 0.25), (int)(BeatmapConstants.DROP_VARIANCE * 0.25) ) );
                    }

                    // Change for next iteration for the point
                    stateMachine = !stateMachine;
                    dropTime += BeatmapConstants.RAINDROP_VELOCITY;
                }
                
                // Delay for next states
                delayCount += 1;
                angleCounter += Math.PI / cloud.Length;
            }

        }

        public void initializeLightning()
        {
            // Procedure to initialize lightning. Used to help clean code.
            strobeLight.move(0, 0, 320, 240, 320, 240);
            strobeLight.scaleVec(0, 0, 0, 1366, 768, 1366, 768);
            strobeLight.fade(0, 0);
            //strobeLight.color(0, 0, System.Drawing.Color.White, System.Drawing.Color.White); // I think it's already white
        }

        public void createLightning(int startTime, double intensity)
        {
            // Generates a lightning strobe. Lightning strobes have
            // a quarter beat length of flash.
            strobeLight.fade((EasingTypes)rng.Next(0,31), startTime, startTime + (int)BeatmapConstants.BEAT_QUARTER*rng.Next(1,4), intensity, 0);
        }

        public void stackLightning(int startTime, int iterations)
        {
            // During this portion of time, lightning will randomly spawn
            // for x measures, determined by iterations.
            // The intensity is also variable too.
            for(int i = 0; i < iterations; ++i)
            {
                if(rng.Next(0,10) > 3)
                {
                    // Generate lightning
                    createLightning(startTime, (rng.Next(0,73)*0.01));
                }
                startTime += (int)BeatmapConstants.BEAT_QUARTER * 4;
            }

        }

        public void correlateMapToImage(string mapPath, string assetName, int startTime, int iterations)
        {
            // Will correspond a collision map to an asset.
            // Iterations are series of measures.

            clearMap();
            addRegion(mapPath, startTime);
            var mapSprite = SB.Sprite(assetName, SB.Foreground, SB.Centre);
            mapSprite.move(0, 0, 320, 330, 320, 330);
            mapSprite.fade(0, 0, 0, 0, 0);
            
            for(int i = 0; i < iterations; i++)
            {
                mapSprite.fade(0, startTime, startTime, 1, 1);
                startTime += (int)BeatmapConstants.BEAT_QUARTER * 4;
            }
            mapSprite.fade(0, startTime, startTime, 0, 0);
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
            initializeLightning();
            clearMap();

            for(int x = 0; x < cloud.Length; x++)
            {
                cloud[x] = new Raindrop(BeatmapConstants.MIN_HEIGHT + rng.NextDouble() * BeatmapConstants.MAX_HEIGHT, 
                    rng.NextDouble() * BeatmapConstants.MAX_FADE);
            }
        }
    }
}
