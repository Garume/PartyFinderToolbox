using System.Reflection;

namespace PartyFiltering.Shared.Utility;

public static class TypeCache
{
    private static readonly Dictionary<Type, Type[]?> TypesWithAttribute = new();
    private static readonly Dictionary<Type, Type[]?> DerivedTypes = new();

    public static Type[]? TryGetTypesWithAttribute<T>() where T : Attribute
    {
        var attribute = typeof(T);
        if (TypesWithAttribute.TryGetValue(attribute, out var types)) return types;

        types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.GetCustomAttributes().Any(x => x.GetType() == attribute))
            .ToArray();
        TypesWithAttribute.Add(attribute, types);
        return types;
    }

    public static IEnumerable<Type>? TryGetDerivedTypes<T>()
    {
        var type = typeof(T);
        if (DerivedTypes.TryGetValue(type, out var types)) return types;

        types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(p => type.IsAssignableFrom(p) && p != type)
            .ToArray();
        DerivedTypes.Add(type, types);
        return types;
    }
}