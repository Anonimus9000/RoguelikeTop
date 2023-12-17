using Config;

namespace GameScripts.LocalConfig
{
public class Config : IConfig
{
    public T GetImage<T>() where T : struct
    {
        return default;
    }
}
}