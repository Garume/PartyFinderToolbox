namespace PartyFiltering.Shared.Services;

[AttributeUsage(AttributeTargets.Class)]
public class LoadServiceAttribute() : Attribute
{
    public bool Disable { get; set; } = false;
    public int Priority { get; set; } = 0;
}