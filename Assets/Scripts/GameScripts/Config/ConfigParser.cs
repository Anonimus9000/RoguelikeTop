using Config;
using Config.Parser;

namespace GameScripts.Config
{
public class ConfigParser : IConfigParser<SoConfig>
{
    public IConfig ParseConfig(SoConfig config)
    {
        return new Config();
    }
}
}