using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// Validates build settings for UnityGLTF
/// 
/// Includes a pre-build hook and an explicit menu item to validate settings.
/// </summary>
public class UnityGLTFPreBuildValidator : IPreprocessBuild
{
    /// <summary>
    /// Path to the shader used by GLTF.
    /// By default, this should be the PbrMetallicRoughness.shader file.
    /// If you have moved your UnityGLTF folder or need to use a different shader, set that here.
    /// </summary>
    public const string PBR_MR_SHADER_PATH = "Assets/UnityGLTF/Shaders/PbrMetallicRoughness.shader";

    #region Validator System Fields
    private List<string> BuildIssues = new List<string>() { string.Empty };
    private List<string> BuildFixes = new List<string>() { string.Empty };
    private List<Action> BuildFixActions = new List<Action>();
    private bool IssueFound = false;

    private void ApplyFixes()
    {
        foreach (var fix in BuildFixActions)
            fix();
    }

    void AddIssue(string issue, string fix = null, Action fixAction = null)
    {
        IssueFound = true;
        BuildIssues.Add(issue);
        if (fix != null)
            BuildFixes.Add(fix);
        if (fixAction != null)
            BuildFixActions.Add(fixAction);
    }

    void ClearIssues()
    {
        BuildIssues = new List<string>() { string.Empty };
        BuildFixes = new List<string>() { string.Empty };
        BuildFixActions = new List<Action>();
        IssueFound = false;
    }

    void CheckBuildSettings()
    {
    }

    void CheckPlayerSettings()
    {
        #region ensure the pbr material is preloaded
        {
            // http://answers.unity.com/answers/1417914/view.html

            var ps = Resources.FindObjectsOfTypeAll<PlayerSettings>();
            var so = new SerializedObject(ps);

            var preloadedAssets = so.FindProperty("preloadedAssets");

            if (preloadedAssets == null || preloadedAssets.isArray == false)
            {
                // could not find preloadedAssets
                throw new Exception("Hololens Build Setting Validator: Could not find preloadedAssets. Has the Unity API changed?");
            }

            // see if the material is already there
            var shaderRef = AssetDatabase.LoadAssetAtPath<Shader>(PBR_MR_SHADER_PATH);
            if (shaderRef == null)
            {
                Debug.LogError("The GLTF Shader was not included in Preloaded Assets. " +
                    "By default, this should be the 'PbrMetallicRoughness' shader included with the GLTF Plugin.\n" +
                    "The shader was not found in '" + PBR_MR_SHADER_PATH + "'. Please update" +
                    "UnityGLTFPreBuildValidator.PBR_MR_SHADER_PATH to reflect the new path of the shader.");
                AddIssue(
                    "The GLTF Shader could not be found (See console for details).",
                    "Update the GLTF Shader Path (See console for details)");
            }
            else
            {

                bool exists = false;
                for (var i = 0; i < preloadedAssets.arraySize && !exists; ++i)
                {
                    var asset = preloadedAssets.GetArrayElementAtIndex(i);
                    if (asset.objectReferenceValue == shaderRef)
                        exists = true;
                }

                if (exists == false)
                {
                    Debug.LogWarning("The GLTF Shader was not included in Preloaded Assets. " +
                        "By default, this should be the 'PbrMetallicRoughness' shader included with the GLTF Plugin.\n" +
                        "If you have moved the GLTF Plugin from it's default location of /Assets/UnityGLTF/, please update" +
                        "UnityGLTFPreBuildValidator.PBR_MR_SHADER_PATH to reflect the new path of the shader.");

                    AddIssue(
                        "The GLTF Shader is not added to the Preloaded Assets (See console for details).",
                        "Add the GLTF Shader to the Preloaded Assets list",
                        () =>
                        {
                            var i = preloadedAssets.arraySize++;

                            var asset = preloadedAssets.GetArrayElementAtIndex(i);
                            asset.objectReferenceValue = shaderRef;

                            so.ApplyModifiedProperties();
                            AssetDatabase.SaveAssets();
                        });
                }
            }
        }
        #endregion
    }

    #endregion

    #region IPreprocessBuild Implementation
    int IOrderedCallback.callbackOrder { get { return 0; } }

    /// <summary>
    /// Invoked before build begins
    /// </summary>
    /// <param name="target"></param>
    /// <param name="path"></param>
    void IPreprocessBuild.OnPreprocessBuild(BuildTarget target, string path)
    {
        if (CheckForBuildIssues())
        {
            string issuesList = string.Join(Environment.NewLine + " - ", BuildIssues.ToArray());
            string fixesList = string.Join(Environment.NewLine + " - ", BuildFixes.ToArray());

            var msg = "The following issue(s) were found:" +
                issuesList +
                Environment.NewLine +
                Environment.NewLine +
                "The following fixes will be applied if you click 'Fix Issues':" +
                fixesList;

            var fixIssues = EditorUtility.DisplayDialog(
                "GLTF Build Settings Validator",
                msg,
                "Fix Issues",
                "Don't Apply Fixes"
            );

            if (fixIssues)
                ApplyFixes();
        }
    }
    #endregion

    /// <summary>
    /// Invoked by the menu item
    /// </summary>
    [MenuItem("GLTF/Validate Build Settings...", false, 15)]
    public static void EditorCheckForBuildIssues()
    {
        var validator = new UnityGLTFPreBuildValidator();

        var hasIssues = validator.CheckForBuildIssues();

        if (hasIssues)
        {
            string issuesList = string.Join(Environment.NewLine + " - ", validator.BuildIssues.ToArray());
            string fixesList = string.Join(Environment.NewLine + " - ", validator.BuildFixes.ToArray());

            var msg = "The following issue(s) were found:" +
                issuesList +
                Environment.NewLine +
                Environment.NewLine +
                "The following fixes will be applied if you click 'Fix Issues':" +
                fixesList;

            var fixIssues = EditorUtility.DisplayDialog(
                "GLTF Build Settings Validator",
                msg,
                "Fix Issues For Me",
                "Don't Fix"
            );

            if (fixIssues)
                validator.ApplyFixes();
        }
        else
        {
            EditorUtility.DisplayDialog(
                "GLTF Build Settings Validator",
                "There are no issues with your build and player settings",
                "OK"
            );
        }
    }

    /// <summary>
    /// Performs the Build Check
    /// </summary>
    private bool CheckForBuildIssues()
    {
        ClearIssues();

        CheckBuildSettings();
        CheckPlayerSettings();

        return IssueFound;
    }
}
