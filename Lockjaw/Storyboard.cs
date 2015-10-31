using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SGL.Framework;
using SGL.Storyboard.Commands;

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


            // So this part is dedicated to generating the rain...
            var mainCloud = new Raincloud();

            // Initialize rain
            mainCloud.correlateMapToImage("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\krool.bmp", "sb\\krool_pursuit.png", BeatmapConstants.SONG_BOOK3, 16);

            // Generate rain
            mainCloud.makeItRain(0,(int)(BeatmapConstants.SONG_BOOK7 / BeatmapConstants.RAINDROP_VELOCITY));
            mainCloud.createWind(0, 0, 0);
            mainCloud.stackLightning(BeatmapConstants.SONG_BOOK3,48);

            // Pursuit
            //mainCloud.addRegion("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\tryhard.bmp", BeatmapConstants.SONG_BOOK3);

            // Chorus phase
            // Generating the rain
            mainCloud.circularRain(BeatmapConstants.SONG_BOOK7, (int)((BeatmapConstants.SONG_BOOK8 - BeatmapConstants.SONG_BOOK7) / BeatmapConstants.RAINDROP_VELOCITY), 128, true);

            // Pass/Fail for this portion
            var logoGood = SB.Sprite("sb\\dk.png", SB.Pass, SB.Centre);
            var logoBad = SB.Sprite("sb\\krem.png", SB.Fail, SB.Centre);

            logoGood.move(0, 0, 0, 320, 240, 320, 240);
            logoBad.move(0, 0, 0, 320, 240, 320, 240);
            logoGood.scale(0, 0, 0, 0.3, 0.3);
            logoBad.scale(0, 0, 0, 0.3, 0.3);

            logoGood.fade(0, BeatmapConstants.SONG_BOOK7, BeatmapConstants.SONG_BOOK7, 0, 1);
            logoBad.fade(0, BeatmapConstants.SONG_BOOK7, BeatmapConstants.SONG_BOOK7, 0, 1);

            logoGood.fade(0, BeatmapConstants.SONG_BOOK8 - (int)(BeatmapConstants.BEAT_QUARTER) * 4, BeatmapConstants.SONG_BOOK8, 1, 0);
            logoBad.fade(0, BeatmapConstants.SONG_BOOK8 - (int)(BeatmapConstants.BEAT_QUARTER) * 4, BeatmapConstants.SONG_BOOK8, 1, 0);

            // Spiral portion at end
            var spiralRaincloud = new Raincloud();
            spiralRaincloud.hideAll(0, false);
            spiralRaincloud.spiralRain( (BeatmapConstants.SONG_BOOK8 - (int)(BeatmapConstants.BEAT_QUARTER)*4), 12);

            // Pass/Fail lightning
            var passFailStrobe = SB.Sprite("sb\\foo.png", SB.Background, SB.Centre);
            passFailStrobe.move(0, 0, 320, 240, 320, 240);
            passFailStrobe.scale(0, 0, 0, 1366, 768);
            passFailStrobe.fade(0, 0);

            passFailStrobe.startTriggerLoop("Passing", BeatmapConstants.SONG_BOOK7, BeatmapConstants.SONG_BOOK8);
            passFailStrobe.fade(0, 0, (int)BeatmapConstants.BEAT_QUARTER*2, 0.5, 0);
            passFailStrobe.endLoop();

            
            passFailStrobe.startTriggerLoop("Failing", BeatmapConstants.SONG_BOOK7, BeatmapConstants.SONG_BOOK8);
            passFailStrobe.fade(0, 0, (int)BeatmapConstants.BEAT_QUARTER * 2, 1, 0);
            passFailStrobe.endLoop();
            

            /*

            // OLD CODE

            // Background
            var tempBG = SB.Sprite("sb\\kq.png", SB.Background, SB.TopCentre);
            tempBG.move(0, 0, 0, 320, -10, 320, -10);
            tempBG.fade(0, 0, BeatmapConstants.SONG_END_OFFSET, 1, 1);
            
            // Rain particles
            var peppyOsu = new Raincloud();

            //peppyOsu.NoNoRegion.hardFill(60, 300, 60, 250);
            //peppyOsu.importMap("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\dummy.bmp");

            // Region List editing...
            peppyOsu.addRegion("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\rectOK.bmp", 19257);
            peppyOsu.addRegion("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\rectOK2.bmp", 38457);
            peppyOsu.addRegion("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\circleTest.bmp", 48057);
            peppyOsu.addRegion("D:\\Alice\\osu!\\C sharp codes\\LOCKJAW\\Lockjaw\\img\\tryHard.bmp", 52857);

            //peppyOsu.addShift(62437, 4822, 4822, 0, true);
            //peppyOsu.addShift(62437, 4822, 600, 0, true);
            peppyOsu.addShift(62437, 16, 4822, 0, true);
            peppyOsu.addShift(67257, 16, -4822, 0, true);
            peppyOsu.addShift(72057, 16, 0, 4822, true);
            peppyOsu.addShift(76857, 16, 4822, 4822, true);
            peppyOsu.addShift(81657, 16, 4822, -4822, true);

            peppyOsu.makeItRain(400, (int)(86457 / BeatmapConstants.RAINDROP_VELOCITY));
            peppyOsu.circularRain(86457,(int)( (96057-86457) / BeatmapConstants.RAINDROP_VELOCITY ), 128, true);

            var spiralRaincloud = new Raincloud();
            spiralRaincloud.hideAll(0, false);
            spiralRaincloud.spiralRain(96057, 18);

            peppyOsu.createWind(0, 0, 0);

            peppyOsu.createWind(62457, 70857, 15);

            peppyOsu.createWind(72057, 80457, -15);

            peppyOsu.createWind(96056, 96056, 0);

            // Cool cinematics buzzkills
            var topCutsceneBars = SB.Sprite("sb\\foo.png", SB.Foreground, SB.TopCentre);
            topCutsceneBars.color(System.Drawing.Color.LimeGreen);
            topCutsceneBars.fade(0.2);
            topCutsceneBars.scaleVec(BeatmapConstants.SCREEN_WIDTH, 180);
            topCutsceneBars.move(SGL.Storyboard.Commands.EasingTypes.InBounce, 19257, 19257 + (int)BeatmapConstants.BEAT_QUARTER * 4, 0, 0, 0, 180);

            var botCutsceneBars = SB.Sprite("sb\\foo.png", SB.Foreground, SB.BottomCentre);
            botCutsceneBars.color(System.Drawing.Color.PowderBlue);
            botCutsceneBars.fade(0.2);
            botCutsceneBars.scaleVec(BeatmapConstants.SCREEN_WIDTH, 180);
            botCutsceneBars.move(SGL.Storyboard.Commands.EasingTypes.InBounce, 19257, 19257 + (int)BeatmapConstants.BEAT_QUARTER * 4, 0, 480, 0, 300);

            */

        }
    }
}