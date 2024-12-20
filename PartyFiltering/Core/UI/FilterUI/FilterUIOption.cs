namespace PartyFiltering.Core.UI;

[Flags]
public enum FilterUIOption : byte
{
    None = 0,
    AlwaysEnabled = 1,
    NoSelector = 2,
    NoOption = 3,
    NoDelete = 5
}