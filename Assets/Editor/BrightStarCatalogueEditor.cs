using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(BrightStarCatalogue))]
public class BrightStarCatalogueEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new ();
        Editor editor = CreateEditor(target);
        IMGUIContainer inspectorIMGUI = new (() => { editor.OnInspectorGUI(); });
        root.Add(inspectorIMGUI);
        root.Add(new Button(() => { InitializeStars(); }) { text = "Initialize Star List" });
        return root;
    }

    void InitializeStars()
    {
        BrightStarCatalogue starList = (BrightStarCatalogue)target;
        List<StarParticle> stars = new ();
        foreach (BrightStarCatalogueEntry entry in BrightStarCatalogueData.Entries())
        {
            stars.Add(entry.CreateStarParticle());
        }

        starList.stars = stars;
    }
}
