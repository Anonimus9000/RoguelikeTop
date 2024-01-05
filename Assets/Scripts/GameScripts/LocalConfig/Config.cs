using System.Collections.Generic;
using Config.MainConfig;
using Logger;

namespace GameScripts.LocalConfig
{
public class Config : IConfig
{
    private readonly IInGameLogger _logger;
    private List<IImage> Images;
    
    public Config(IInGameLogger logger, IImage playerImage, IImage cameraImage)
    {
        _logger = logger;
        Images = new List<IImage>
        {
            playerImage,
            cameraImage,
        };
    }

    public T GetImage<T>() where T : IImage
    {
        foreach (var image in Images)
        {
            if (image is T concreteImage)
            {
                return concreteImage;
            }
        }

        _logger.LogError($"Can't find image {nameof(T)}");
        return default;
    }
}
}