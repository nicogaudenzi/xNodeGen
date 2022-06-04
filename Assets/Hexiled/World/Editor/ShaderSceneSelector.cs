using Hexiled.World.Components;
using UnityEditor;
using UnityEngine;
using Hexiled.World.SO;
using Hexiled.World.Events;
namespace Hexiled.World.Editor
{

    [CustomEditor(typeof(ShaderSelector))]
    public class ShaderSceneSelector : UnityEditor.Editor
    {
        [SerializeField]
        WorldDataContainer wdc;

        [SerializeField]
        IntSO shadebar;

        [SerializeField]
        GUIArrayVariable shadeTips;

        [SerializeField]
        GUIArrayVariable terrainVisibility;


        [SerializeField]
        BoolSO terrainVisible;

        [SerializeField]
        Vector3EventSO chunckChanged;
        [SerializeField]
        VoidEventSO shadeBarChanged, shadeBarNeedsRepaint;

        //Vector2Int currentChunk;

        private void OnEnable()
        {

            //if (wdc == null)
            //    wdc = (WorldDataContainer)EditorGUIUtility.LoadRequired(InternalPaths.stateObjects + "World Data Container" + ".asset");
            //if (shadebar == null)
            //    shadebar = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables + "ShaderBar" + ".asset");
            //if (shadeTips == null)
            //    shadeTips = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons + "Shader GUI Array Variable" + ".asset");
            //if (terrainVisibility == null)
            //    terrainVisibility = (GUIArrayVariable)EditorGUIUtility.LoadRequired(InternalPaths.icons + "TerrainVisibility" + ".asset");
            //if (terrainVisible == null)
            //    terrainVisible = (BoolVariable)EditorGUIUtility.LoadRequired(InternalPaths.boolvariables + "TerrainVisible" + ".asset");
            //if (chunckChanged == null)
            //    chunckChanged = (Vector3Event)EditorGUIUtility.LoadRequired(InternalPaths.vector3events + "ChunkChanged" + ".asset");
            //if (shadeBarChanged == null)
            //    shadeBarChanged = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "ShadeBarChanged" + ".asset");
            //if (shadeBarNeedsRepaint == null)
            //    shadeBarNeedsRepaint = (VoidEvent)EditorGUIUtility.LoadRequired(InternalPaths.voidevents + "shadeBarNeedsRepaint" + ".asset");
            if (Application.isPlaying) return;
            //SceneView.duringSceneGui += OnScene;
            chunckChanged.Event.AddListener(OnChunckChanged);
            shadeBarNeedsRepaint.Event.AddListener(Repaint);
        }
        private void OnDisable() => Clean();

        private void OnDestroy() => Clean();

        void Clean()
        {
            if (Application.isPlaying) return;
            //SceneView.duringSceneGui -= OnScene;
            chunckChanged.Event.RemoveListener(OnChunckChanged);
            shadeBarNeedsRepaint.Event.RemoveListener(Repaint);

        }
        void OnChunckChanged(Vector3 v)
        {
            //currentChunk = v.Fomchunck3ToChunck2Int();
            Repaint();
        }

        //public void OnScene(SceneView sceneView)
        public void OnSceneGUI()

        {
            GUILayout.BeginArea(new Rect(Screen.width * .75f, Screen.height * 0f, 90, 36));
            GUILayout.BeginHorizontal();
            GUIStyle s = new GUIStyle
            {
                fixedHeight = 32
            };
            EditorGUI.BeginChangeCheck();

            shadebar.Value = GUILayout.Toolbar(shadebar.Value, shadeTips.GuiContentArray());
            if (EditorGUI.EndChangeCheck())
            {
                shadeBarChanged.Event.Invoke();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
            GUILayout.BeginArea(new Rect(Screen.width * .75f, 40, 36, 36));
            terrainVisible.Value = GUILayout.Toggle(terrainVisible.Value, terrainVisibility.GuiContentArray()[0], "Button");

            GUILayout.EndArea();
            
        }

    }
}
