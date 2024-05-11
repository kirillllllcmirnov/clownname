using Content.Shared.FixedPoint;
using Content.Shared.Store;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using SixLabors.ImageSharp;

namespace Content.Shared.Cluwne;


[Serializable, NetSerializable]
public sealed class SendClownNameInterfaceMessage : BoundUserInterfaceMessage
{
    public string Name;
    public SendClownNameInterfaceMessage(string name){
        Name = name;
    }
}
