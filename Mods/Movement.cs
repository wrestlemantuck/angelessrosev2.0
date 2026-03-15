using GorillaLocomotion;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.XR;
using StupidTemplate.Notifications;
using static StupidTemplate.Settings;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Movement
    {

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

        public static void WASDFly()
        {
            float speed = 10f;
            Transform head = GorillaTagger.Instance.headCollider.transform;

            Vector3 move = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                move += head.forward;
            }

            if (Input.GetKey(KeyCode.S))
            {
                move -= head.forward;
            }

            if (Input.GetKey(KeyCode.A))
            {
                move -= head.right;
            }

            if (Input.GetKey(KeyCode.D))
            {
                move += head.right;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                move += Vector3.up;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                move -= Vector3.up;
            }

            if (move != Vector3.zero)
            {
                GTPlayer.Instance.transform.position += move.normalized * speed * Time.deltaTime;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        // vars for TeleportGun below
        public static bool previousTeleportTrigger;
        public static GameObject currentPointer;
        public static GameObject currentLine;
        public static float maxTeleportDistance = 60f;

        public static void TeleportGun()
        {
        if (!ControllerInputPoller.instance.rightGrab)
        {
        if (currentPointer != null)
        {
        Object.Destroy(currentPointer);
        currentPointer = null;
        }

            if (currentLine != null)
            {
                Object.Destroy(currentLine);
                currentLine = null;
            }

            previousTeleportTrigger = false;
            return;
        }

        var gunData = RenderGun();
        currentPointer = gunData.NewPointer;

        if (currentLine == null)
        {
            currentLine = new GameObject("TeleportGunLine");
            currentLine.AddComponent<LineRenderer>();
        }

        LineRenderer lr = currentLine.GetComponent<LineRenderer>();
        lr.positionCount = 2;

        Vector3 startPos = GorillaTagger.Instance.rightHandTransform.position;
        Vector3 targetPos = currentPointer.transform.position;

        lr.SetPosition(0, startPos);
        lr.SetPosition(1, targetPos);

        Vector3 headPos = GorillaTagger.Instance.headCollider.transform.position;

        if (Vector3.Distance(headPos, targetPos) > maxTeleportDistance)
        {
            previousTeleportTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.8f;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(targetPos + Vector3.up * 2f, Vector3.down, out hit, 10f))
        {
            targetPos = hit.point;
        }

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
