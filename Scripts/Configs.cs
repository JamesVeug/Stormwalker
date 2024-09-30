using System;
using BepInEx.Configuration;

namespace Stormwalker;

public static class Configs
{
	// Call this so the static fields are also called
	public static void Initialize()
	{
		// Does actually do stuff!
	}
	
	// Settings
	public static int ZoomLimit => m_zoomLimit.Value;
	private static ConfigEntry<int> m_zoomLimit = Bind("Zoom", "Zoom Height", -60, "The zoom heights the hotkey will move the camera to.", -100, -8);

    private static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
	{
		return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, null, Array.Empty<object>()));
	}
    
	private static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description, T min, T max) where T : IComparable
	{
		return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, new AcceptableValueRange<T>(min,max), Array.Empty<object>()));
	}
}