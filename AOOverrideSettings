using MelonLoader;
using System.Reflection;

namespace PostProcessingOverrides
{
    public static class PostProcessingOverridesSettings
    {
        public static float AmbientOcclusion = 0f;
        public static float Bloom = 0f;
        public static bool ACESTonemapping = false;
        public static float Temperature = 0f;
        public static float Exposure = 0f;
        public static float Saturation = 0f;
        public static float Contrast = 0f;
        public static void RegisterSettings()
        {
            // Register mod settings
            MelonPrefs.RegisterCategory("PostProcessingOverrides", "Post-processing overrides");

            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(AmbientOcclusion), 10f, "Ambient occlusion (0.5 recommended)");
            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(Bloom), 0f, "Bloom (0.1 recommended)");
            MelonPrefs.RegisterBool("PostProcessingOverrides", nameof(ACESTonemapping), false, "ACES tonemapping");
            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(Temperature), 0f, "Temperature");
            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(Exposure), 0f, "Exposure");
            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(Saturation), 0f, "Saturation");
            MelonPrefs.RegisterFloat("PostProcessingOverrides", nameof(Contrast), 0f, "Contrast");

            OnModSettingsApplied();
        }

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(PostProcessingOverridesSettings).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.FieldType == typeof(int))
                    fieldInfo.SetValue(null, MelonPrefs.GetInt("PostProcessingOverrides", fieldInfo.Name));
                if (fieldInfo.FieldType == typeof(bool))
                    fieldInfo.SetValue(null, MelonPrefs.GetBool("PostProcessingOverrides", fieldInfo.Name));
                if (fieldInfo.FieldType == typeof(float))
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat("PostProcessingOverrides", fieldInfo.Name));
            }
        }
    }
}
