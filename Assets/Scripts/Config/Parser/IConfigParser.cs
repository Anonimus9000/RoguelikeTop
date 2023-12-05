namespace Config.Parser
{
public interface IConfigParser<T>
{
    public IConfig ParseConfig(T config);
}
}