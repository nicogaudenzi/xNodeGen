using XNode;
using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu("Modifier/TerrainPostprocessing")]
public class TerrainPostprocessingNode:AbsGeneratorNode
{
    [Input(ShowBackingValue.Never, ConnectionType.Override)] public AbsGeneratorNode Generator;
    [Output] public SerializableMultiArray<Color> Colors;

    public Gradient gradient;
    public float heightMultiplier = 1;
    public AnimationCurve heightCurve;
    SerializableMultiArray<Color> colors;
    TerrainPostProssesingGenerator terrainPostProssesingGenerator;
    public override Generator GetGenerator()
    {
        //Debug.Log("Getting Terrain Post Processing");
        var genNode1 = GetInputValue<AbsGeneratorNode>("Generator");
        //if(terrainPostProssesingGenerator==null)
            terrainPostProssesingGenerator = new TerrainPostProssesingGenerator(gradient, heightMultiplier, heightCurve, genNode1.GetGenerator());
        return terrainPostProssesingGenerator;
    }

    public override string GetTitle()
    {
        return "Noise PostProcessing";
    }
    public void FillColors()
    {
        var genNode1 = GetInputValue<AbsGeneratorNode>("Generator").GetGenerator();
        colors = new SerializableMultiArray<Color>();
        for (int i = 0; i < 32; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                colors[i, 0, j] = gradient.Evaluate(genNode1.GetValue(i, 0, j));
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
        else if (port.fieldName == "Output")
        {
            return this;
        }
        // Hopefully this won't ever happen, but we need to return something
        // in the odd case that the port isn't "result"
        else return null;
    }
}
