using UnityEngine;
using System.Collections.Generic;

namespace Hexiled.World.SO
{
    [CreateAssetMenu(fileName = "GUIArrayVariable", menuName = "Hexiled/Editor Specific/GUIArrayVariable")]

    public class GUIArrayVariable : ArrayVariable<IconContent>
    {
        public GUIContent[] GuiContentArray()
        {
            List<GUIContent> list = new List<GUIContent>();
            for (int i = 0; i < Value.Length; i++)
            {
                list.Add(Value[i].guiElement());
            }
            return list.ToArray();
        }
    }
    [System.Serializable]
    public class IconContent
    {
        public Texture Texture;
        public string text;
        public GUIContent guiElement()
        {
            return new GUIContent(Texture as Texture, text);
        }
    }
}
