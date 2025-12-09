using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Leaderboard")]

public class LeaderboardData
{
    [XmlElement("Entry")]
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

[Serializable]
public class LeaderboardEntry
{
    public string Name;
    public int Score;
    public float Time;

    public LeaderboardEntry() { }

    public LeaderboardEntry(string name, int score, float time)
    {
        Name = name;
        Score = score;
        Time = time;
    }
}