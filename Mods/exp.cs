using GorillaLocomotion;
using StupidTemplate.Classes;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using System.Linq;
using System.Collections.Generic;
using GorillaNetworking;
using System.Reflection;
using System.Collections;
using GorillaTagScripts; // important
using HarmonyLib;
using StupidTemplate.Notifications; // notifs
using GorillaTag.Reactions;
using System;
using UnityEngine.SceneManagement;

namespace StupidTemplate.Mods
{
    public static class Experimental
    {

public static void DespawnAllItems()
        {
        }
        public static void DespawnAllCritters()
        {
            if (CrittersManager.instance == null || VRRig.LocalRig == null)
                return;
            CrittersManager.instance.QueueDespawnAllCritters(); // This requires Patches/authoritypatches/LocalAuthorityPatch.cs because gorilla tag has protection
        }
        public static void SpawnCritterAtHand(int critterType) // Requires master
        {
            if (CrittersManager.instance == null || VRRig.LocalRig == null)
                return;

            Transform handTransform = VRRig.LocalRig.rightHandTransform;
            if (handTransform == null) return;

            Vector3 spawnPos = handTransform.position;
            Quaternion spawnRot = handTransform.rotation;

            CrittersManager.instance.SpawnCritter(critterType, spawnPos, spawnRot);
        }

        public static void SpawnRandomCritterAtHand()
        {
            int randomCritter = UnityEngine.Random.Range(1, 17); // 1-16 is the amount of critter prefabs im pretty sure
            SpawnCritterAtHand(randomCritter);
        }

        public static void ApplyKnockback(float strength) // this is really useless except for like fly mods possibly
        {
            if (GTPlayer.Instance == null) return;

            Vector3 direction = Vector3.up; // Direction can be anything.
            GTPlayer.Instance.SetMaximumSlipThisFrame();
            GTPlayer.Instance.ApplyKnockback(direction, strength, false);
        }

        public static void ApplyKnockback(float strength, Vector3 direction) // this is the same method as the normal ApplyKnockback but with a direction param, it used to be hardcoded into it so this allows free direction changing on runtime.
        {
            if (GTPlayer.Instance == null) return;

            GTPlayer.Instance.SetMaximumSlipThisFrame();
            GTPlayer.Instance.ApplyKnockback(direction.normalized, strength, false);
        }

        public static void AddProgressionPoints(int amount)
        {
            if (GhostReactorProgression.instance == null) return;

            GRPlayer player = UnityEngine.Object.FindObjectOfType<GRPlayer>(); // because gorilla tag changes GTPlayer to GRPlayer when ghost reactor is initialized im pretty sure or im dumb yk
            if (player == null) return;

            GhostReactorProgression.instance.SetProgression(amount, player); // this method sends a API request to https://prog-prod.gtag-cf.com/ (is in Playfab something i forgot string "ProgressionApiBaseUrl")
        }


        public static void SetTotalPlayTime(float seconds) // not exactly sure if this works, there is no visualization of the total play time
        {
            GhostReactorShiftManager manager = UnityEngine.Object.FindObjectOfType<GhostReactorShiftManager>();
            if (manager == null) return;

            FieldInfo playTimeField = typeof(GhostReactorShiftManager).GetField("totalPlayTime", BindingFlags.NonPublic | BindingFlags.Instance);
            playTimeField?.SetValue(manager, seconds);
        }
    }
}