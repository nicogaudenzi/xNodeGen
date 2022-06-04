using UnityEngine;
using UnityEditor;
namespace Hexiled.World.SO { 
    public abstract class SOBase<T>:ScriptableObject
    {
        [SerializeField]
        private T _value;
        public T Value {
            get {

                return _value;
            }
            set
            {


                _value = value;
//#if UNITY_EDITOR
//                EditorUtility.SetDirty(this);
//#endif
            }
        }
        #if UNITY_EDITOR
                private void OnValidate()
                {
                    EditorUtility.SetDirty(this);
                }
        #endif
    }
}