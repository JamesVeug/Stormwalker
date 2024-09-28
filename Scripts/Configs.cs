using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using UnityEngine;

namespace Stormwalker;

public static class Configs
{
	// Hotkeys
	public static List<KeyCode> Woodcutters_AssignOne => StringToKeyCode(m_woodcutters_AssignOne.Value, "Woodcutters_AssignOne");
	private static ConfigEntry<string> m_woodcutters_AssignOne = Bind("Hotkeys", "WoodCutters AssignOne", "X+LeftControl", "Assign one villager to be a woodcutter.");
	
	public static List<KeyCode> Woodcutters_UnassignAll => StringToKeyCode(m_woodcutters_UnassignAll.Value, "Woodcutters_UnassignAll");
	private static ConfigEntry<string> m_woodcutters_UnassignAll = Bind("Hotkeys", "WoodCutters Unassign All", "X+LeftShift", "Unassigns all woodcutters.");
	
	public static List<KeyCode> Woodcutters_BalanceHostility => StringToKeyCode(m_woodcutters_BalanceHostility.Value, "Woodcutters_BalanceHostility");
	private static ConfigEntry<string> m_woodcutters_BalanceHostility = Bind("Hotkeys", "WoodCutters Balance Hostility", "X+LeftAlt", "Unassigns enough woodcutters to lower hostility by 1.");

	// public static List<KeyCode> Ingredients_ShowRecipes => StringToKeyCode(m_ingredients_ShowRecipes.Value, "Ingredients_ShowRecipes");
	// private static ConfigEntry<string> m_ingredients_ShowRecipes = Bind("Hotkeys", "Show Ingredient Recipe", "Mouse0+LeftControl", "On items in the ingredient wheel will bring you the recipe panel.");

	public static List<KeyCode> Zoom_Toggle => StringToKeyCode(m_zoomToggle.Value, "Zoom_Toggle");
	private static ConfigEntry<string> m_zoomToggle = Bind("Hotkeys", "Toggle overview Zoom", "Backspace", "Zooms out to view the whole map. Press again to zoom back in.");

	// public static List<KeyCode> Villager_LeavePosition => StringToKeyCode(m_villagerLeavePosition.Value, "Villager_LeavePosition");
	// private static ConfigEntry<string> m_villagerLeavePosition = Bind("Hotkeys", "Villager Leave Position", "Mouse1+LeftControl", "Toggling this on a villager in a building will schedule them to leave their position as soon as the next production cycle finishes.");
	
	// Settings
	public static float ZoomLimit => m_zoomLimit.Value;
	private static ConfigEntry<float> m_zoomLimit = Bind("Zoom", "Zoom Heights", -60f, "The zoom heights the hotkey will move the camera to.");

	
	private static List<KeyCode> StringToKeyCode(string str, string debug)
	{
		string trimmed = str.Trim();
		string[] split = trimmed.Split('+');
		List<KeyCode> keyCodes = new();
		foreach (string key in split)
		{
			if (Enum.TryParse(key, out KeyCode keyCode))
			{
				keyCodes.Add(keyCode);
			}
			else
			{
				Debug.LogError($"Failed to parse hotkey {key} for {debug}. Make sure its typed correctly and can be found here: https://docs.unity3d.com/ScriptReference/KeyCode.html");
				return null;
			}
		}

		return keyCodes;
	}

	private static KeyboardShortcut StringToKeyboardShortcut(string str)
	{
		KeyboardShortcut stringToKeyboardShortcut = KeyboardShortcut.Deserialize(str);
		Plugin.Log($"StringToKeyboardShortcut: {str} -> {stringToKeyboardShortcut.MainKey} + {string.Join(",", stringToKeyboardShortcut.Modifiers)}");
		return stringToKeyboardShortcut;
	}
	
    private static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
	{
		return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, null, Array.Empty<object>()));
	}
}