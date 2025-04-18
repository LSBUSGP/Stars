using UnityEngine;
using UnityEngine.VFX;

[System.Serializable]
[VFXType(VFXTypeAttribute.Usage.GraphicsBuffer)]
public struct StarParticle
{
    public const int sizeOf = 28;
    public Vector3 colour;
    public Vector3 position;
    public float size;
}
