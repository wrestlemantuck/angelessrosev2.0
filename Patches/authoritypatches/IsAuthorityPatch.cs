// Made by wrestle
// Same thing as LocalAuthorityPatch, but its for GameEntityManager
// This will let you mess around more, like for example, messing with SuperInfection, etc
// simple patch
// Credits are WANTED
using HarmonyLib;

[HarmonyPatch(typeof(GameEntityManager), "IsAuthority")]
public class GameEntityManager_IsAuthority_Patch
{
    static bool Prefix(ref bool __result)
    {
        __result = true; // The client will think you are authoritzed to do actions. Because gorilla tag is dumb, and this method is not called on other clients checking your client, or even if its the server, all client checks will be bypassed with tihs.
        return false; // skip original
    }
}