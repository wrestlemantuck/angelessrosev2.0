using GorillaLocomotion;
using UnityEngine;
using System.Collections.Generic;

namespace StupidTemplate.Mods
{
public class Visual
{
private static Dictionary<VRRig, GameObject> boxESP = new Dictionary<VRRig, GameObject>();

    public static void BoxESP()
    {
        VRRig[] rigs = Object.FindObjectsOfType<VRRig>();

        List<VRRig> toRemove = new List<VRRig>();

        foreach (KeyValuePair<VRRig, GameObject> pair in boxESP)
        {
            bool exists = false;

            foreach (VRRig rig in rigs)
            {
                if (rig == pair.Key)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                Object.Destroy(pair.Value);
                toRemove.Add(pair.Key);
            }
        }

        foreach (VRRig rig in toRemove)
        {
            boxESP.Remove(rig);
        }

        foreach (VRRig rig in rigs)
        {
            if (rig == null || rig.isLocal)
                continue;

            GameObject box;

            if (!boxESP.TryGetValue(rig, out box))
            {
                box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Object.Destroy(box.GetComponent<BoxCollider>());

                box.transform.localScale = new Vector3(0.6f, 1.1f, 0.05f);

                Renderer renderer = box.GetComponent<Renderer>();
                renderer.material.shader = Shader.Find("GUI/Text Shader");
                renderer.material.color = Color.red;

                boxESP.Add(rig, box);
            }

            box.transform.position = rig.transform.position + Vector3.up * 0.5f;

            box.transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
        }
    }

    public static void DisableBoxESP()
    {
        foreach (GameObject box in boxESP.Values)
        {
            if (box != null)
                Object.Destroy(box);
        }

        boxESP.Clear();
    }
}

}
