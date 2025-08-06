# nullable enable

using System;
using System.IO;
using UnityEngine;

namespace RealityLog
{
    public static class AppSettings
    {
        private const string SETTINGS_FILE_NAME = "settings.yaml";
        private const int DEFAULT_FPS = 10;

        public static int Fps { get; private set; } = DEFAULT_FPS;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load()
        {
            var path = Path.Combine(Application.persistentDataPath, SETTINGS_FILE_NAME);

            try
            {
                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                    File.WriteAllText(path, $"fps: {DEFAULT_FPS}\n");
                    Fps = DEFAULT_FPS;
                    return;
                }

                foreach (var line in File.ReadAllLines(path))
                {
                    var trimmed = line.Trim();
                    if (trimmed.StartsWith("fps", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = trimmed.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out var parsed) && parsed > 0)
                        {
                            Fps = parsed;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{Constants.LOG_TAG}] Failed to load settings: {ex.Message}");
                Fps = DEFAULT_FPS;
            }
        }
    }
}
