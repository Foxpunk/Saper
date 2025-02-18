using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TableRecord
{
    private const string FileName = "Table.txt";
    private Dictionary<string, PlayerRecord> records;

    public TableRecord()
    {
        records = new Dictionary<string, PlayerRecord>();
        LoadRecords();
    }

    public void LoadRecords()
    {
        if (!File.Exists(FileName))
        {
            File.Create(FileName).Close();
            return;
        }

        foreach (var line in File.ReadLines(FileName))
        {
            var parts = line.Split(';');
            if (parts.Length == 4)
            {
                string name = parts[0];
                int easy = int.TryParse(parts[1], out var e) ? e : -1;
                int medium = int.TryParse(parts[2], out var m) ? m : -1;
                int hard = int.TryParse(parts[3], out var h) ? h : -1;

                records[name] = new PlayerRecord(name, easy, medium, hard);
            }
        }
    }

    public void SaveRecords()
    {
        using (StreamWriter writer = new StreamWriter(FileName, false))
        {
            foreach (var record in records.Values)
            {
                writer.WriteLine($"{record.Name};{record.EasyTime};{record.MediumTime};{record.HardTime}");
            }
        }
    }

    public void UpdateRecord(string name, string difficulty, int time)
    {
        if (!records.ContainsKey(name))
        {
            records[name] = new PlayerRecord(name);
        }

        var record = records[name];
        switch (difficulty.ToLower())
        {
            case "easy":
                if (time < record.EasyTime || record.EasyTime == -1)
                    record.EasyTime = time;
                break;
            case "medium":
                if (time < record.MediumTime || record.MediumTime == -1)
                    record.MediumTime = time;
                break;
            case "hard":
                if (time < record.HardTime || record.HardTime == -1)
                    record.HardTime = time;
                break;
        }

        SaveRecords();
    }

    public List<PlayerRecord> GetRecords()
    {
        return records.Values.ToList();
    }
}

public class PlayerRecord
{
    public string Name { get; set; }
    public int EasyTime { get; set; }
    public int MediumTime { get; set; }
    public int HardTime { get; set; }

    public PlayerRecord(string name, int easyTime = -1, int mediumTime = -1, int hardTime = -1)
    {
        Name = name;
        EasyTime = easyTime;
        MediumTime = mediumTime;
        HardTime = hardTime;
    }

    public string GetFormattedEasyTime()
    {
        return EasyTime == -1 ? "-" : EasyTime.ToString();
    }

    public string GetFormattedMediumTime()
    {
        return MediumTime == -1 ? "-" : MediumTime.ToString();
    }

    public string GetFormattedHardTime()
    {
        return HardTime == -1 ? "-" : HardTime.ToString();
    }

    public override string ToString()
    {
        return $"{Name}: Easy={GetFormattedEasyTime()}, Medium={GetFormattedMediumTime()}, Hard={GetFormattedHardTime()}";
    }
}
