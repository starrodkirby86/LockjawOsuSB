/*

CLASS: CollisionMap
    Map region where the rain is allowed to fall. The collision map
    is a 2D array overlay of the playscreen that has boolean flags
    of whether or not a particle can fall to that space.

    There are special properties to make the rain seem surreal
    and it looks like Moses is doing something (not makin' it rain, that's already going).

    TODO:
    X -- Load a bitmap file and return a 2D array based on RGB pixel color.
     -- Shift collision map a certain direction.
     -- Correspond .osu hitcircles to collision map. (NAH LET'S DO THIS ANOTHER TIME)
    X -- Create a class that stores a collection of strings and timepoints that can be loaded.
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGL.Framework;

namespace Lockjaw
{
    public class CollisionMap
    {
        // Class members:

        // Property
        Bitmap sourceImage;
        public bool[,] map;

        // Method

        public void bmp2CollisionMap(string path)
        {
            // Loads a bitmap file given by 'path', then checks
            // the pixels if it's white or nonwhite (preferably black).
            // The corresponding pixel would then be converted to the 2D boolean map.
            // WHITE = false
            // BLACK = true

            // Load the bitmap. (You know, I should really implement try-catch methods lol)
            sourceImage = new Bitmap(path, true);

            // Make some temporary comparison variables to make life easier.
            Color sourcePixelColor;


            // GetPixel mania!
            for (int x1 = 0; x1 < sourceImage.Width; x1++)
            {
                for (int x2 = 0; x2 < sourceImage.Height; x2++)
                {
                    sourcePixelColor = sourceImage.GetPixel(x1, x2);
                    map[x1,x2] = sourcePixelColor.ToArgb().Equals(Color.Black.ToArgb());
                }
            }

        }

        public void clearMap()
        {
            // Empties the map to be fully transparent.
            for(int x1 = 0; x1 < BeatmapConstants.SCREEN_WIDTH; x1++)
            {
                for(int x2 = 0; x2 < BeatmapConstants.SCREEN_HEIGHT; x2++)
                {
                    map[x1,x2] = false;
                }
            }
        }

        public void hardFill(int x0, int x1, int y0, int y1)
        {
            // Rectangle fill a spot with true.
            for(int i = x0; i < x1; i++)
            {
                for(int j = y0; j < y1; j++)
                {
                    map[i, j] = true;
                }
            }
        }

        public void shiftMap(int xDist, int yDist, bool wrappingFlag)
        {
            // Shifts map a given distance in either the horizontal or vertical direction.
            // Also has a screen wrapping parameter.
            // Wrapping:
            //      False - No wrapping, new existing content will be false by default
            //      True - Contents on one edge will go to the other edge

            // Make a copy of the map to keep the data integrity.
            var dupeMap = map;

            // Clear the original map.
            clearMap();

            // Begin duplicating.
            for (int x1 = 0; x1 < sourceImage.Width; x1++)
            {
                for (int x2 = 0; x2 < sourceImage.Height; x2++)
                {
                    var finalX = x1 + xDist;
                    var finalY = x2 + yDist;

                    var overBoundFlag = (finalX) >= sourceImage.Width || (finalY) >= sourceImage.Height;
                    var underBoundFlag = (finalX) < 0 || (finalY) < 0;

                    // SHIFT checks for OOB
                    if ( overBoundFlag && !underBoundFlag )
                    {
                        if(wrappingFlag)
                        {
                            // Which item exceeds that boundary?
                            var xBound = (x1 + xDist) >= sourceImage.Width;
                            var yBound = (x2 + yDist) >= sourceImage.Height;

                            // Calculation...

                            if(xBound)
                            {
                                finalX -= sourceImage.Width;
                            }

                            if(yBound)
                            {
                                finalY -= sourceImage.Height;
                            }

                            // And that's a wrap.
                            map[finalX, finalY] = dupeMap[x1, x2];
                        }

                        // If it's here, then we can just ignore it since the map is already white anyway.
                    }
                    else if(!overBoundFlag && underBoundFlag )
                    {
                        if(wrappingFlag)
                        {
                                // Which item is underneath that boundary?
                            var xBound = (x1 + xDist) < 0;
                            var yBound = (x2 + yDist) < 0;

                            // Calculation...

                            if (xBound)
                            {
                                finalX += sourceImage.Width;
                            }

                            if (yBound)
                            {
                                finalY += sourceImage.Height;
                            }

                            // And that's a wrap.
                            map[finalX, finalY] = dupeMap[x1, x2];
                         }

                        // No wrap means ignored...
                    }
                    else if(overBoundFlag && underBoundFlag)
                    {
                        // There are two scenarios that would make this case possible:
                        // X is overachieving and Y is underachieving
                        // X is underachieving and Y is overachieving
                        // Therein, our conditions are going to pinpoint on that.

                        if (wrappingFlag)
                        {
                            // Scenario 1 and 2 checks
                            if (finalX >= sourceImage.Width)
                            {
                                finalX -= sourceImage.Width;
                                finalY += sourceImage.Height;
                            }
                            else
                            {
                                finalX += sourceImage.Width;
                                finalY -= sourceImage.Height;
                            }

                            // And that's a wrap.
                            map[finalX, finalY] = dupeMap[x1, x2];
                        }

                        // No wrap means ignored...
                    }
                    else
                    {
                        // No OOB whatsoever.
                        map[finalX, finalY] = dupeMap[x1, x2];
                    }
                 }
            }
        }

        // Instance Constructor
        public CollisionMap()
        {
            // Initialize the contents (Image stays empty though !!! caution)
            map = new bool[BeatmapConstants.SCREEN_WIDTH,BeatmapConstants.SCREEN_HEIGHT];
        }
    }

    public class CollisionNode
    {
        // Used for a List to help fetch and receive data of collision maps
        // Class members:
        public string path;
        public int startTime;
        // Property
        // Method
        // Instance Constructor
        public CollisionNode(string pathInp = "null", int startTimeInp = 0)
        {
            path = pathInp;
            startTime = startTimeInp;
        }
    }

    public class ShiftNode
    {
        // Used for a list to help control when and where the collision map should shift directions
        // Just a note regarding tween: The divided values must yield to be whole numbers in the end

        // Class members:

        // Properties
        public int startTime;
        public int duration;
        public int xInp;
        public int yInp;
        public bool wrappingFlag;

        // These guys are for tweening purposes
        public int tweenDistX;
        public int tweenDistY;

        // Property
        // Method
        // Instance Constructor
        public ShiftNode(int startInp = 0, int durationInp = 0, int xInpInp = 0, int yInpInp = 0, bool wrappingFlagInp = false)
        {
            startTime = startInp;
            duration = durationInp;
            xInp = xInpInp;
            yInp = yInpInp;
            wrappingFlag = wrappingFlagInp;

            tweenDistX = xInp / duration;
            tweenDistY = yInp / duration;
        }
    }
}
