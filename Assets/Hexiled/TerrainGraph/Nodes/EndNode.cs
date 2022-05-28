using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu("End Generator")]
public class EndNode : XNode.Node
{
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Noise;
	[Input(ShowBackingValue.Never, ConnectionType.Override)] public SerializableMultiArray<Color> Colors;

	public SerializableMultiArray<Color> GetColors()
    {
		return GetInputValue<SerializableMultiArray<Color>>("Colors");
	}
	/// <summary>
	/// Returns the "final" generator attached to this 
	/// node's input
	/// </summary>
    
	public Generator GetFinalGenerator()
	{
		//Debug.Log("Getting FINAL generator");
		return GetInputValue<AbsGeneratorNode>("Noise").GetGenerator();
	}
}