namespace PartyFiltering.Services;

public class PartyFilteringServiceAttribute(bool disable = false) : Attribute
{
    public bool Disable { get; set; } = disable;
}