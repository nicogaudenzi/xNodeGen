using System;
using XNode;
using UnityEngine;
using Hexiled.World.Data;
[CreateNodeMenu(MENU_PARENT_NAME + "Naive Interpolator")]
public class NaiveInterpolationNode : AbsTwoModNode
{
    public override Generator GetGenerator()
    {
        if (!HasBothGenerators())
        {
            return null;
        }

        var g1 = GetGenerator1();
        var g2 = GetGenerator2();
                  
        return (g1 + g2)/2; 
    }
    public override string GetTitle()
    {
        return "Naive Interpolation";
    }
}