namespace Config
{
public interface IConfig
{
    public T GetImage<T>() where T : struct;
}
}