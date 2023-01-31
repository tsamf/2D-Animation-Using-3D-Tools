using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour {


    private TotalData total;
    private List<EnemyData> enemies = new List<EnemyData>();
    private List<FriendlyData> friendlies = new List<FriendlyData>();
    private List<FriendlyData> lines = new List<FriendlyData>();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WriteToFile()
    {
        string path = string.Format(@"c:\temp\MyTest {0}.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                //Create header for total data
                sw.WriteLine(TotalData.ReturnHeader());
                
                //Write Total
                sw.WriteLine(total.ToString());

                //Create header for enemy data
                sw.WriteLine(EnemyData.ReturnHeader());

                //Write enemy lines
                foreach (EnemyData enemy in enemies)
                {
                    sw.WriteLine(enemy.ToString());
                }

                //Create header for friendly data
                sw.WriteLine(FriendlyData.ReturnHeader());

                //Write friendly lines
                foreach (FriendlyData friendly in friendlies)
                {
                    sw.WriteLine(friendly.ToString());
                }
            }
        }
    }

    public void AddEnemy(EnemyData enemy)
    {
        enemies.Add(enemy);
    }

    public void AddFriendly(FriendlyData friendly)
    {
        friendlies.Add(friendly);
    }

    public void SetTotal(TotalData total)
    {
        this.total = total;
    }

    //When the level is done write to a file
    private void OnDestroy()
    {
        WriteToFile();
    }
}

public class EnemyData
{
    string Type = "E";
    int ID { get; set; }
    int Points { get; set; }
    float TimeAlive { get; set; }
    string ObjectName { get; set; }

    public EnemyData(int ID, int Points, float TimeAlive, string ObjectName)
    {
        this.ID = ID;
        this.Points = Points;
        this.TimeAlive = TimeAlive;
        this.ObjectName = ObjectName;
    }

    public static string ReturnHeader()
    {
        return string.Format("{0},{1},{2},{3},{4}","Type", "ID", "Points", "TimeAlive","ObjectName");
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4}",Type, ID, Points, TimeAlive,ObjectName);
    }
}

public class FriendlyData
{
    string Type = "F";
    int ID { get; set; }
    float TimeAlive { get; set; }
    string ObjectName { get; set; }

    public FriendlyData(int ID, float TimeAlive, string ObjectName)
    {
        this.ID = ID;
        this.TimeAlive = TimeAlive;
        this.ObjectName = ObjectName;
    }

    public static string ReturnHeader()
    {
        return string.Format("{0},{1},{2},{3}","Type", "ID", "TimeAlive","ObjectName");
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3}",Type, ID, TimeAlive,ObjectName);
    }
}

public class TotalData
{
    int TotalPoints { get; set; }
    int HighestMultiplier { get; set; }
    int EnemiesKilled { get; set; }
    int FriendliesKilled { get; set; }
    int ClickCount { get; set; }
    int CurrentLevel { get; set; }
    string ActualLevel { get; set; }
    string ShaderSetting { get; set; }

    public TotalData(int TotalPoints, int HighestMultiplier, int EnemiesKilled, int FriendliesKilled,int ClickCount,int CurrentLevel,string ActualLevel,string ShaderSetting)
    {
        this.TotalPoints = TotalPoints;
        this.HighestMultiplier = HighestMultiplier;
        this.EnemiesKilled = EnemiesKilled;
        this.FriendliesKilled = FriendliesKilled;
        this.ClickCount = ClickCount;
        this.CurrentLevel = CurrentLevel;
        this.ActualLevel = ActualLevel;
        this.ShaderSetting = ShaderSetting;
    }

    public static string ReturnHeader()
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", "TotalPoints", "HighestMultiplier", "EnemiesKilled", "FriendliesKilled","ClickCount","CurrentLevel","ActualLevel","ShaderSetting");
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", TotalPoints,HighestMultiplier,EnemiesKilled,FriendliesKilled,ClickCount,CurrentLevel,ActualLevel,ShaderSetting);
    }
}
