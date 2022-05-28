using UnityEngine;
using UnityEditor;
using Hexiled.World.SO;
//using UnityAtoms.BaseAtoms;
namespace Hexiled.World.Editor
{
    [CustomEditor(typeof(WorldData))]
    public class WorldDataEditor : UnityEditor.Editor
    {
        public WorldEventSystem worldEventSystem;
        public WorldDataContainer wdc;
        //VoidEvent askRepaint,
        //    worldDataChanged,
        //    updateMaterialList,
        //    repaintTileEditor,
        //    repaintPaletteEditor,
        //    meshRotate;
        Texture2D icon;
        WorldData w;
        private void OnEnable()
        {
            //if (askRepaint == null)
            //    askRepaint = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "askedForTileRepaint" + ".asset");
            //if (worldDataChanged == null)
            //    worldDataChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "worldDataChanged" + ".asset");
            //if (updateMaterialList == null)
            //    updateMaterialList = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "UpdateMaterialList" + ".asset");
            //if (repaintTileEditor == null)
            //    repaintTileEditor = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "repaintTileEditor" + ".asset");
            //if (repaintPaletteEditor == null)
            //    repaintPaletteEditor = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "repaintPaletteEditor" + ".asset");
            //if (icon == null)
            //    icon = (Texture2D)EditorGUIUtility.LoadRequired("Icons/WorldDataIcon.png");
            //if (meshRotate == null)
            //    meshRotate = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "RotateMesh" + ".asset");

            w = (WorldData)target;

        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

          
            w.PreviewIcon = icon;
            if (GUILayout.Button("Load")) {
                if (EditorUtility.DisplayDialog("Continue Loading World Data?",
                    "You can't Crtl-z your way back from this one, but you can always load back the previous data.",
                    "Yes",
                    "No"))
                {

                    Load();
                    worldEventSystem.askRepaint.Invoke();
                    worldEventSystem.worldDataChanged.Invoke();
                    worldEventSystem.updateMaterialList.Invoke();
                    worldEventSystem.repaintTileEditor.Invoke();
                    worldEventSystem.repaintPaletteEditor.Invoke();
                    worldEventSystem.meshRotate.Invoke();
                }
                
            }
        }
        public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
        {
            WorldData example = (WorldData)target;

            if (example == null || example.PreviewIcon == null)
                return null;

            Texture2D tex = new Texture2D(width, height);
            EditorUtility.CopySerialized(example.PreviewIcon, tex);

            return tex;
        }
        void Load()
        {
            var path = AssetDatabase.GetAssetPath(target);
            //WorldDataContainer wdc = AssetDatabase.LoadAssetAtPath<WorldDataContainer>("Assets/Editor Default Resources/" + InternalPaths.stateObjects + "World Data Container" + ".asset");
            Object[] data = AssetDatabase.LoadAllAssetsAtPath(path);
            Undo.RegisterCompleteObjectUndo(wdc, "World Data Changed");

            foreach (Object o in data)
            {

                if (o is WorldData)
                {
                    WorldData wd = o as WorldData;
                    wdc.WorldData = wd;
                }
            }
            foreach (Object o in data)
            {

                if (o is TerrainSettings) {
                    TerrainSettings terrainSettings = o as TerrainSettings;
                    wdc.WorldData.TerrainSettings = terrainSettings;
                    }
                if (o is AtlasCollection)
                {
                    AtlasCollection worldTilemaps = o as AtlasCollection;
                    wdc.WorldData.WorldTilemaps = worldTilemaps;
                }
                if (o is WorldMeshes)
                {
                    WorldMeshes worldMeshes = o as WorldMeshes;
                    wdc.WorldData.WorldMeshes = worldMeshes;
                }
                if (o is MapPrefabsContainer)
                {
                    MapPrefabsContainer mapPrefabs = o as MapPrefabsContainer;
                    wdc.WorldData.MapPrefabsContainer = mapPrefabs;
                }
                EditorUtility.SetDirty(wdc);
            }
        }
    }
}