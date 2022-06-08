using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu("End Generator")]
public class EndNode : XNode.Node
{
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Noise;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public SerializableMultiArray<Color> Colors;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode unProccessedNoise;

	public SerializableMultiArray<Color> GetColors()
    {
		return GetInputValue<SerializableMultiArray<Color>>("Colors");
	}
	public Generator GetUnprocessedNoise(Vector2Int v)
	{
		return GetInputValue<AbsGeneratorNode>("unProccessedNoise").GetGenerator(v);
	}
	/// <summary>
	/// Returns the "final" generator attached to this 
	/// node's input
	/// </summary>

	public Generator GetFinalGenerator()
	{
		//Debug.Log("Getting FINAL generator");
		if (GetInputValue<AbsGeneratorNode>("Noise") == null)
			return new Constant(0);
		return GetInputValue<AbsGeneratorNode>("Noise").GetGenerator();
	}
}