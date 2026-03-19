using GorillaLocomotion;
using UnityEngine;
using System.Collections.Generic;

namespace StupidTemplate.Mods
{
public class Visual
{
private static Dictionary<VRRig, GameObject> boxESP = new Dictionary<VRRig, GameObject>();
private static float nextRigScan = 0f;

    public static void BoxESP()
    {
        if (Time.time > nextRigScan)
        {
            nextRigScan = Time.time + 0.1f; // 0.1f is the invertal of the scanning

            VRRig[] rigs = Object.FindObjectsOfType<VRRig>();

            foreach (VRRig rig in rigs)
            {
                if (rig == null || rig.isLocal)
                    continue;

                if (!boxESP.ContainsKey(rig))
                {
                    GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Object.Destroy(box.GetComponent<BoxCollider>());

                    box.transform.SetParent(rig.transform);
                    box.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                    box.transform.localRotation = Quaternion.identity;
                    box.transform.localScale = new Vector3(0.6f, 1.1f, 0.05f);

                    Renderer renderer = box.GetComponent<Renderer>();
                    renderer.material.shader = Shader.Find("GUI/Text Shader");

                    boxESP.Add(rig, box);
                }
            }

            List<VRRig> toRemove = new List<VRRig>();

            foreach (KeyValuePair<VRRig, GameObject> pair in boxESP)
            {
                if (pair.Key == null)
                {
                    Object.Destroy(pair.Value);
                    toRemove.Add(pair.Key);
                }
            }

            foreach (VRRig rig in toRemove)
                boxESP.Remove(rig);
        }

        foreach (KeyValuePair<VRRig, GameObject> pair in boxESP)
        {
            VRRig rig = pair.Key;
            GameObject box = pair.Value;

            if (rig == null || box == null)
                continue;

            Renderer renderer = box.GetComponent<Renderer>();

            if (rig.isLocal)
                renderer.material.color = Color.green;
            else
                renderer.material.color = Color.white;

            renderer.material.color = Color.white;
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
