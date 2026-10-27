using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public sealed class JsonReader
{
    private const string Filename = "config.json";

    public void Write(Config config)
    {
        string json = JsonConvert.SerializeObject(config, Formatting.Indented);
        string path = Path.Combine(Application.persistentDataPath, Filename);
        File.WriteAllText(path, json);
    }

    public Config Read()
    {
        string path = Path.Combine(Application.persistentDataPath, Filename);
        Config data = null;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = JsonConvert.DeserializeObject<Config>(json);
        }

        return data;
    }
}