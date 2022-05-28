using UnityEngine;
using UnityEditor;
using Hexiled.World.Components;
using Hexiled.World.SO;

[CustomEditor(typeof(OnSceneMouseTraker))]
public class MouseTrackerEditor : Editor
{
    [SerializeField]
    Vector3SO markerPos = null;
    [SerializeField]
    IntSO currentLayer = null;
    [SerializeField]
    Vector3SO mouseHitPos = null;
    //[SerializeField]
    //Vector3Variable currentChunck = null;
    OnSceneMouseTraker traker;

    private void OnEnable()
    {
        traker = target as OnSceneMouseTraker;
        //if (markerPos == null)
        //    markerPos = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables+"MarkerPos"+".asset");
        //if (currentLayer == null)
        //    currentLayer = (IntVariable)EditorGUIUtility.LoadRequired(InternalPaths.intvariables+"CurrentLayer"+".asset");
        //if (mouseHitPos == null)
        //    mouseHitPos = (Vector3Variable)EditorGUIUtility.LoadRequired(InternalPaths.vector3variables+"MouseHitPosition"+".asset");
    }
    void OnSceneGUI()
    {
        if (Event.current.modifiers == EventModifiers.CapsLock || Event.current.modifiers == EventModifiers.Shift)
        {
            return;
        }

        if (this.UpdateHitPosition())
        {
            SceneView.RepaintAll();
        }

        this.RecalculateMarkerPosition();
        //Event current = Event.current;


    }

    private bool UpdateHitPosition()
    {
        var p = new Plane(traker.transform.TransformDirection(Vector3.up), Vector3.zero + new Vector3(0, currentLayer.Value, 0));

        var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

        var hit = new Vector3();

        float dist;

        if (p.Raycast(ray, out dist))
        {
            hit = ray.origin + (ray.direction.normalized * dist);
        }

        var value = traker.transform.InverseTransformPoint(hit);

        if (value != this.mouseHitPos.Value)
        {
            this.mouseHitPos.Value = value;
            return true;
        }
        return false;
    }
    private void RecalculateMarkerPosition()
        {
            Vector2Int tilepos = this.GetTilePositionFromMouseLocation();
            markerPos.Value = new Vector3(tilepos.x, currentLayer.Value, tilepos.y);
        }
    private Vector2Int GetTilePositionFromMouseLocation()
    {
        var pos = new Vector3(this.mouseHitPos.Value.x, currentLayer.Value, this.mouseHitPos.Value.z);
        return new Vector2Int(Mathf.FloorToInt(pos.x+.5f), Mathf.FloorToInt(pos.z + .5f));
    }
    
}
