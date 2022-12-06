using UnityEngine.Animations;

namespace GBG.VisualPlayable
{
    internal interface IBlendSpace
    {
        internal AnimationMixerPlayable Mixer { get; }
    }
}