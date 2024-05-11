using Content.Shared.Administration.Logs;
using Content.Shared.Cluwne;
using Content.Shared.Database;
using Content.Shared.Surgery;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
namespace Content.Client.Clown;

[UsedImplicitly]
public sealed class ClownNameUserInterface : BoundUserInterface
{

    private ClownNameMenu? _menu;

    public ClownNameUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }
    protected override void Open()
    {
        _menu = new ClownNameMenu("aboba");
        _menu.OpenCentered();
        _menu.OnSendButtonPressed+=(args,name)=>{
            SendMessage(new SendClownNameInterfaceMessage(name));
        } ;
    }
}
