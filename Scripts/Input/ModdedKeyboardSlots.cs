﻿using System.Collections.Generic;
using Eremite.View.Popups.GameMenu;
using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stormwalker;

[HarmonyPatch]
public class ModdedKeyboardSlots :MonoBehaviour
{
    public string ModName = "Hotkey";
    public List<KeyBindingSlot> slots = new List<KeyBindingSlot>();
    public int usedSlots = 0;

    public void Start()
    {
        Plugin.Log("Modded keyboard slots started.");
    }

    public void Initialize(string hotkeyName, Transform clone)
    {
        ModName = hotkeyName;
        slots.AddRange(clone.GetComponentsInChildren<KeyBindingSlot>(true));
    }

    public void ResetCounter()
    {
        usedSlots = 0;
    }

    public void SetupKeyboardSlots(InputActionMap actionMap, KeyBindingsPanel panel)
    {
        Plugin.Log("Setting up key bindings. " + actionMap.actions.Count + " actions and " + slots.Count + " slots.");
        foreach (InputAction action in actionMap.actions)
        {
            if (!InputConfigs.IsActionActive(action))
            {
                continue;
            }
            
            KeyBindingSlot slot = panel.GetOrCreate(slots, usedSlots++);
            slot.SetUp(action, panel.OnChangeRequested);
        }
    }
}