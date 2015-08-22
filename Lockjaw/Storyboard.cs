using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGL.Framework;

// Ensure that the Namespace is the same as your
// project's name. (So in this case, because this
// project is named Lockjaw, I named it Lockjaw!
// We want to make sure we're in the domain, after all!

namespace Lockjaw
{
    public class Storyboard : BaseStoryboard
    {

        public override String GetSongFolder()
        {
            // Type the directory of your beatmap's song folder here.
            return @"D:\Cirno\Rhythm\osu!\Songs\lockjaw";
        }

        public override String GetOsbPath()
        {
            // Type the filename of your beatmap's OSB file here.
            return "David Wise, Kenji Yamamoto - Irate Eight (Tension) ~ Lockjaw's Saga (Starrodkirby86).osb";
        }

        public override bool IsWidescreen()
        {
            // No need to mess with this. Though if you want to have
            // your map not use widescreen, set this to false.
            return true;
        }

        public override void DeclareMaps(List<Map> maps)
        {
            // If you have difficulty-exclusive storyboards, then
            // add it here so that the program knows its existence.
            //
            // Format:
            // var map1 = new Map("testsong (Damnae) [testdiff1].osu");
            // map1.Put("someProperty", 1);
            // maps.Add(map1);

        }

        public override void GenerateMapStoryboard(Map map)
        {
            // Any maps that have been instantiated in the DeclareMaps method
            // can be called here. The generation is simply based on
            // the someProperty ID given in DeclareMaps.
            //
            // Example:
            // var someProperty = (Int32)map.Get("someProperty");
            //
            // if (someProperty == 1)
            // {
            //     var sprite = SB.Sprite("dot2.png", SB.Foreground, SB.TopCentre);
            //     sprite.move(0, 10000, 320, 240, 340, 360);
            // }
            // dot2.png is now exclusive to testdiff1.


        }

        public override void GenerateStoryboard()
        {
            // Main code goes here.
            // All code in here will belong in the .osb file.

            var peppyOsu = new Raincloud();
            //peppyOsu.makeItRain(400, 1);
            //peppyOsu.importMap("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\dummy.bmp");
            peppyOsu.makeItRain(400, (int)(BeatmapConstants.SONG_END_OFFSET / BeatmapConstants.RAINDROP_VELOCITY)) ;

            var secondWave = new Raincloud();
            secondWave.importMap("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\dummy.bmp");
            secondWave.makeItRain(400, (int)(BeatmapConstants.SONG_END_OFFSET / BeatmapConstants.RAINDROP_VELOCITY));

            peppyOsu.createWind(62457, 70857, 15);
            secondWave.createWind(62457, 70857, 15);

            peppyOsu.createWind(72057, 80457, -15);
            secondWave.createWind(72057, 80457, -15);

        }
    }
}
