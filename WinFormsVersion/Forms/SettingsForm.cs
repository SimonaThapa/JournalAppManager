using System;
using System.IO;

namespace SimsAppJournal.Services
{
    public static class SettingsService
    {
        private static readonly string SettingsFile = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "settings.txt"
        );

        // Save PIN securely 
        public static void UpdatePin(string pin)
        {
            // Save PIN to file
            try
            {
                File.WriteAllText(SettingsFile, pin);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save PIN: " + ex.Message);
            }
        }

        // Get saved PIN
        public static string GetPin()
        {
            try
            {
                if (!File.Exists(SettingsFile))
                    return string.Empty;

                return File.ReadAllText(SettingsFile).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}



