using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Server.Access.Systems;
using Content.Server.StationRecords.Systems;
namespace Content.Server.Cluwne;

public sealed class ClownNameSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _data = default!;
    [Dependency] private readonly IdCardSystem _card = default!;
    [Dependency] private readonly QuickDialogSystem _dialog = default!;
    [Dependency] private readonly StationRecordsSystem _record = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ClownNameComponent, PlayerSpawnCompleteEvent>(OnInit);
    }
    public void OnInit(EntityUid uid,ClownNameComponent comp ,  PlayerSpawnCompleteEvent init){
        if(!TryComp<ActorComponent>(uid,out var actor)){return;}
        if(!TryComp<MetaDataComponent>(uid,out var data)){return;}
        if(!_card.TryFindIdCard(uid, out var card)){return;}
        _dialog.OpenDialog(actor.PlayerSession,"Твое имя","Введи свое имя",(string name)=>{
            if(name=="" )return;

            if (!TryComp<StationRecordKeyStorageComponent>(card, out var keyStorage)
                || keyStorage.Key is not { } key
                || !_record.TryGetRecord<GeneralStationRecord>(key, out var record))
            {
                return;
            }

            _card.TryChangeFullName(card,name);
            _data.SetEntityName(uid,name,data);
            record.Name=name;
            _record.Synchronize(key);
            });
    }

}
