using System;
using UnityEngine;

namespace GameScripts.Player.Provider
{
public interface IPlayerProvider : IDisposable
{
    public Transform GetPlayerTransform();
}
}