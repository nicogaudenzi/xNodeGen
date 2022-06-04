using XNode;
using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu("Source/Texture")]
public class TextureNode: Node
{
    public Texture2D texture;

    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Vector2Int Offset;

    [Output]public SerializableMultiArray<Color> Colors;
    SerializableMultiArray<Color> colors;

    public void FillColors()
    {
        Vector2Int offset = GetInputValue<Vector2Int>("Offset") / 32;

        colors = new SerializableMultiArray<Color>();
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                Color c = texture.GetPixel(i+offset.x*32, j+offset.y*32);
                colors[i, 0, j] = c;
            }
        }
    }
    public override object GetValue(NodePort port)
    {

        // Check which output is being requested. 
        // In this node, there aren't any other outputs than "result".
        if (port.fieldName == "Colors")
        {
            FillColors();
            // Return input value + 1
            return colors;
        }
       
        else return null;
    }
}
