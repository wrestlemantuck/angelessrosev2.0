using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;

namespace StupidTemplate.Classes
{
    public class RigManager
    {
        public static VRRig GetVRRigFromPlayer(Player p) =>
            GorillaGameManager.instance.FindPlayerVRRig(p);

        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            VRRig[] rigs = Object.FindObjectsOfType<VRRig>();

            if (!includeSelf)
                rigs = rigs.Where(r => r != VRRig.LocalRig).ToArray();

            if (rigs.Length == 0)
                return null;

            return rigs[Random.Range(0, rigs.Length)];
        }

        public static VRRig GetClosestVRRig()
        {
            float closest = float.MaxValue;
            VRRig outRig = null;

            VRRig[] rigs = Object.FindObjectsOfType<VRRig>();

            foreach (VRRig rig in rigs)
            {
                if (rig == VRRig.LocalRig) continue;

                float dist = Vector3.Distance(
                    GorillaTagger.Instance.bodyCollider.transform.position,
                    rig.transform.position
                );

                if (dist < closest)
                {
                    closest = dist;
                    outRig = rig;
                }
            }

            return outRig;
        }

        public static PhotonView GetPhotonViewFromVRRig(VRRig p) =>
            (PhotonView)Traverse.Create(p).Field("photonView").GetValue();

        public static Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
                return PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)];
            else
                return PhotonNetwork.PlayerListOthers[Random.Range(0, PhotonNetwork.PlayerListOthers.Length)];
        }

        public static Player GetPlayerFromVRRig(VRRig p) =>
            GetPhotonViewFromVRRig(p).Owner;

        public static Player GetPlayerFromID(string id)
        {
            Player found = null;

            foreach (Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }

            return found;
        }

        public static Color GetPlayerColor(VRRig Player)
        {
            if (Player.bodyRenderer.cosmeticBodyType == GorillaBodyType.Skeleton)
                return Color.green;

            switch (Player.setMatIndex)
            {
                case 1:
                    return Color.red;

                case 2:
                case 11:
                    return new Color32(255, 128, 0, 255);

                case 3:
                case 7:
                    return Color.blue;

                case 12:
                    return Color.green;

                default:
                    return Player.playerColor;
            }
        }
    }
}