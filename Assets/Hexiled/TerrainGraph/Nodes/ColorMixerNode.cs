using UnityEngine;
using XNode;
using Hexiled.World.Data;
[CreateNodeMenu("Modifier/Color")]
public class ColorMixerNode : Node
{
    [Input] public SerializableMultiArray<Color> Color1;
    [Input] public SerializableMultiArray<Color> Color2;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public SerializableMultiArray<Color> Colors;
	SerializableMultiArray<Color> colors;
	public Operation operation = Operation.Add;
	public enum Operation { Replace,Mix, Add, Subtract, Multiply}
	void FillColors()
	{
		var colNode1 = GetInputValue<SerializableMultiArray<Color>>("Color1");
		var colNode2 = GetInputValue<SerializableMultiArray<Color>>("Color2");
		colors = new SerializableMultiArray<Color>();
		
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				switch (operation)
				{
					case Operation.Replace:
						colors[i, 0, j] = colNode1[i, 0, j].a == 0 ? colNode2[i, 0, j] : colNode1[i, 0, j];
						break;
					case Operation.Mix:
						colors[i, 0, j] = colNode1[i, 0, j].a == 0?colNode2[i,0,j]:Color.Lerp(colNode1[i, 0, j], colNode2[i, 0, j], colNode1[i,0,j].a);
						break;
					case Operation.Add:
						colors[i, 0, j] = colNode1[i, 0, j] + colNode2[i, 0, j];
						break;
					case Operation.Subtract:
						colors[i, 0, j] = colNode1[i, 0, j] - colNode2[i, 0, j];
						break;
					case Operation.Multiply:
						colors[i, 0, j] = colNode1[i, 0, j] * colNode2[i, 0, j];
						break;
				}
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
			return colors;
		}
		
		// Hopefully this won't ever happen, but we need to return something
		// in the odd case that the port isn't "result"
		else return null;
	}
}
