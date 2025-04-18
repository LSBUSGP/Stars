using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;
using System.Collections.Generic;

public class InitializeStarsGraphicsBuffer : MonoBehaviour
{
    [SerializeField] VisualEffect starsEffect;
    [SerializeField] ExposedProperty bufferProperty = "Stars";
    [SerializeField] ExposedProperty bufferCountProperty = "Stars Count";
    [SerializeField] BrightStarCatalogue catalogue;

    GraphicsBuffer graphicsBuffer;

    void Awake()
    {
        EnsureBufferCapacity(ref graphicsBuffer, catalogue.stars.Count, StarParticle.sizeOf, starsEffect, bufferProperty);
    }

    void LateUpdate()
    {
        // Set Buffer data, but before that ensure there is enough capacity
        EnsureBufferCapacity(ref graphicsBuffer, catalogue.stars.Count, StarParticle.sizeOf, starsEffect, bufferProperty);
        graphicsBuffer.SetData(catalogue.stars);
        // Update current number of elements
        starsEffect.SetInt(bufferCountProperty, catalogue.stars.Count);
    }

    void OnDestroy()
    {
        ReleaseBuffer(ref graphicsBuffer);
    }

    private void EnsureBufferCapacity(ref GraphicsBuffer buffer, int capacity, int stride, VisualEffect vfx, int vfxBufferProperty)
    {
        // Reallocate new buffer only when buffer is null or capacity is not sufficient
        if (buffer == null || buffer.count < capacity)
        {
            // Buffer memory must be released
            buffer?.Release();
            // Vfx Graph uses structured buffer
            buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, capacity, stride);
            // Update buffer referenece
            vfx.SetGraphicsBuffer(vfxBufferProperty, buffer);
        }
    }

    private void ReleaseBuffer(ref GraphicsBuffer buffer)
    {
        // Buffer memory must be released
        buffer?.Release();
        buffer = null;
    }
}
