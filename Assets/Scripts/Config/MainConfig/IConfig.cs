namespace Config.MainConfig
{
public interface IConfig
{
    public T GetImage<T>() where T : IImage;
}
}