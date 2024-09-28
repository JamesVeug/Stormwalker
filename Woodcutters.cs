using System;
using System.Linq;
using BepInEx.Configuration;
using Eremite;
using Eremite.Buildings;
using Eremite.Characters.Villagers;
using Eremite.Model.State;
using Eremite.View.HUD.Woodcutters;

namespace Stormwalker;

public class Woodcutters : GameMB {

    private WoodcuttersHUD woodcuttersHUD;

    private void Awake()
    {
        InputConfigs.RegisterKey("assignOne", Configs.Woodcutters_AssignOne, ()=>woodcuttersHUD.OnRightClick());
        InputConfigs.RegisterKey("unassignAll", Configs.Woodcutters_UnassignAll, ()=>woodcuttersHUD.OnClick());
        InputConfigs.RegisterKey("balanceHostility", Configs.Woodcutters_BalanceHostility, ()=>UnassignSomePressed());
    }

    public void PatchPanel(WoodcuttersHUD hud){
        woodcuttersHUD = hud;
        hud.transform.localPosition = new(120f, -71f, 0f);
    }

    private void UnassignSomePressed()
    {
        if(canLowerHostility(out int numUnassigned)){
            UnassignSome(numUnassigned);
            Publish(numUnassigned);
        }
    }

    private int UnassignAll(Camp camp){
        var workers = camp.state.workers.Where(i=>i>0).Count();
        if(workers > 0) camp.ClearWorkers();
        return workers;
    }

    private bool canLowerHostility(out int amount){
        if((HostilityService as Eremite.Services.HostilityService).IsFirstLevel()){
            amount = -1;
            return false;
        }

        int woodcutterHostility = HostilityService.GetPointsFor(HostilitySource.Woodcutter);
        if(woodcutterHostility <= 0){
            amount = -1;
            return false;
        }
            
        int currentPoints = HostilityService.Points.Value + 1;
        int numWoodcutters = HostilityService.GetSourceAmount(HostilitySource.Woodcutter);
        amount = (int)Math.Ceiling((double)currentPoints * numWoodcutters / woodcutterHostility);
        // Plugin.Log($"tot wc host: {woodcutterHostility}, cp: {currentPoints}, nw: {numWoodcutters} amount: {amount}");
        return numWoodcutters >= amount;
    }

    private void UnassignSome(int toUnassign){
        for(int i = 0; i < 6; i++) { // instead of infinite; as a failsafe
            foreach (var camp in BuildingsService.Camps.Values){
                toUnassign -= TryUnassignOne(camp);
                if(toUnassign == 0) return;
            }
        }
    }

    private int TryUnassignOne(Camp camp){
        foreach (int num in camp.state.workers) {
            if (num > 0){
                camp.GetVillagerRole<Forager>(num).ReactToRemovedCamp();
                return 1;
            }
        }
        return 0;
    }

    private void Publish(int number){
        if(number > 0) NewsService.PublishNews($"Unassigned {number} woodcutters");
    }
}