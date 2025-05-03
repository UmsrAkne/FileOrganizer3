using System;
using System.IO;
using System.Text.Json;

namespace FileOrganizer3.Models
{
    public class AppSettings
    {
        // ReSharper disable once ArrangeModifiersOrder
        public static readonly double DefaultFontSize = 13.0;

        // ReSharper disable once ArrangeModifiersOrder
        private static readonly string SettingsFilePath = "appSettings.json";

        public double FontSize { get; set; } = DefaultFontSize;

        public string PrefixText { get; set; }

        public string SuffixText { get; set; }

        public int FormatDigitCount { get; set; }

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<AppSettings>(json);
                }
                else
                {
                    Console.WriteLine("**** APP WARNING **** Config file not found. Using default settings.");
                    return new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** An error occurred while loading settings: {ex.Message}");
                return new AppSettings();
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true, });

                File.WriteAllText(SettingsFilePath, json);
                Console.WriteLine("**** INFO **** Settings have been saved.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"**** ERROR **** An error occurred while saving settings: {ex.Message}");
            }
        }
    }
}