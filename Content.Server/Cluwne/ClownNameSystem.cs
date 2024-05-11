using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Server.Access.Systems;
namespace Content.Server.Cluwne;

public sealed class ClownNameSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _data = default!;
    [Dependency] private readonly IdCardSystem _card = default!;
    [Dependency] private readonly QuickDialogSystem _dialog = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ClownNameComponent, PlayerSpawnCompleteEvent>(OnInit);
    }
    public void OnInit(EntityUid uid,ClownNameComponent comp ,  PlayerSpawnCompleteEvent init){
        if(!TryComp<ActorComponent>(uid,out var actor))
            return;
        if(!TryComp<MetaDataComponent>(uid,out var data))
            return;
        if(!_card.TryFindIdCard(uid, out var card))
            return;
        _dialog.OpenDialog(actor.PlayerSession,"Твое имя","Введи свое имя",(string name)=>{
            if(name!="" && _card.TryChangeFullName(card,name)){
                _data.SetEntityName(uid,name,data);
            }
        });
    }

}
