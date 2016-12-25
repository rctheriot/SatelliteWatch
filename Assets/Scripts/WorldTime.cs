using UnityEngine;
using System;

/// <summary>
/// Static class keeping track of the time (UTC and LOCAL).
/// TimeController class controls the time.
/// </summary>

public static class WorldTime
{
    public static DateTime worldTime { get; set; }
    public static DateTime localTime { get; set; }
    public static int timeMultiplier;

    static WorldTime()
    {
        worldTime = DateTime.UtcNow;
        localTime = DateTime.Now;
    }

    public static void setTimeMultiplier(int multiplier)
    {
        timeMultiplier = multiplier;
    }

    public static float getTimeMultiplier()
    {
        return timeMultiplier;
    }

    public static void updateTime()
    {
        worldTime = worldTime.AddSeconds(Time.deltaTime * timeMultiplier);
        localTime = localTime.AddSeconds(Time.deltaTime * timeMultiplier);
    }

    public static DateTime getUTCTime()
    {
        return worldTime;
    }

    public static DateTime getLocalTime()
    {
        return localTime;
    }

}
