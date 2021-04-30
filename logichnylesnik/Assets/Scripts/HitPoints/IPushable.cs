using UnityEngine;

public interface IPushable
{
    void PushAgainst(Vector3 againstPosition, float pushPower);
}