using GBG.VisualPlayable.Attribute;
using System;
using UnityEngine;

namespace GBG.VisualPlayable
{
    [ExcludeFromNodeGenerate]
    [Serializable]
    public struct AnimationClipInfo
    {
        public AnimationClip Clip;

        public float Speed;

        [Min(0)]
        public float Time;

        [Range(0, 1)]
        public float Weight;


        public AnimationClipInfo(AnimationClip clip, float weight, float speed = 1, float time = 0)
        {
            Clip = clip;
            Speed = speed;
            Time = time;
            Weight = weight;
        }
    }
}