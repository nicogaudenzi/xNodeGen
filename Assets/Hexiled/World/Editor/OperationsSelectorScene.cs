using UnityEngine;
using Hexiled.World.SO;
using Hexiled.World.Events;
using Hexiled.World.Components;
namespace Hexiled.World.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(OperationsSelector))]
    public class OperationsSelectorScene : Editor
    {

        [SerializeField]
        IntSO toolBar, editorMode, terrainOps;
        [SerializeField]
        GUIArrayVariable opTips;

        [SerializeField]
        GUIArrayVariable textureTips;

        [SerializeField]
        OperationsState opState;

        [SerializeField]
        GUIArrayVariable geometryTips;
        [SerializeField]
        GUIArrayVariable terrainEditorTips;


        [SerializeField]
        BoolEventSO terrainVisibilityChanged;

        [SerializeField]
        VoidEventSO brushLoaded;
        [SerializeField]
        IntSO brushTypeSelected;
        [SerializeField]
        FloatSO brushStrength;
        [SerializeField]
        animationCurveSO brushProfile;
        [SerializeField]
        GradientSO color;
        [SerializeField]
        BrushContainer brushContainer;
        [SerializeField]
        BoolSO useColorWhilePattern;
        [SerializeField]
        BoolSO useGeometryWhilePattern;
        private void OnEnable()
        {

            if (Application.isPlaying) return;

            terrainVisibilityChanged.Event.AddListener(OnTerraiVisibilityChanged);
            brushLoaded.Event.AddListener(OnBrushLoaded);
        }

        private void OnDisable() => Clean();

        private void OnDestroy() => Clean();

        void Clean()
        {
            terrainVisibilityChanged.Event.RemoveListener(OnTerraiVisibilityChanged);
            brushLoaded.Event.RemoveListener(OnBrushLoaded);
        }
        public void OnSceneGUI()

        {

            switch (editorMode.Value)
            {
                case 0:
                    DrawTileEditorOps();
                    break;
                case 1:
                    DrawTerrainOps();
                    break;
            }

        }
        void OnBrushLoaded()
        {
            Repaint();
        }
        void OnTerraiVisibilityChanged(bool b)
        {
            Repaint();
        }

        void DrawTerrainOps()
        {
            GUILayout.BeginArea(new Rect(Screen.width * .2f, Screen.height * 0, 400, 36));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            terrainOps.Value = GUILayout.Toolbar(terrainOps.Value, terrainEditorTips.GuiContentArray());

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(Screen.width * .3f, 38, 200, 100));
            GUILayout.BeginVertical();
            brushProfile.Value = EditorGUILayout.CurveField("BrushCurve", brushProfile.Value);
            brushStrength.Value = GUILayout.HorizontalSlider(brushStrength.Value, 0, 2, GUILayout.Height(16));
            if (terrainOps.Value == 3)
            {
                color.Value = EditorGUILayout.GradientField("Color", color.Value);
            }
            if (terrainOps.Value == 4)
            {
                brushContainer.Value = (BrushGraph)EditorGUILayout.ObjectField(brushContainer.Value, typeof(BrushGraph));
                useGeometryWhilePattern.Value = EditorGUILayout.Toggle("Change Height", useGeometryWhilePattern.Value);
                useColorWhilePattern.Value = EditorGUILayout.Toggle("Change Color", useColorWhilePattern.Value);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
               
        }
        void DrawTileEditorOps()
        {
            GUILayout.BeginArea(new Rect(Screen.width * .2f, Screen.height * 0, 400, 36));
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            //EditorGUI.BeginChangeCheck();
            toolBar.Value = GUILayout.Toolbar(toolBar.Value, opTips.GuiContentArray());
            //if (EditorGUI.EndChangeCheck())
            //{
            //selectionChanged.Raise(toolBar.Value);
            //Event.current.Use();
            //}
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

            switch (toolBar.Value)
            {
                case 0:
                    GUILayout.BeginArea(new Rect(Screen.width * .2f, 40, 400, 36));
                    //GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUI.BeginChangeCheck();
                    opState.setTint = GUILayout.Toggle(opState.setTint, textureTips.GuiContentArray()[4], "Button");
                    opState.paintTopTexture = GUILayout.Toggle(opState.paintTopTexture, textureTips.GuiContentArray()[0], "Button");
                    opState.paintBottomTexture = GUILayout.Toggle(opState.paintBottomTexture, textureTips.GuiContentArray()[1], "Button");
                    opState.changeLight = GUILayout.Toggle(opState.changeLight, textureTips.GuiContentArray()[2], "Button");
                    if (opState.changeLight)
                        opState.darken = GUILayout.Toggle(opState.darken, textureTips.GuiContentArray()[2], "Button", GUILayout.Width(24), GUILayout.Height(24));
                    opState.makeWalkable = GUILayout.Toggle(opState.makeWalkable, textureTips.GuiContentArray()[3], "Button");
                    if (opState.makeWalkable)
                        opState.walkableValue = GUILayout.Toggle(opState.walkableValue, textureTips.GuiContentArray()[3], "Button", GUILayout.Width(24), GUILayout.Height(24));
                    //opState.changeRoom = GUILayout.Toggle(opState.changeRoom, textureTips.GuiContentArray()[4], "Button");
                    if (EditorGUI.EndChangeCheck())
                    {
                        //EditorUtility.SetDirty(opState);
                        // Event.current.Use();
                        //selector.selectionChanged?.Invoke();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                    break;
                case 1:
                    GUILayout.BeginArea(new Rect(Screen.width * .2f, 40, 400, 36));
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUI.BeginChangeCheck();
                    opState.changeHeight = GUILayout.Toggle(opState.changeHeight, geometryTips.GuiContentArray()[0], "Button");
                    opState.rotateUV = GUILayout.Toggle(opState.rotateUV, geometryTips.GuiContentArray()[1], "Button");

                    if (EditorGUI.EndChangeCheck())
                    {

                        // selectionChanged?.Raise(toolBar.Value);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                    break;
                case 2:
                    brushTypeSelected.Value = 0;
                    GUILayout.BeginArea(new Rect(Screen.width * .2f, 40, 400, 36));
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    EditorGUI.BeginChangeCheck();
                    opState.paintTopTexture = GUILayout.Toggle(opState.paintTopTexture, textureTips.GuiContentArray()[0], "Button");
                    opState.paintBottomTexture = GUILayout.Toggle(opState.paintBottomTexture, textureTips.GuiContentArray()[1], "Button");

                    opState.makeWalkable = GUILayout.Toggle(opState.makeWalkable, textureTips.GuiContentArray()[3], "Button");
                    if (opState.makeWalkable)
                        opState.walkableValue = GUILayout.Toggle(opState.walkableValue, textureTips.GuiContentArray()[3], "Button", GUILayout.Width(24), GUILayout.Height(24));
                    //opState.changeRoom = GUILayout.Toggle(opState.changeRoom, textureTips.GuiContentArray()[4], "Button");
                    if (EditorGUI.EndChangeCheck())
                    {
                        // selectionChanged?.Raise(toolBar.Value);
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();
                    break;

            }
        }

    }
}