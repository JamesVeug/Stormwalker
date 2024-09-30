using System.IO;
using ATS_API;
using BepInEx;
using Eremite;
using Eremite.Controller;
using Eremite.Services;
using Eremite.View.HUD.Construction;
using HarmonyLib;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stormwalker;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static readonly float SUPER_SPEED_SCALE = 5f;
    
    public static void Log(object obj) => Instance.Logger.LogInfo(obj);
    public static void Warning(object obj) => Instance.Logger.LogWarning(obj);
    public static void Error(object obj) => Instance.Logger.LogError(obj);
    public static PluginState State { get; private set; } = new();
        
    public static Plugin Instance;
    public static BuildingsPanel buildingPanel = null;

    private Harmony harmony;
    private Vector2? zoomLimit = null;

    private void Awake()
    {
        Logger.LogInfo($"Loading Plugin {PluginInfo.PLUGIN_GUID}...");
        Instance = this;
        harmony = Harmony.CreateAndPatchAll(typeof(Patches).Assembly, PluginInfo.PLUGIN_GUID);

        Configs.Initialize();
        Hotkeys.RegisterKey("Stormwalker", "zoom", "Zoom Overview", [KeyCode.Backspace], ZoomToggled);
        Hotkeys.RegisterKey("Stormwalker", "5X", "5X Speed", [KeyCode.Alpha5], SuperSpeedToggled);

        gameObject.AddComponent<Woodcutters>();
        gameObject.AddComponent<BuildingCopier>();
        
        // Stops Unity from destroying it for some reason. Same as Setting the BepInEx config HideManagerGameObject to true.
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private void Update()
    {
        Hotkeys.Update();
    }

    public static void SetupState(bool noGameState)
    {
        if (noGameState || MB.GameSaveService.IsNewGame())
        {
            State = new();
        }
        else
        {
            State = Load();
        }

        MB.GameSaveService.IsSaving.Where(isStarting => !isStarting).Subscribe(_ => Save());
    }

    private static void Save()
    {
        var path = Path.Combine(Application.persistentDataPath, "Stormwalker.save");
        Log($"Saving state to {path}");
        JsonIO.SaveToFile(State, path);
    }

    private static PluginState Load()
    {
        var path = Path.Combine(Application.persistentDataPath, "Stormwalker.save");
        if (!File.Exists(path))
        {
            Log("No save state found, creating new one!");
            return new PluginState();
        }
        
        try
        {
            PluginState pluginState = JsonIO.GetFromFile<PluginState>(path);
            Log($"Loaded state from {path}");
            return pluginState;
        }
        catch
        {
            Error("Error while trying to load save state from " + path);
            return new();
        }
    }

    private static void SuperSpeedToggled(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameMB.TimeScaleService.Change(SUPER_SPEED_SCALE, true, false);
        }
    }

    private void ZoomToggled(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        if (!zoomLimit.HasValue)
        {
            zoomLimit = GameController.Instance.CameraController.zoomLimit;
        }
            
        var zoom = Configs.ZoomLimit;
        var cam = GameController.Instance.CameraController;
        if (cam.zoomLimit.x == zoom && cam.zoomLimit.y == zoom)
        {
            // Zoom back to normal
            cam.zoomLimit = zoomLimit.Value;
        }
        else
        {
            // Zoom out
            zoomLimit = cam.zoomLimit;
            cam.zoomLimit = new Vector2(zoom, zoom);
        }
    }

    private void OnDestroy()
    {
        harmony.UnpatchSelf();
    }
}