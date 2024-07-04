
using UnityEngine;

namespace ExtensionMethods;
public static class MyExtensions1
{
    public static Transform FindChildByName(this Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                return child;
            }

            Transform result = FindChildByName(child, name);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}