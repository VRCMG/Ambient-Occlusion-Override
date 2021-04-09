using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[assembly: MelonInfo(typeof(AOOverride.AOOverride), "AOOverride", "1.0.0", "Xerolide", "https://github.com/Xerolide/Ambient-Occlusion-Override")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace AOOverride
{
    public class AOOverride : MelonMod
    {

        public override void OnApplicationStart()
        {
            AOOverrideSettings.RegisterSettings();
        }

        public override void OnPreferencesSaved() // Called when preferences are updated
        {
            AOOverrideSettings.OnModSettingsApplied();

            // Disable old volumes
            PostProcessVolume[] oldVolumes = GameObject.FindObjectsOfType<PostProcessVolume>();
            foreach (PostProcessVolume oldVolume in oldVolumes)
            {
                if (oldVolume.gameObject.tag == "GenVol")
                    oldVolume.enabled = false;
            }

            // Setup effects for new volume
            AmbientOcclusion occlusion = ScriptableObject.CreateInstance<AmbientOcclusion>();
            occlusion.enabled.Override(AOOverrideSettings.EnableAO);
            occlusion.mode.Override(AmbientOcclusionMode.MultiScaleVolumetricObscurance);
            occlusion.intensity.Override(AOOverrideSettings.AmbientOcclusion);
            occlusion.ambientOnly.Override(false);
            occlusion.thicknessModifier.Override(1);
            occlusion.color.Override(Color.black);

            // Create new volume
            GameObject volumeObject = new GameObject("Post-process Volume");
            volumeObject.tag = "GenVol";
            PostProcessVolume volume = volumeObject.AddComponent<PostProcessVolume>();

            // Configure volums
            volume.profile.AddSettings(occlusion);
            volume.isGlobal = true;
            volume.priority = float.MaxValue;

            // Add post-process layer to camera
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            PostProcessLayer layer;
            foreach (var cam in cameras)
            {
                if (cam.gameObject.GetComponent<PostProcessLayer>() == null)
                    layer = cam.gameObject.AddComponent<PostProcessLayer>();
                else
                    layer = cam.gameObject.GetComponent<PostProcessLayer>();
                layer.volumeLayer = -1;
            }
        }

    }
}
