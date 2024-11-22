namespace PartyFiltering.UI;

public abstract class Tab : IUI
{
    public abstract string Name { get; }
    public abstract void Draw();
}