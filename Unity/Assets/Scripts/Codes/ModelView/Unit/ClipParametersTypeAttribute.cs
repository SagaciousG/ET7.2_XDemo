using System;
using UnityEngine;

namespace ET.Client
{
    public class ClipParametersTypeAttribute : Attribute
    {
        public AnimatorControllerParameterType Type;

        public Vector2Int IntRange;
        public Vector2 FloatRange;
        
        public ClipParametersTypeAttribute(AnimatorControllerParameterType type)
        {
            this.Type = type;
        }

        public ClipParametersTypeAttribute(int max, int min = 0)
        {
            this.Type = AnimatorControllerParameterType.Int;
            this.IntRange = new Vector2Int(min, max);
        }

        public ClipParametersTypeAttribute(Vector2 floatRange)
        {
            this.Type = AnimatorControllerParameterType.Float;
            this.FloatRange = floatRange;
        }
    }
}