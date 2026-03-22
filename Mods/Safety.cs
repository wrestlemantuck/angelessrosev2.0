using GorillaLocomotion;
using StupidTemplate.Classes;
using StupidTemplate.Notifications;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;
using static StupidTemplate.Classes.RigManager;
using static StupidTemplate.Menu.Main;
using Photon.Pun;


namespace StupidTemplate.Mods
{
    public class Safety
    {
        public static void SpoofColor()
        {
            if (GorillaTagger.Instance == null)
                NotifiLib.SendNotification("Cant find gorillatagger (maybe join a lobby?)");
            GorillaTagger.Instance.UpdateColor(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        }
        private static float nextRefreshTime = 0f;
        private static MethodInfo refreshMethod;

        [System.Obsolete]
        public static void RPCSafety()
        {
            if (Time.time < nextRefreshTime)
                return;

            MonkeAgent agent = Object.FindObjectOfType<MonkeAgent>();
            if (agent == null)
                return;

            if (refreshMethod == null)
            {
                refreshMethod = typeof(MonkeAgent).GetMethod(
                    "RefreshRPCs",
                    BindingFlags.NonPublic | BindingFlags.Instance 
                );
            }

            if (refreshMethod != null)
            {
                refreshMethod.Invoke(agent, null);
                NotifiLib.SendNotification("[RPCSafety] Refreshed RPCS");
            }
            else
            {
                NotifiLib.SendNotification("[RPCSafety] Failed to find RefreshRPCs method");
            }

            nextRefreshTime = Time.time + 30f; // 30 seconds, change 30f to whatever invertal you want the Refresh RPCS to run
        }
    }
}