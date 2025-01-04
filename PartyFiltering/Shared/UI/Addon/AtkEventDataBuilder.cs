using FFXIVClientStructs.FFXIV.Component.GUI;

namespace PartyFinderToolbox.Shared.UI.Addon;

public unsafe class AtkEventDataBuilder
{
    public readonly AtkEventData Data;

    public AtkEventDataBuilder Write<T>(int pos, T data) where T : unmanaged
    {
        fixed (AtkEventData* eventDataPtr = &Data)
        {
            var ptr = (nint)eventDataPtr + pos;
            *(T*)ptr = data;
        }

        return this;
    }

    public AtkEventData Build()
    {
        return Data;
    }

    public static AtkEventDataBuilder Create()
    {
        return new AtkEventDataBuilder();
    }
}