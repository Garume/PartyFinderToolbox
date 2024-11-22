namespace PartyFiltering.UI;

[AttributeUsage(AttributeTargets.Class)]
public class FilterUIAttribute(Type FilterType, bool Debug = false) : Attribute;