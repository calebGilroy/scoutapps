using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Avalonia.Platform;
using ScoutApp.ViewModels;

public static class JsonToCSConverter
    {
        private const string DefaultScheduleAssetPath = "avares://ScoutApp/Assets/2025wasno.json";

        private static Dictionary<int, int[]> TeamsByMatch = [];

        private sealed class ScheduleJson
        {
            [JsonPropertyName("records")]
            public List<ScheduleRecord>? Records { get; set; }
        }

        private sealed class ScheduleRecord
        {
            [JsonPropertyName("comp_level")]
            public string? CompLevel { get; set; }

            [JsonPropertyName("match_number")]
            public int MatchNumber { get; set; }

            [JsonPropertyName("red1")]
            public int Red1 { get; set; }

            [JsonPropertyName("blue1")]
            public int Blue1 { get; set; }

            [JsonPropertyName("red2")]
            public int Red2 { get; set; }

            [JsonPropertyName("blue2")]
            public int Blue2 { get; set; }

            [JsonPropertyName("red3")]
            public int Red3 { get; set; }

            [JsonPropertyName("blue3")]
            public int Blue3 { get; set; }
        }

        public static void LoadFromScheduleJson(string json)
        {
            ScheduleJson? parsed = JsonSerializer.Deserialize<ScheduleJson>(json);
            if (parsed?.Records == null)
                return;

            var next = new Dictionary<int, int[]>(capacity: parsed.Records.Count);
            foreach (ScheduleRecord record in parsed.Records)
            {
                if (record.CompLevel != "qm")
                    continue;
                next[record.MatchNumber] = [record.Red1, record.Red2, record.Red3, record.Blue1, record.Blue2, record.Blue3];
            }

            TeamsByMatch = next;
        }

        public static bool TryLoadDefaultSchedule()
        {
            try
            {
                var uri = new Uri(DefaultScheduleAssetPath);
                if (!AssetLoader.Exists(uri))
                    return false;

                using var stream = AssetLoader.Open(uri);
                using var reader = new StreamReader(stream);
                LoadFromScheduleJson(reader.ReadToEnd());
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void EnsureLoaded()
        {
            if (TeamsByMatch.Count <= 1)
                TryLoadDefaultSchedule();
        }

        public static bool TryGetTeamNumber(int matchNumber, AlliancePosition alliance, out int teamNumber)
        {
            EnsureLoaded();
            teamNumber = default;

            if (!TeamsByMatch.TryGetValue(matchNumber, out int[]? teams) || teams.Length < 6)
                return false;

            int index = alliance switch
            {
                AlliancePosition.Red1 => 0,
                AlliancePosition.Red2 => 1,
                AlliancePosition.Red3 => 2,
                AlliancePosition.Blue1 => 3,
                AlliancePosition.Blue2 => 4,
                AlliancePosition.Blue3 => 5,
                _ => -1
            };

            if (index < 0)
                return false;

            teamNumber = teams[index];
            return true;
        }
    }
