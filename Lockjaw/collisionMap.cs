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
}
