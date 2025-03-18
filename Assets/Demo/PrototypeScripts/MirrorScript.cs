using UnityEngine;

public class MirrorScript : MonoBehaviour, IReflective
{
    public Vector2 ReflectBeam(Vector2 incomingDirection, Vector2 hitPoint)
    {
        Vector2 normalizedSurface = (Vector2)(incomingDirection - hitPoint).normalized;
        Vector2 directionReflected = Vector2.Reflect(incomingDirection, normalizedSurface);
        return directionReflected;
    }
}
