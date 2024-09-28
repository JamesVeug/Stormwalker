using System;
using System.Collections.Generic;
using Eremite;
using Eremite.Services;
using Eremite.View;
using Eremite.View.Popups.GameMenu;
using Eremite.View.Utils;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stormwalker;

[HarmonyPatch]
public static class InputConfigs
{
    public static string s_HOTKEY_NAME = "Stormwalker";  
        
    private class PendingHotkey
    {
        public string keyName;
        public List<KeyCode> codes;
        public Action onRelease;
    }
    
    private static InputActionMap actionMap;

    private static List<KeyBindingSlot> slots;
    private static int usedSlots;
    private static InputActionAsset InputAsset;
    private static List<PendingHotkey> pendingHotkeys = new List<PendingHotkey>();


    public static void RegisterKey(string keyName, List<KeyCode> codes, Action onPress=null, Action onRelease = null)
    {
        Plugin.Log($"Registering key {keyName} with code {string.Join(",", codes)}");
        if (actionMap == null)
        {
            Plugin.Log($"Action map is null, adding to pending hotkeys.");
            var item = new PendingHotkey();
            item.keyName = keyName;
            item.codes = codes;
            item.onRelease = onRelease;
            pendingHotkeys.Add(item);
            
            return;
        }

        string localizedKeyName = "MenuUI_KeyBindings_Action_" + keyName;
        
        
        
        InputAction action = null;
        if (codes.Count > 1)
        {
            action = actionMap.AddAction(keyName);
                
            var comp = action.AddCompositeBinding(keyName + "_Composite");
            comp.With("Button", $"<Keyboard>/{codes[0]}");
            for (int i = 1; i < codes.Count; i++)
            {
                comp.With("Modifier" + i, $"<Keyboard>/{codes[i]}");
            }
        }
        else
        {
            action = actionMap.AddAction(keyName, binding: $"<Keyboard>/{codes[0]}");
        }
        InputSystem.RegisterBindingComposite<AssignOneComposite>("assignOne_Composite");

        if (onPress != null)
        {
            action.performed += ctx =>
            {
                Plugin.Log($"{keyName} action performed!");
                onPress.Invoke();
            };
        }

        if (onRelease != null)
        {
            action.canceled += ctx =>
            {
                Plugin.Log($"{keyName} action canceled!");
                onRelease.Invoke();
            };
        }

        Plugin.Log($"Registered key {keyName} with code {string.Join(",", codes)}");
    }
    
    [HarmonyPatch(typeof(KeyBindingsPanel), nameof(KeyBindingsPanel.OnEnable))]
    [HarmonyPrefix]
    public static void KeyBindingsPanel_Start_Postfix(KeyBindingsPanel __instance)
    {
        if (actionMap == null)
        {
            Plugin.Log("Action map is null, returning.");
            return;
        }
        
        if (slots == null)
        {
            Plugin.Log("Slots are null, creating new slots.");
            Transform parent = __instance.slots[0].transform.parent;
            Transform clone = GameObject.Instantiate(parent, parent.parent);
            clone.gameObject.name = "Modded";
            slots = new List<KeyBindingSlot>();
            slots.AddRange(clone.GetComponentsInChildren<KeyBindingSlot>(true));

            Transform header = clone.Find("Header");
            TMP_Text headerText = header.GetComponent<TMP_Text>();
            GameObject.Destroy(header.GetComponent<LocalizationText>());
            GameObject.Destroy(header.GetComponent<TextFontFeaturesHelper>());
            headerText.text = s_HOTKEY_NAME;
            
            Plugin.Log("Slots created.");
        }
        else
        {
            Plugin.Log("Slots are already set up.");
        }
    }

    [HarmonyPatch(typeof(KeyBindingsPanel), nameof(KeyBindingsPanel.SetUpSlots))]
    [HarmonyPostfix]
    public static void KeyBindingsPanel_SetUpSlots_Postfix(KeyBindingsPanel __instance)
    {
        if (actionMap == null)
        {
            Plugin.Log("Action map is null, returning.");
            return;
        }
        
        Plugin.Log("Setting up key bindings. " + actionMap.actions.Count + " actions and " + slots.Count + " slots.");
        foreach (InputAction action in actionMap.actions)
        {
            KeyBindingSlot slot = __instance.GetOrCreate(slots, usedSlots++);
            slot.SetUp(action, __instance.OnChangeRequested);
        }
    }
    
    [HarmonyPatch(typeof(InputConfig), MethodType.Constructor)]
    [HarmonyPostfix]
    private static void HookMainControllerSetup(InputConfig __instance)
    {
        Plugin.Log($"Setting up custom action map.");
        InputAsset = __instance.asset;
        
        // Create an action map containing a single action with a gamepad binding.
        InputActionMap map = InputAsset.FindActionMap(s_HOTKEY_NAME);
        if (map != null)
        {
            InputAsset.RemoveActionMap(map);
        }

        actionMap = new InputActionMap(s_HOTKEY_NAME);
        InputAsset.AddActionMap(actionMap);
        
        // Let's assume we have two gamepads connected. If we enable the
        // action map now, the 'Fire' action will bind to both.
        actionMap.Enable();
        
        foreach (var pendingHotkey in pendingHotkeys)
        {
            RegisterKey(pendingHotkey.keyName, pendingHotkey.codes, pendingHotkey.onRelease);
        }
    }

    [HarmonyPatch(typeof(KeyBindingsPanel), nameof(KeyBindingsPanel.ResetCounter))]
    [HarmonyPostfix]
    private static void KeyBindingsPanel_ResetCounter(InputConfig __instance)
    {
        Plugin.Log($"Reset counter.");
        usedSlots = 0;
    }

    [HarmonyPatch(typeof(InputService), nameof(InputService.GetShortcutLabel))]
    [HarmonyPrefix]
    private static bool InputService_GetShortcutLabel(InputAction action, ref string __result)
    {
        foreach (InputAction mapAction in actionMap.actions)
        {
            if(mapAction == action)
            {
                __result = mapAction.name;
                return false;
            }
        }

        return true;
    }
    
    // Register the composite
    [RuntimeInitializeOnLoadMethod]
    private static void RegisterComposite()
    {
        InputSystem.RegisterBindingComposite<AssignOneComposite>("assignOne_Composite");
    }
}
