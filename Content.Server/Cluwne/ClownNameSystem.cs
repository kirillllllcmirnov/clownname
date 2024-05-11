using Content.Server.Administration.Commands;
using Content.Server.Popups;
using Content.Shared.Popups;
using Content.Shared.Mobs;
using Content.Server.Chat;
using Content.Server.Chat.Systems;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Stunnable;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Damage;
using Robust.Shared.Prototypes;
using Content.Server.Emoting.Systems;
using Content.Server.Speech.EntitySystems;
using Content.Shared.Cluwne;
using Content.Shared.Interaction.Components;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Content.Server.GameTicking.Rules;
using Content.Shared.Roles.Jobs;
using Content.Shared.Roles;
using Content.Shared.Administration.Logs;
using Content.Shared.Database;
using Content.Shared.Body.Components;
using Content.Server.Chat.Managers;
using Content.Shared.Chat;
using Robust.Server.GameObjects;
using Robust.Shared.Player;
using Content.Shared.Surgery;
using FastAccessors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Content.Server.Cluwne;

public sealed class ClownNameSystem : EntitySystem
{
    [Dependency] private readonly MetaDataSystem _data = default!;
    [Dependency] private readonly SharedJobSystem _jobs = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _ui = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MetaDataComponent,  MindGetAllRolesEvent>(OnJobGetAddRoles);
        SubscribeLocalEvent<ClownNameComponent,  ComponentInit>(OnInit);
        SubscribeLocalEvent<ClownNameComponent,  SendClownNameInterfaceMessage>(OnMessage);
    }
    public void OnInit(EntityUid uid,ClownNameComponent comp , ComponentInit init){
        if(!TryComp<ActorComponent>(uid,out var actor)){return;}
        if(!TryComp<UserInterfaceComponent>(uid,out var ui) && ui==null){return;}
        if(_ui.TryGetUi(uid,ClownNameUiKey.Key,out var bui,ui)){
            _ui.OpenUi(bui,actor.PlayerSession);
        }
    }
    public void OnMessage(EntityUid uid,ClownNameComponent comp , SendClownNameInterfaceMessage msg){
        if(msg.Name=="")return;
        if(TryComp<MetaDataComponent>(uid,out var data)){
            _data.SetEntityName(uid,msg.Name,data);
        }
    }
    public void OnJobGetAddRoles(EntityUid uid,MetaDataComponent comp , MindGetAllRolesEvent args ){
        var user=new EntityUid(uid.Id+1);
        if (!_jobs.MindTryGetJobName(uid,out var name))
            return;
        if(name=="Clown"){
            AddComp<ClownNameComponent>(user);
        }


    }



}
