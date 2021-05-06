using UnityEditor;

/// <summary>
/// This script verifies if there is a folder named "Local Depot" in the asset folder and if there isn't one it creates it
///
/// It is a place to store files that will be ignored by github
/// </summary>

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        if(!AssetDatabase.IsValidFolder("Assets/Local Depot")){
            AssetDatabase.CreateFolder("Assets", "Local Depot");
        }
    }
}