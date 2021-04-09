using MelonLoader;
using System.Reflection;

namespace AOOverride
{
    public static class AOOverrideSettings
    {
        public static float AmbientOcclusion = 0.5f;
        public static bool EnableAO = true;
        public static void RegisterSettings()
        {
            // Register mod settings
            MelonPrefs.RegisterCategory("AOOverride", "Ambient occlusion override");

            MelonPrefs.RegisterFloat("AOOverride", nameof(AmbientOcclusion), 0.5f, "Ambient occlusion (0.5 recommended)");
            MelonPrefs.RegisterBool("AOOverride", nameof(EnableAO), true, "Enable AO");

            OnModSettingsApplied();
        }

        public static void OnModSettingsApplied()
        {
            foreach (var fieldInfo in typeof(AOOverrideSettings).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                if (fieldInfo.FieldType == typeof(int))
                    fieldInfo.SetValue(null, MelonPrefs.GetInt("AOOverride", fieldInfo.Name));
                if (fieldInfo.FieldType == typeof(bool))
                    fieldInfo.SetValue(null, MelonPrefs.GetBool("AOOverride", fieldInfo.Name));
                if (fieldInfo.FieldType == typeof(float))
                    fieldInfo.SetValue(null, MelonPrefs.GetFloat("AOOverride", fieldInfo.Name));
            }
        }
    }
}
