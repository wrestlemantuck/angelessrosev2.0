using static StupidTemplate.Menu.Main;
using StupidTemplate.Notifications;
using static StupidTemplate.Settings;

namespace StupidTemplate.Mods.Settings
{
    public class Movement
    {
        public static int flySpeedIndex = 2;
        public static float flySpeed = 15f; // Again, this is from the original source of II Stupid template.

        public static void ChangeLongArmLength()
        {
            if (LongArmsLength == "Normal")
            {
                LongArmsLength = "Medium";
                NotifiLib.SendNotification("Changed long arm length to medium");
            }
            else if (LongArmsLength == "Medium")
            {
                LongArmsLength = "Large";
                NotifiLib.SendNotification("Changed long arm length to large");
            }
            else if (LongArmsLength == "Large")
            {
                LongArmsLength = "Normal";
                NotifiLib.SendNotification("Changed long arm length to normal");
            }
        }

        public static void ChangeFlySpeed()
        {
            string[] speedNames = new string[] { "Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Extreme" };
            float[] speedValues = new float[] { 5f, 10f, 15f, 20f, 30f, 50f }; // Seems to not work? I havent made the fly mod, that was from II Stupid template.

            flySpeedIndex++;
            flySpeedIndex %= speedNames.Length;

            GetIndex("Change Fly Speed").overlapText = $"Change Fly Speed [{speedNames[flySpeedIndex]}]"; // I dont prefer this. I prefer to have Notifications. But i CBA to change this.
        }
    }
}
