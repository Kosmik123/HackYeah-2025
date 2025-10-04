#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class FogColorSynchronization
{
    private const string EnableSyncMenuItemPath = "CONTEXT/Camera/Enable Synchronize Fog Color";
    private const string DisableSyncMenuItemPath = "CONTEXT/Camera/Disable Synchronize Fog Color";

    private enum ColorChange
    {
        None,
        CameraBackground,
        SceneFog,
    }

    public static bool IsEnabled { get; set; } = true;

    private static ColorChange lastChange = ColorChange.None;

    [MenuItem(EnableSyncMenuItemPath, true)]
    private static bool EnableSyncValidate() => !IsEnabled;

    [MenuItem(EnableSyncMenuItemPath)]
    private static void EnableSync() => IsEnabled = true;

    [MenuItem(DisableSyncMenuItemPath, true)]
    private static bool DisableSyncValidate() => IsEnabled;

    [MenuItem(DisableSyncMenuItemPath)]
    private static void DisableSync() => IsEnabled = false;

    [InitializeOnLoadMethod]
    private static void Subscribe()
    {
        Undo.postprocessModifications -= OnPropertiesChanged;
        Undo.postprocessModifications += OnPropertiesChanged;

        Undo.undoRedoPerformed -= OnUndoRedo;
        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private static void OnUndoRedo()
    {
        if (!IsEnabled)
            return;

        if (Camera.main.backgroundColor == RenderSettings.fogColor)
            return;

        if (lastChange == ColorChange.CameraBackground)
        {
            Camera.main.backgroundColor = RenderSettings.fogColor;
        }
        else
        {
            RenderSettings.fogColor = Camera.main.backgroundColor;
        }
        lastChange = ColorChange.None;
    }

    private static UndoPropertyModification[] OnPropertiesChanged(UndoPropertyModification[] modifications)
    {
        SynchronizeColors(modifications);
        return modifications;
    }

    private static void SynchronizeColors(UndoPropertyModification[] modifications)
    {
        if (!IsEnabled)
            return;

        foreach (var mod in modifications)
        {
            var target = mod.currentValue.target;
            var propertyPath = mod.currentValue.propertyPath;
            if (target is RenderSettings && propertyPath.Contains("m_FogColor"))
            {
                lastChange = ColorChange.CameraBackground;
                Camera.main.backgroundColor = RenderSettings.fogColor;
                break;
            }
            else if (target == Camera.main && propertyPath.Contains("m_BackGroundColor"))
            {
                lastChange = ColorChange.SceneFog;
                RenderSettings.fogColor = Camera.main.backgroundColor;
                break;
            }
        }
    }
}
#endif