using UnityEngine;

namespace GBG.VisualPlayable
{
    public readonly struct AnimationClipInfo
    {
        public readonly AnimationClip Clip;

        public readonly float Speed;

        public readonly float Time;

        public readonly float Weight;


        public AnimationClipInfo(AnimationClip clip, float weight, float speed = 1, float time = 0)
        {
            Clip = clip;
            Speed = speed;
            Time = time;
            Weight = weight;
        }
    }
}