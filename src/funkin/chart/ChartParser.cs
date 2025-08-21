using System;
using System.Collections.Generic;
using System.IO;
using framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace funkin;

public static class ChartParser
{
    // =============================
    // Main Methods
    // =============================

    public static ChartFile ParseFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Chart file not found: {filePath}");


        string json = File.ReadAllText(filePath);
        return ParseFromJson(json);
    }

    public static ChartFile ParseFromJson(string json)
    {
        return JsonConvert.DeserializeObject<ChartFile>(json);
    }

    public static void SaveToFile(ChartFile chart, string filePath, bool prettyPrint = true)
    {
        var formatting = prettyPrint ? Formatting.Indented : Formatting.None;
        string json = JsonConvert.SerializeObject(chart, formatting);
        File.WriteAllText(filePath, json);
    }

    // =============================
    // Models (FULL FORMAT)
    // =============================

    public class ChartFile
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("songName")]
        public string SongName { get; set; }

        [JsonProperty("bpm")]
        public float Bpm { get; set; }

        [JsonProperty("scrollSpeed")]
        public ScrollSpeedData ScrollSpeed { get; set; }

        [JsonProperty("notes")]
        public DifficultyNotes Notes { get; set; }

        [JsonProperty("events")]
        public List<EventData> Events { get; set; }

        [JsonProperty("generatedBy")]
        public string GeneratedBy { get; set; }
    }

    public class ScrollSpeedData
    {
        [JsonProperty("easy")]
        public float Easy { get; set; }

        [JsonProperty("normal")]
        public float Normal { get; set; }

        [JsonProperty("hard")]
        public float Hard { get; set; }
    }

    public class DifficultyNotes
    {
        [JsonProperty("easy")]
        public List<NoteData> Easy { get; set; }

        [JsonProperty("normal")]
        public List<NoteData> Normal { get; set; }

        [JsonProperty("hard")]
        public List<NoteData> Hard { get; set; }
    }

    public class NoteData
    {
        [JsonProperty("t")]
        public float Time { get; set; }

        [JsonProperty("d")]
        public int Data { get; set; }

        [JsonProperty("l")]
        public float Length { get; set; }
    }

    [JsonConverter(typeof(EventDataConverter))]
    public class EventData
    {
        [JsonProperty("t")]
        public int Time { get; set; }

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("v")]
        public object Value { get; set; }
    }

    // =============================
    // Event Value Models
    // =============================

    public class PlayAnimationValue
    {
        [JsonProperty("target")]
        public string Target { get; set; }

        [JsonProperty("anim")]
        public string Animation { get; set; }

        [JsonProperty("force")]
        public bool Force { get; set; }
    }

    public class CameraEventValue
    {
        [JsonProperty("x")]
        public float X { get; set; }

        [JsonProperty("y")]
        public float Y { get; set; }
    }

    // =============================
    // Custom Converter for EventData
    // =============================

    public class EventDataConverter : JsonConverter<EventData>
    {
        public override EventData ReadJson(JsonReader reader, Type objectType, EventData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var ev = new EventData
            {
                Time = obj["t"].Value<int>(),
                EventType = obj["e"].Value<string>()
            };

            var valueToken = obj["v"];
            if (valueToken == null || valueToken.Type == JTokenType.Null)
            {
                ev.Value = null;
                return ev;
            }

            switch (ev.EventType)
            {
                case "PlayAnimation":
                    ev.Value = valueToken.ToObject<PlayAnimationValue>(serializer);
                    break;

                case "CameraMove":
                    ev.Value = valueToken.ToObject<CameraEventValue>(serializer);
                    break;

                default:
                    ev.Value = valueToken.ToObject<object>();
                    break;
            }

            return ev;
        }

        public override void WriteJson(JsonWriter writer, EventData value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("t");
            writer.WriteValue(value.Time);
            writer.WritePropertyName("e");
            writer.WriteValue(value.EventType);
            writer.WritePropertyName("v");
            serializer.Serialize(writer, value.Value);
            writer.WriteEndObject();
        }
    }

    public static class ChartHelpers
    {
        public static string GetSongName(string chartFilePath, ChartFile chart)
        {
            if (!string.IsNullOrEmpty(chart.SongName))
                return chart.SongName;

            // Get the parent folder of the chart file (song folder)
            string songFolder = Path.GetFileName(Path.GetDirectoryName(chartFilePath));

            // Check if the chart is inside a mods folder
            string modsFolder = Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(chartFilePath)));
            if (modsFolder.Equals("mods", StringComparison.OrdinalIgnoreCase))
            {
                string modName = Assets.CurrentMod; // mod folder
                return $"{modName}/{songFolder}";
            }

            return songFolder;
        }
    }
}
