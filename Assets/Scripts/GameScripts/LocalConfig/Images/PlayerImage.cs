using Config.MainConfig;

namespace GameScripts.LocalConfig.Images
{
public sealed class PlayerImage : IImage
{
    public float MoveSpeed { get; }
    public float RotationSpeed { get; }

    public PlayerImage(float moveMoveSpeed, float rotationSpeed)
    {
        MoveSpeed = moveMoveSpeed;
        RotationSpeed = rotationSpeed;
    }
}
}