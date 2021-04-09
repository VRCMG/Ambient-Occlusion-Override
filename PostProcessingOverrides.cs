using MelonLoader;
//using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

[assembly: MelonInfo(typeof(PostProcessingOverrides.PostProcessingOverrides), "Post-Processing Overrides", "1.0.0", "Xerolide")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace PostProcessingOverrides
{
    public class PostProcessingOverrides : MelonMod
    {
        public override void OnApplicationStart() // Called on world load
        {
            // Register user preferences
            PostProcessingOverridesSettings.RegisterSettings();

            // Setup post-processing volume
        }

        public override void OnPreferencesSaved() // Called when preferences are updated
        {
            // Update user preferences
            PostProcessingOverridesSettings.OnModSettingsApplied();

            // Update post-processing settings

        }
    }
}