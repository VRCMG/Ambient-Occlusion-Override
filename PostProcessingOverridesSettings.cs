using System.Reflection;
using MelonLoader;

namespace PostProcessingOverrides
{
    public static class PostProcessingOverridesSettings
    {
        private const string SettingsCategory = "PostProcessingOverrides";

        public static void RegisterSettings()
        {
            MelonPrefs.RegisterCategory(SettingsCategory, "Post-processing overrides");

            MelonPrefs.RegisterBool(SettingsCategory, nameof(OverridePostProcessing), false, "Override post-processing");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(AmbientOcclusionIntensity), 0f, "Ambient occlusion intensity (0.5 recommended)");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(BloomIntensity), 0f, "Bloom intensity (0.1 recommended)");
            MelonPrefs.RegisterBool(SettingsCategory, nameof(TonemappingEnabled), false, "ACES tonemapping");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(Temperature), 0f, "Temperature");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(PostExposure), 0f, "Post-exposure");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(Saturation), 0f, "Saturation");
            MelonPrefs.RegisterFloat(SettingsCategory, nameof(Contrast), 0f, "Contrast");

            OnModSettingsApplied();
        }

        public static bool OverridePostProcessing;
        public static float AmbientOcclusionIntensity;
        public static float BloomIntensity;
        public static bool TonemappingEnabled;
        public static float Temperature;
        public static float PostExposure;
        public static float Saturation;
        public static float Contrast;

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(PostProcessingOverridesSettings).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.FieldType == typeof(int))
                    fieldInfo.SetValue(null, MelonPrefs.GetInt(SettingsCategory, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(bool))
                    fieldInfo.SetValue(null, MelonPrefs.GetBool(SettingsCategory, fieldInfo.Name));

                if (fieldInfo.FieldType == typeof(float))
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat(SettingsCategory, fieldInfo.Name));
            }
        }
    }
}