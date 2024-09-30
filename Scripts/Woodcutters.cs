using System;
using ATS_API;
using Eremite;
using Eremite.Buildings;
using Eremite.Characters;
using Eremite.Model.State;
using Eremite.View.HUD.Woodcutters;
using UnityEngine;

namespace Stormwalker;

public class Woodcutters : GameMB {

    private WoodcuttersHUD woodcuttersHUD;

    private void Awake()
    {
        Hotkeys.RegisterKey("Stormwalker", "assign1", "Assign one Villager", [KeyCode.LeftControl,KeyCode.X], (ctx)=>
        {
            if (ctx.performed)
            {
                woodcuttersHUD.OnRightClick();
            }
        });
        Hotkeys.RegisterKey("Stormwalker", "unassignAll", "Unassign all Villagers", [KeyCode.LeftShift,KeyCode.X], (ctx)=>
        {
            if (ctx.performed)
            {
                woodcuttersHUD.OnClick();
            }
        });
        Hotkeys.RegisterKey("Stormwalker", "balanceHostility", "Unassign Villagers to lower Hostility", [KeyCode.LeftAlt,KeyCode.X], (ctx)=>
        {
            if (ctx.performed)
            {
                UnassignSomePressed();
            }
        });
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

    // private int UnassignAll(Camp camp){
    //     var workers = camp.state.workers.Where(i=>i>0).Count();
    //     if(workers > 0) camp.ClearWorkers();
    //     return workers;
    // }

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
        for (int i = 0; i < camp.Workers.Length; i++)
        {
            int id = camp.Workers[i];
            if (!GameMB.ActorsService.HasActor(id))
            {
                continue;
            }
            Actor actor = GameMB.ActorsService.GetActor(id);
            if (!actor.IsBoundToWorkplace)
            {
                VillagersService.ReleaseFromProfession(GameMB.VillagersService.GetVillager(camp.Workers[i]));
                return 1;
            }
        }
        
        return 0;
    }

    private void Publish(int number){
        if(number > 0) NewsService.PublishNews($"Unassigned {number} woodcutters");
    }
}