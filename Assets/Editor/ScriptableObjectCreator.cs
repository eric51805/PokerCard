using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectCreator
{
    [MenuItem("Assets/Create/Create ScriptableObject")]
    public static void CreateScriptableObjectAsset()
    {

        ScriptableObject obj = null;

        if (Selection.activeObject is MonoScript)
        {
            var script = (MonoScript)Selection.activeObject;
            System.Type type = script.GetClass();
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                obj = ScriptableObject.CreateInstance(type.ToString());
            }
        }

        if (obj == null) return;
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        path = path.Replace(Path.GetFileName(path), string.Empty);
        path = AssetDatabase.GenerateUniqueAssetPath(path + Path.AltDirectorySeparatorChar + Selection.activeObject.name + ".asset");
        AssetDatabase.CreateAsset(obj, path);
        Selection.activeObject = obj;
    }

    public static T Create<T>(string path) where T : ScriptableObject
    {
        var data = ScriptableObject.CreateInstance<T>();
        if (!Directory.Exists(path)) return data;
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
        Selection.activeObject = data;
        return data;
    }
}