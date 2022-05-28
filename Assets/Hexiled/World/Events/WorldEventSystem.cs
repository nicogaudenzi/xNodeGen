using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="WorldEventSystem",menuName ="Hexiled/Yeti3D/Events/WorldEventsSystem")]

public class WorldEventSystem: ScriptableObject
{
    public UnityEvent meshesChanged;
    public UnityEvent worldDataChanged;
    public UnityEvent repaintTileEditor;
    public UnityEvent repaintPaletteEditor;
    public UnityEvent meshRotate;
    public UnityEvent askRepaint;
    public UnityEvent updateMaterialList;
    public UnityEvent TerrainSettingsChanged;

    public UnityEvent<bool> AutoUpdateChanged;

    private void OnEnable()
    {
        if(meshesChanged==null)
            meshesChanged = new UnityEvent();
        if(worldDataChanged==null)
            worldDataChanged = new UnityEvent();
        if(repaintTileEditor==null)
            repaintTileEditor = new UnityEvent();
        if(repaintPaletteEditor==null)
            repaintPaletteEditor = new UnityEvent();
        if(meshRotate==null)
            meshRotate = new UnityEvent();
        if (askRepaint == null)
            askRepaint = new UnityEvent();
        if (updateMaterialList == null)
            updateMaterialList = new UnityEvent();

    }
}
