using Config;

namespace GameScripts.Config
{
public class Config : IConfig
{
    public T GetImage<T>() where T : struct
    {
        return default;
    }
}
}