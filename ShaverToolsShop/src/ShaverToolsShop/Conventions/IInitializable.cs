namespace ShaverToolsShop.Conventions
{
    public interface IInitializable
    {
        int Order { get; }
        void Initialize();
    }
}