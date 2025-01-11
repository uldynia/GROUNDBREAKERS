#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEngine;

[CreateAssetMenu]
public class BuildAutomator : ScriptableObject
{
    [SerializeField] public BuildProfile[] profiles;
}
[CustomEditor(typeof(BuildAutomator))]
public class BuildAutomatorEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Build!")) {
            var ba = (BuildAutomator)target;
            foreach(var profile in ba.profiles) {
                var buildPlayerOptions = new BuildPlayerWithProfileOptions();
                buildPlayerOptions.buildProfile = profile;
                buildPlayerOptions.options |= BuildOptions.ShowBuiltPlayer;
                buildPlayerOptions.locationPathName = $"Builds/{profile.name}/{profile.name}";
                BuildPipeline.BuildPlayer(buildPlayerOptions);
            }
        }   
    }
}
#endif