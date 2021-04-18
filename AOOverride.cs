using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

// Assembly info
[assembly: MelonInfo(typeof(AOOverride.AOOverride), "AOOverride", "1.1.0", "Xerolide", "https://github.com/Xerolide/Ambient-Occlusion-Override")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace AOOverride
{
    public class AOOverride : MelonMod
    {
        private static PostProcessVolume volume;
        private static AmbientOcclusion occlusionConfig;

        public override void OnApplicationStart()
        {
            // Load user preferences from AOOverrideSettings
            AOOverrideSettings.RegisterSettings();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            // Wait until the third build index to initialize
            switch (buildIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                default:
                    // Run asynchronously so thread can wait until world load
                    MelonCoroutines.Start(WorldLoadedCoroutine());
                    break;
            }
        }

        public override void OnPreferencesSaved()
        {
            // Load user preferences
            AOOverrideSettings.LoadSettings();

            // Update volume according to user preferences
            UpdateVolume();
        }

        private IEnumerator WorldLoadedCoroutine()
        {
            yield return new WaitForSeconds(5);
            AOOverrideSettings.LoadSettings();
            PostProcessifyCamera();
            AddVolume();
            yield break;
        }

        private void PostProcessifyCamera()
        {
            // Add post-process layer to camera
            Camera camera = Camera.main;
            PostProcessLayer layer;
            if (camera.gameObject.GetComponent<PostProcessLayer>() == null)
                layer = camera.gameObject.AddComponent<PostProcessLayer>();
            else
                layer = camera.gameObject.GetComponent<PostProcessLayer>();
            layer.volumeLayer = -1;
        }

        /// <summary>
        /// Adds configurable post-processing volume to scene
        /// </summary>
        private void AddVolume()
        {
            // Setup occlusion instance
            occlusionConfig = ScriptableObject.CreateInstance<AmbientOcclusion>();

            // Create new volume
            GameObject volumeObj = new GameObject("Post-process Volume");
            volume = volumeObj.AddComponent<PostProcessVolume>();

            // Configure volume
            volume.profile.AddSettings(occlusionConfig);
            volume.isGlobal = true;
            volume.priority = float.MaxValue;

            // Update volume
            UpdateVolume();
        }

        /// <summary>
        /// Updates volume settings according to user preferences
        /// </summary>
        private void UpdateVolume()
        {
            PostProcessifyCamera(); // Backup in the event that camera was modified during world load

            // Configure effects for volume
            occlusionConfig.enabled.Override(AOOverrideSettings.EnableAO);
            occlusionConfig.mode.Override(AmbientOcclusionMode.MultiScaleVolumetricObscurance);
            occlusionConfig.intensity.Override(AOOverrideSettings.AOIntensity);
            occlusionConfig.ambientOnly.Override(false);
            occlusionConfig.thicknessModifier.Override(1);
            occlusionConfig.color.Override(Color.black);
        }
    }
}
