using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public static class UpgradeEnumGenerator
{
    [MenuItem("Tools/Generate UpgradeType Enum")]
    public static void Generate()
    {
        var interfaceType = typeof(IUpgradeEffect);
        var upgradeTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => interfaceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .Select(t => t.Name)
            .Distinct()
            .ToList();

        string enumContent = "public enum UpgradeType\n{\n";
        foreach (var typeName in upgradeTypes)
            enumContent += $"    {typeName},\n";
        enumContent += "}";

        string path = "Assets/Main/Scripts/Upgrade/UpgradeType.cs";
        File.WriteAllText(path, enumContent);
        AssetDatabase.Refresh();
        Debug.Log($"UpgradeType enum generated with {upgradeTypes.Count} entries.");
    }
}
#endif
