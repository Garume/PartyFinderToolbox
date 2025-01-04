using ImGuiNET;

namespace PartyFinderToolbox.Shared.Utility;

public static class ImGuiValue
{
    public static bool Checkbox(string label, bool value, Action? onChange = null)
    {
        var refValue = value;
        if (ImGui.Checkbox(label, ref refValue))
        {
            onChange?.Invoke();
            return refValue;
        }

        return value;
    }

    public static string InputText(string label, string value, uint maxLength, Action? onChange = null)
    {
        var refValue = value;
        if (ImGui.InputText(label, ref refValue, maxLength))
        {
            onChange?.Invoke();
            return refValue;
        }

        return value;
    }

    public static uint InputUInt(string label, uint value, uint min, uint max, Action? onChange = null)
    {
        var refValue = (int)value;
        if (ImGui.InputInt(label, ref refValue))
        {
            onChange?.Invoke();
            return (uint)Math.Clamp(refValue, min, max);
        }

        return value;
    }
}