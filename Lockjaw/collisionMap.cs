/*

CLASS: CollisionMap
    Map region where the rain is allowed to fall. The collision map
    is a 2D array overlay of the playscreen that has boolean flags
    of whether or not a particle can fall to that space.

    There are special properties to make the rain seem surreal
    and it looks like Moses is doing something (not makin' it rain, that's already going).

    TODO:
    -- Load a bitmap file and return a 2D array based on RGB pixel color.
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

            // GetPixel mania!
            for (int x1 = 0; x1 < BeatmapConstants.SCREEN_WIDTH; x1++)
            {
                for (int x2 = 0; x2 < BeatmapConstants.SCREEN_HEIGHT; x2++)
                {
                    map[x1, x2] = (sourceImage.GetPixel(x1, x2) != Color.White);
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

        // Instance Constructor
        public CollisionMap()
        {
            // Initialize the contents (Image stays empty though !!! caution)
            map = new bool[BeatmapConstants.SCREEN_WIDTH,BeatmapConstants.SCREEN_HEIGHT];
        }
    }
}
