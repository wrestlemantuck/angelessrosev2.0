using GorillaLocomotion;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.XR;
using StupidTemplate.Notifications;
using static StupidTemplate.Settings;
using static StupidTemplate.Menu.Main;
using Oculus.Interaction.Input;

namespace StupidTemplate.Mods
{
    public class Movement
    {
        public static void GhostMonke()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }
        public static void Noclip()
        {
            bool thing = ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f;
            MeshCollider[] meshColliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            foreach (MeshCollider meshCollider in meshColliders)
            {
                meshCollider.enabled = !thing;
            }
        }
        public static void CarMonke()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.1f)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * 15f; // 15f is speed
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.headCollider.transform.forward * Time.deltaTime * -15f; // -15f is speed
            }
        }

        public static void UCSpeedBoost()
        {
            GTPlayer.Instance.maxJumpSpeed = 8.5f;
        }
        public static void CSpeedBoost()
        {
            GTPlayer.Instance.maxJumpSpeed = 9.5f;
            GTPlayer.Instance.maxJumpSpeed = 9.5f;
        }
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;                                     
            }
        }

                public static void DisableLongArms()
        {
            GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        public static GameObject platl;
        public static GameObject platr;

        public static void NoTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = false;
        }
        public static void ForceTagFreeze()
        {
            GorillaLocomotion.GTPlayer.Instance.disableMovement = true;
        }

        public static void EnableLongArms()
        {
            if (LongArmsLength == "Normal")
            {
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            }
            if (LongArmsLength == "Medium")
            {
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            }
            if (LongArmsLength == "Large")
            {
                GorillaLocomotion.GTPlayer.Instance.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
            }
        }

        public static void Platforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (platl == null)
                {
                    platl = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platl.transform.position = TrueLeftHand().position;
                    platl.transform.rotation = TrueLeftHand().rotation;

                    FixStickyColliders(platl);

                    ColorChanger colorChanger = platl.AddComponent<ColorChanger>();
                    colorChanger.colors = StupidTemplate.Settings.backgroundColor;
                }
                else
                {
                    if (platl != null)
                    {
                        Object.Destroy(platl);
                        platl = null;
                    }
                }
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                if (platr == null)
                {
                    platr = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platr.transform.position = TrueRightHand().position;
                    platr.transform.rotation = TrueRightHand().rotation;

                    FixStickyColliders(platr);

                    ColorChanger colorChanger = platr.AddComponent<ColorChanger>();
                    colorChanger.colors = StupidTemplate.Settings.backgroundColor; // becomes the background color
                }
                else
                {
                    if (platr != null)
                    {
                        Object.Destroy(platr);
                        platr = null;
                    }
                }
            }
        }


        // vars for TeleportGun below
        public static bool previousTeleportTrigger;
        public static float maxTeleportDistance = 60f;

        public static void TeleportGun()
        {
        if (!ControllerInputPoller.instance.rightGrab)
        {
        if (GunPointer != null)
        GunPointer.SetActive(false);

            if (GunLine != null)
                GunLine.gameObject.SetActive(false);

            previousTeleportTrigger = false;
            return;
        }

        var gunData = RenderGun();
        Vector3 startPos = GorillaTagger.Instance.headCollider.transform.position;
        Vector3 targetPos = gunData.NewPointer.transform.position;

        if (Vector3.Distance(startPos, targetPos) > maxTeleportDistance)
        {
            previousTeleportTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.8f;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(targetPos + Vector3.up * 2f, Vector3.down, out hit, 10f))
            targetPos = hit.point;

        bool trigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.8f;

        if (trigger && !previousTeleportTrigger)
        {
            GTPlayer.Instance.TeleportTo(
                targetPos + Vector3.up * 0.1f,
                GorillaTagger.Instance.headCollider.transform.rotation
            );

            GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            GorillaTagger.Instance.rigidbody.angularVelocity = Vector3.zero;
        }

        previousTeleportTrigger = trigger;


        } // may have made this a better mod than the normal one.
    }
}
