using Config.MainConfig;
using Config.Parser;
using GameScripts.LocalConfig.Images;
using GameScripts.LocalConfig.SOConfig;
using GameScripts.LocalConfig.SOConfig.SoImages;
using Logger;

namespace GameScripts.LocalConfig
{
public class SOConfigParser : IConfigParser
{
    private readonly SoMainConfig _soMainConfig;
    private readonly IInGameLogger _logger;

    public SOConfigParser(SoMainConfig soMainConfig, IInGameLogger logger)
    {
        _soMainConfig = soMainConfig;
        _logger = logger;
    }
    
    public IConfig ParseConfig()
    {
        var playerSoImage = _soMainConfig.PlayerSoImage;
        var cameraSoImage = _soMainConfig.CameraSoImage;

        var playerImage = ParsePlayerImage(playerSoImage);
        var cameraImage = ParseCameraImage(cameraSoImage);
        
        IConfig config = new Config(_logger, playerImage, cameraImage);

        return config;
    }

    private IImage ParseCameraImage(CameraSoImage cameraSoImage)
    {
        var cameraSmoothSpeed = cameraSoImage.CameraSmoothSpeed;
        var cameraTargetOffset = cameraSoImage.CameraTargetOffset;
        var cameraFieldOfView = cameraSoImage.CameraFieldOfView;
        var delayOnTargetDuration = cameraSoImage.DelayOnTargetDuration;
        var cinematicMoveToTargetDuration = cameraSoImage.CinematicMoveToTargetDuration;

        var cameraImage = new CameraImage(cinematicMoveToTargetDuration, delayOnTargetDuration, cameraTargetOffset,
            cameraSmoothSpeed, cameraFieldOfView);

        return cameraImage;
    }

    private IImage ParsePlayerImage(PlayerSoImage playerSoImage)
    {
        var moveMoveSpeed = playerSoImage.MoveSpeed;
        var rotationSpeed = playerSoImage.RotationSpeed;
        
        var playerImage = new PlayerImage(moveMoveSpeed, rotationSpeed);

        return playerImage;
    } 
}
}