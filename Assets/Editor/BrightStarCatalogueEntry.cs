using UnityEngine;

public struct BrightStarCatalogueEntry
{
    // light temperature in K
    public int temperature;
    // celestial coordinates J2000 in radians
    public float rightAscension;
    public float declination;
    // apparent scale 10^(-mv/5) 
    public float size;

    public BrightStarCatalogueEntry(int temperature, float rightAscension, float declination, float size)
    {
        this.temperature = temperature;
        this.rightAscension = rightAscension;
        this.declination = declination;
        this.size = size;
    }

    public StarParticle CreateStarParticle()
    {
        Color colour = Mathf.CorrelatedColorTemperatureToRGB(temperature);
        Vector3 position;
        float rA = rightAscension;
        float decl = declination;
        position.x = Mathf.Cos(rA) * Mathf.Cos(decl);
        position.y = Mathf.Sin(rA) * Mathf.Cos(decl);
        position.z = Mathf.Sin(decl);
        return new ()
        {
            colour = (Vector4)colour,
            position = position,
            size = size
        };
    }
}
