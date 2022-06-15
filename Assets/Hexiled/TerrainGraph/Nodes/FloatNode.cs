using XNode;

public class FloatNode:AbsGeneratorNode
{
    //[Output] public AbsGeneratorNode Output;
    public float value;
    public override Generator GetGenerator()
    {
        return new Constant(value);
    }
    public override string GetTitle()
    {
        return "Container";
    }
    
}
