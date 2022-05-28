using UnityEngine;
using XNode;
using Hexiled.World.Data;

[CreateNodeMenu(AbsTwoModNode.MENU_PARENT_NAME + "Mask")]
public class MaskNode : AbsGeneratorNode
{
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Generator1;
	[Input] public SerializableMultiArray<Color> Color1;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Generator2;
	[Input] public SerializableMultiArray<Color> Color2;

	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Mask;
	[Output(ShowBackingValue.Never, ConnectionType.Override)] public SerializableMultiArray<Color> Colors;

	SerializableMultiArray<Color> colors;

	public override Generator GetGenerator()
	{
		var genNode1 = GetInputValue<AbsGeneratorNode>("Generator1");
		var genNode2 = GetInputValue<AbsGeneratorNode>("Generator2");

		var blendNode = GetInputValue<AbsGeneratorNode>("Mask");

		if (!HasAllGenerators(genNode1, genNode2, blendNode))
		{
			return null;
		}

		var g1 = genNode1.GetGenerator();
		var g2 = genNode2.GetGenerator();
		var mask = blendNode.GetGenerator();

		return new Blend(g1, g2, mask);
	}
	void FillColors()
    {
		var colNode1 = GetInputValue<SerializableMultiArray<Color>>("Color1");
		var colNode2 = GetInputValue<SerializableMultiArray<Color>>("Color2");
		var blendNode = GetInputValue<AbsGeneratorNode>("Mask").GetGenerator();
		colors = new SerializableMultiArray<Color>();
		for (int i = 0; i < 32; i++)
		{
			for (int j = 0; j < 32; j++)
			{
				var w = Mathf.Clamp01(blendNode.GetValue(i, 0, j));
				//return m_A.GetValue(x, y, z) * (1 - w) + m_B.GetValue(x, y, z) * w;
				colors[i, 0, j] = Color.Lerp(colNode1[i, 0, j], colNode2[i, 0, j], w);
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
		else if (port.fieldName == "Output")
		{
			return this;
		}
		// Hopefully this won't ever happen, but we need to return something
		// in the odd case that the port isn't "result"
		else return null;
	}
	public override string GetTitle()
	{
		return "Mask";
	}
}