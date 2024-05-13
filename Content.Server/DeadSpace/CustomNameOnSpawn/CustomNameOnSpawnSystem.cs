using Content.Server.Administration;
using Content.Server.GameTicking;
using Content.Server.Access.Systems;
using Content.Server.StationRecords.Systems;
using Robust.Shared.Player;
using Content.Shared.StationRecords;

namespace Content.Server.DeadSpace;

public sealed class CustomNameOnSpawnSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _data = default!;
    [Dependency] private readonly IdCardSystem _cardSystem = default!;
    [Dependency] private readonly QuickDialogSystem _quickDialog = default!;
    [Dependency] private readonly StationRecordsSystem _record = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<CustomNameOnSpawnComponent, PlayerSpawnCompleteEvent>(OnInit);
    }

    public void OnInit(EntityUid uid, CustomNameOnSpawnComponent comp, PlayerSpawnCompleteEvent args)
    {
        if (!TryComp<ActorComponent>(uid, out var actor))
            return;

        if (!TryComp<MetaDataComponent>(uid, out var data))
            return;

        if (!_cardSystem.TryFindIdCard(uid, out var card))
            return;

        _quickDialog.OpenDialog(actor.PlayerSession, "Введи новое имя персонажа", "Новое имя персонажа:", (string name) =>
        {
            if (name == "")
                return;

            if (!TryComp<StationRecordKeyStorageComponent>(card, out var keyStorage) || keyStorage.Key is not { } key
                || !_record.TryGetRecord<GeneralStationRecord>(key, out var record))
                return;

            _cardSystem.TryChangeFullName(card, name);
            _data.SetEntityName(uid, name, data);
            record.Name = name;
            _record.Synchronize(key);
        });
    }
}
