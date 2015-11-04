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
        public const double BEAT_QUARTER = 1 / SONG_BPM * 60 * 1000;
        public const double BEAT_EIGHTH = BEAT_QUARTER / 2;
        public const double BEAT_SIXTEENTH = BEAT_QUARTER / 4;

        // Checkpoints
        public const int SONG_BOOK1 = 9657;     // Song begin
        public const int SONG_BOOK2 = 19257;    // Buildup
        public const int SONG_BOOK3 = 38457;    // The pursuit section
        public const int SONG_BOOK4 = 52857;    // Break
        public const int SONG_BOOK5 = 57657;    // Break end, drums
        public const int SONG_BOOK6 = 62457;    // Pursuit Part 2
        public const int SONG_BOOK7 = 86457;    // Chorus
        public const int SONG_BOOK8 = 105657;   // Outro
        public const int SONG_BOOK9 = 115257;   // ENDING

        // Non-bookmarked checkpoints
        public const int SONG_BOOK6_A = 81657;  // EXPAND DONG HYPE

        // !!!!
        // WINDOW SPECIFIC CONSTANTS
        // !!!!

        // Screen and Playfield Dimensions
        public const int SCREEN_WIDTH = 854;
        public const int SCREEN_HEIGHT = 480;
        public const int PLAYFIELD_WIDTH = 510;
        public const int PLAYFIELD_HEIGHT = 385;

        // Minimum and Maximum Locations due to Widescreen Support
        public const int SCREEN_LEFT = -107;
        public const int SCREEN_RIGHT = SCREEN_WIDTH + SCREEN_LEFT;
        public const int SCREEN_TOP = 0;
        public const int SCREEN_BOTTOM = SCREEN_HEIGHT + SCREEN_TOP;

        public const int SCREEN_TOP_OFFSET = 20;

        public const int PLAYFIELD_OFFSET_X = 64;
        public const int PLAYFIELD_OFFSET_Y = 56;

        // !!!!
        // STORYBOARD SPECIFIC CONSTANTS
        // !!!!

        // Raindrop Dimensions
        public const int RAINDROP_WIDTH = 4;
        public const int RAINDROP_HEIGHT = 114;

        // Raindrop Velocity (time it takes from top to bottom)
        // In other words, this is how much ms it takes to go from
        // y to SCREEN_HEIGHT + heightOffset
        public const int RAINDROP_VELOCITY = 600;

        // Maximum Raindrops on the screen per cloud
        public const int MAX_RAINDROPS = 1000;

        // Height Ratio settings
        public const double MIN_HEIGHT = 0.1;
        public const double MAX_HEIGHT = 2.5;

        // Fade settings
        public const double MAX_FADE = 0.6;

        // Make the Raindrops' Y locations vary too!
        public const int DROP_VARIANCE = 400;

        // Max rotation distance for a raindrop
        // This will determine angleOffset
        public const int MAX_ROTATION_DISTANCE = SCREEN_RIGHT;
    }
}
