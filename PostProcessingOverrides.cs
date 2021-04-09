using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Threading;

[assembly: MelonInfo(typeof(PostProcessingOverrides.PostProcessingOverrides), "Post-Processing Overrides", "1.0.0", "Xerolide")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace PostProcessingOverrides // TODO: Preserve world post-processing settings; optimize mod
{
    public class PostProcessingOverrides : MelonMod
    {
        private AmbientOcclusion occlusion;
        private Bloom bloom;
        private ColorGrading colorGrading;
        private PostProcessVolume[] oldVolumes;
        private PostProcessVolume volume;
        private PostProcessLayer layer;

        public override void OnApplicationStart()
        {
            PostProcessingOverridesSettings.RegisterSettings();
        }

        public override void OnPreferencesSaved() // Called when preferences are updated
        {
            PostProcessingOverridesSettings.OnModSettingsApplied();

            // Disable old volumes
            oldVolumes = GameObject.FindObjectsOfType<PostProcessVolume>();
            foreach (PostProcessVolume oldVolume in oldVolumes)
            {
                    oldVolume.enabled = false;
            }

            // Setup effects for new volume
            occlusion = ScriptableObject.CreateInstance<AmbientOcclusion>();
            occlusion.enabled.Override(true);
            occlusion.mode.Override(AmbientOcclusionMode.MultiScaleVolumetricObscurance);
            occlusion.intensity.Override(PostProcessingOverridesSettings.AmbientOcclusion);

            bloom = ScriptableObject.CreateInstance<Bloom>();
            bloom.enabled.Override(true);
            bloom.intensity.Override(PostProcessingOverridesSettings.Bloom <= 1 ? PostProcessingOverridesSettings.Bloom : 1);
            bloom.threshold.Override(0f);

            colorGrading = ScriptableObject.CreateInstance<ColorGrading>();
            colorGrading.enabled.Override(true);
            colorGrading.tonemapper.Override(PostProcessingOverridesSettings.ACESTonemapping ? Tonemapper.ACES : Tonemapper.None);
            colorGrading.temperature.Override(PostProcessingOverridesSettings.Temperature);
            colorGrading.postExposure.Override(PostProcessingOverridesSettings.Exposure < 3 ? PostProcessingOverridesSettings.Exposure :
                (PostProcessingOverridesSettings.Exposure > -3 ? PostProcessingOverridesSettings.Exposure : -3));
            colorGrading.saturation.Override(PostProcessingOverridesSettings.Saturation);
            colorGrading.contrast.Override(PostProcessingOverridesSettings.Contrast > -90 ? PostProcessingOverridesSettings.Contrast : -90);

            // Create new volume
            GameObject volumeObject = new GameObject("Post-process Volume");
            volume = volumeObject.AddComponent<PostProcessVolume>();

            volume.profile.AddSettings(occlusion);
            volume.profile.AddSettings(bloom);
            volume.profile.AddSettings(colorGrading);
            volume.isGlobal = true;
            volumeObject.layer = 8;

            // Add post-process layer to camera
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            foreach (var cam in cameras)
            {
                if (cam.gameObject.GetComponent<PostProcessLayer>() == null)
                {
                    layer = cam.gameObject.AddComponent<PostProcessLayer>();
                }
                else
                    layer = cam.gameObject.GetComponent<PostProcessLayer>();
                layer.volumeLayer = 256;
            }
        }

    }
}
