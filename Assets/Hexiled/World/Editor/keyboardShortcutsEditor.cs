using UnityEngine;
using UnityEditor;
using Hexiled.World.SO;
using Hexiled.World.Components;
using Hexiled.World.Events;

[CustomEditor(typeof(keyboardShortcuts))]
public class keyboardShortcutsEditor : Editor
{
    [SerializeField]
    FillConditionsCheck fcc;
    [SerializeField]
    IntSO brushType, toolbar, brushSize, shadeBar, terrainOps;
    [SerializeField]
    VoidEventSO brushTypeChanged, opTypeChanged, shadeBarNeedsRepaint, shadeBarChanged;
    [SerializeField]
    BoolSO useCircleVoxelBrush;
    [SerializeField]
    OperationsState opState;
    private void OnEnable()
    {

        if (Application.isPlaying) return;
        SceneView.duringSceneGui += DuringScene;
    }
    private void OnDisable() => Clean();

    private void OnDestroy() => Clean();

    void Clean()
    {
        SceneView.duringSceneGui -= DuringScene;
    }
    void DuringScene(SceneView sceneView)
    {
        Event current = Event.current;
        if (current.type == EventType.KeyDown)
            switch (current.keyCode)
            {
                case KeyCode.Alpha1:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        fcc.Checktop = !fcc.Checktop;
                    }
                    else
                    {
                        brushType.Value = 0;
                    }
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha2:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        fcc.CheckBottom = !fcc.CheckBottom;
                    }
                    else
                    {
                        brushType.Value = 1;
                    }
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha3:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        fcc.CheckHeight = !fcc.CheckHeight;
                    }
                    else
                    {
                        brushType.Value = 2;
                    }
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha4:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        fcc.CheckRotation = !fcc.CheckRotation;
                    }
                    else
                    {
                        brushType.Value = 3;
                    }
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha5:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        fcc.CheckWalkable = !fcc.CheckWalkable;
                    }
                    else
                    {
                        brushType.Value = 4;
                    }
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha6:
                    //if (current.modifiers == EventModifiers.Control)
                    //{
                    //    fcc.CheckWalkable = !fcc.CheckWalkable;
                    //}
                    //else
                    //{
                    brushType.Value = 5;
                    //}
                    brushTypeChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.B:
                    toolbar.Value = 0;
                    terrainOps.Value = 0;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.C:
                    useCircleVoxelBrush.Value = !useCircleVoxelBrush.Value;
                    brushTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.G:
                    toolbar.Value = 1;
                    terrainOps.Value = 1;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.M:
                    toolbar.Value = 2;
                    terrainOps.Value = 2;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.L:
                    toolbar.Value = 3;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.Plus:
                    brushSize.Value++;
                    brushTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.Minus:

                    brushSize.Value = brushSize.Value <= 1 ? 1 : brushSize.Value - 1;
                    brushTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.Alpha9:

                    shadeBar.Value = 0;
                    shadeBarChanged.Event.Invoke();

                    current.Use();
                    break;
                case KeyCode.Alpha0:

                    shadeBar.Value = 1;
                    shadeBarChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.W:
                    if (current.modifiers == EventModifiers.Control)
                        opState.walkableValue = !opState.walkableValue;
                    else
                        opState.makeWalkable = !opState.makeWalkable;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.D:
                    if (current.modifiers == EventModifiers.Control)
                    {
                        opState.darken = !opState.darken;
                    }
                    else
                    {
                        opState.changeLight = !opState.changeLight;
                    }
                    current.Use();
                    break;
                case KeyCode.T:

                    opState.setTint = !opState.setTint;
                    current.Use();
                    break;
                case KeyCode.UpArrow:

                    opState.paintTopTexture = !opState.paintTopTexture;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.DownArrow:

                    opState.paintBottomTexture = !opState.paintBottomTexture;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.RightArrow:

                    opState.rotateUV = !opState.rotateUV;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
                case KeyCode.LeftArrow:

                    opState.changeHeight = !opState.changeHeight;
                    opTypeChanged.Event.Invoke();
                    current.Use();
                    break;
            }
    }
}
