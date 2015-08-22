using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lockjaw
{
    static class BeatmapConstants 
    {
        // !!!!
        // SONG SPECIFIC CONSTANTS
        // !!!!

        // Timing
        public const double SONG_BPM = 200.00;
        public const int SONG_OFFSET = 9657;
        public const int SONG_END_OFFSET = 124749;

        // Beat Snap Divisors
        public const double BEAT_QUARTER = 300;
        public const double BEAT_EIGHTH = BEAT_QUARTER / 2;
        public const double BEAT_SIXTEENTH = BEAT_QUARTER / 4;


        // !!!!
        // WINDOW SPECIFIC CONSTANTS
        // !!!!

        // Screen and Playfield Dimensions
        public const int SCREEN_WIDTH = 854;
        public const int SCREEN_HEIGHT = 480;
        public const int PLAYFIELD_WIDTH = 510;
        public const int PLAYFIELD_HEIGHT = 385;

        // Minimum and Maximum Locations due to Widescreen Support
        public const int SCREEN_LEFT = -120;
        public const int SCREEN_RIGHT = SCREEN_WIDTH + SCREEN_LEFT;
        public const int SCREEN_TOP = -5;
        public const int SCREEN_BOTTOM = SCREEN_HEIGHT + SCREEN_TOP;

        // !!!!
        // STORYBOARD SPECIFIC CONSTANTS
        // !!!!

        // Raindrop Dimensions
        public const int RAINDROP_WIDTH = 4;
        public const int RAINDROP_HEIGHT = 114;
        
        // Raindrop Velocity (time it takes from top to bottom)
        public const int RAINDROP_VELOCITY = 600;

        // Maximum Raindrops on the screen per cloud
        public const int MAX_RAINDROPS = 500;

        // Height Ratio settings
        public const double MIN_HEIGHT = 0.1;
        public const double MAX_HEIGHT = 2.5;

        // Fade settings
        public const double MAX_FADE = 0.75;

        // Make the Raindrops' Y locations vary too!
        public const int DROP_VARIANCE = 400;
    }
}
