
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] public class ScoreEntry { public string name; public int score; }
[Serializable] class ScoreList { public List<ScoreEntry> items = new(); }

public static class LeaderboardStorage
{
    const string KEY = "leaderboard_json";
    const int MAX_ENTRIES = 50;

    static ScoreList Load()
    {
        var json = PlayerPrefs.GetString(KEY, "");
        return string.IsNullOrEmpty(json) ? new ScoreList() : JsonUtility.FromJson<ScoreList>(json);
    }

    static void Save(ScoreList list)
    {
        list.items.Sort((a, b) => b.score.CompareTo(a.score));             
        if (list.items.Count > MAX_ENTRIES) list.items.RemoveRange(MAX_ENTRIES, list.items.Count - MAX_ENTRIES);
        PlayerPrefs.SetString(KEY, JsonUtility.ToJson(list));
        PlayerPrefs.Save();
    }

    public static void Add(string name, int score)
    {
        var list = Load();
        list.items.Add(new ScoreEntry
        {
            name = string.IsNullOrWhiteSpace(name) ? "Jugador" : name.Trim(),
            score = Mathf.Max(0, score)
        });
        Save(list);
    }

    public static List<ScoreEntry> GetAll() => Load().items;

    public static void ClearAll()
    {
        PlayerPrefs.DeleteKey(KEY);
        PlayerPrefs.Save();
    }
}
