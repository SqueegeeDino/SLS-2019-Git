using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SXLFixHDRP : MonoBehaviour
{
    [MenuItem("SkaterXL/Fix HDRP")]
    static void FixHDRP() {
        Shader hdrpLit = Shader.Find("HDRP/Lit");
        Shader hdrpDecal = Shader.Find("HDRP/Decal");
        Material debugMat = new Material(hdrpLit);
        debugMat.color = Color.red;

        Debug.Log("Running Shader Fixes...");
        foreach (Renderer renderer in UnityEngine.Object.FindObjectsOfType<Renderer>())
        {
            Material[] sharedMaterials = renderer.sharedMaterials;
            for (int index = 0; index < sharedMaterials.Length; ++index)
            {
                try {
                    Shader s = sharedMaterials[index].shader;
                    bool hdrp = s.name.Contains("HDRP");
                    bool legacyHDRP = s.name.Contains("HDRenderPipeline");
                    sharedMaterials[index].shader = hdrp ? Shader.Find(s.name) : legacyHDRP ? s.name.Contains("Decal") ? hdrpDecal : hdrpLit : Shader.Find(s.name);
                    continue;
                    }
                catch {
                    Debug.Log($"{renderer.gameObject.name} Has a material ({index}) that needs to be addressed.");
                    renderer.material = debugMat;
                }
                continue;
            }
        }
    }
}
