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

        // Beat Snap Divisors
        public const double BEAT_QUARTER = 300;
        public const double BEAT_EIGHTH = BEAT_QUARTER / 2;
        public const double BEAT_SIXTEENTH = BEAT_QUARTER / 4;


        // !!!!
        // WINDOW SPECIFIC CONSTANTS
        // !!!!

        // Screen and Playfield Dimensions
        public const int SCREEN_WIDTH = 1366;
        public const int SCREEN_HEIGHT = 768;
        public const int PLAYFIELD_WIDTH = 510;
        public const int PLAYFIELD_HEIGHT = 385;

        // !!!!
        // STORYBOARD SPECIFIC CONSTANTS
        // !!!!
    }
}
