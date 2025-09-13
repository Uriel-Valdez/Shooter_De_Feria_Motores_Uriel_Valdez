using UnityEngine;

public interface IShootable
{
    void OnShot(Shooter shooter, UnityEngine.RaycastHit hit);
}
