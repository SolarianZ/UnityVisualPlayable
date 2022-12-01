using UnityEngine.Animations;

namespace GBG.VisualPlayable
{
    public interface IBlendSpace
    {
        internal AnimationMixerPlayable Mixer { get; }
    }
}