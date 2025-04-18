using System.Collections.Generic;

public class BrightStarCatalogueData
{
    public static IEnumerable<BrightStarCatalogueEntry> Entries()
    {
        yield return new BrightStarCatalogueEntry(10000, 1.0f, 0.0f, 1.0f);
    }
}
