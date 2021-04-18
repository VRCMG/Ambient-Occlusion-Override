using MelonLoader;
using System.Reflection;

namespace AOOverride
{
    public static class AOOverrideSettings
    {
        public static bool EnableAO;
        public static float AOIntensity;
        public static void RegisterSettings()
        {
            // Register mod settings
            MelonPrefs.RegisterCategory("AOOverride", "Ambient Occlusion");

            MelonPrefs.RegisterBool("AOOverride", nameof(EnableAO), true, "Enable AO");
            MelonPrefs.RegisterFloat("AOOverride", nameof(AOIntensity), 1f, "AO intensity");

            LoadSettings();
        }

        public static void LoadSettings()
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
