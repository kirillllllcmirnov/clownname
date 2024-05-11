using Robust.Shared.Player;
using Content.Server.Administration;
using Content.Server.GameTicking;
namespace Content.Server.Cluwne;

public sealed class ClownNameSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _data = default!;
    [Dependency] private readonly QuickDialogSystem _dialog = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ClownNameComponent, PlayerSpawnCompleteEvent>(OnSpawn);
    }
    public void OnSpawn(EntityUid uid,ClownNameComponent comp ,  PlayerSpawnCompleteEvent init){
        if(!TryComp<ActorComponent>(uid,out var actor)){return;}
        if(!TryComp<MetaDataComponent>(uid,out var data)){return;}
        _dialog.OpenDialog(actor.PlayerSession,"Твое имя","Введи свое имя",(string name)=>{
            if(name!=""){
                _data.SetEntityName(uid,name,data);
            }
        });
    }

}
