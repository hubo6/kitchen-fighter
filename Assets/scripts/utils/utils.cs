using System.Linq;
using UnityEngine;
    // Start is called before the first frame update
public static class utils {
    public static T GetComponentInChildren<T>(this GameObject obj, bool excludeParent = false, bool includeInactive = false) where T : Component {
        var components = obj.GetComponentsInChildren<T>(includeInactive);

        if (!excludeParent)
            return components.FirstOrDefault();

        return components.FirstOrDefault(childComponent =>
            childComponent.transform != obj.transform);
    }
}

