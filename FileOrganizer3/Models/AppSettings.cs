using System;
using System.IO;
using System.Text.Json;

namespace FileOrganizer3.Models
{
    public class AppSettings
    {
        // ReSharper disable once ArrangeModifiersOrder
        private static readonly string SettingsFilePath = "appSettings.json";

        public double FontSize { get; set; }

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
                    Console.WriteLine("設定ファイルが見つかりません。デフォルト設定を使用します。");
                    return new AppSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"設定の読み込み中にエラーが発生しました: {ex.Message}");
                return new AppSettings();
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true, });

                File.WriteAllText(SettingsFilePath, json);
                Console.WriteLine("設定が保存されました。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"設定の保存中にエラーが発生しました: {ex.Message}");
            }
        }
    }
}