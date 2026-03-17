// Made by wrestle to help you do more stuff with crittersmanager
// Simple patch
// Please give credits if used.
using HarmonyLib;


[HarmonyPatch(typeof(CrittersManager), "LocalAuthority")] // CrittersManager.LocalAuthority() patcher.
public class LocalAuthorityPatch
{
    static bool Prefix(ref bool __result)
    {
        __result = true; // Make it look like you are the local authority (the method is very weird, but you can just patch it to fix it.)
        return false;   // skip original
    }
} 
