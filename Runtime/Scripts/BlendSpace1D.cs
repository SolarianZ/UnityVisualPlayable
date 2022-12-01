using UnityEngine.Animations;

namespace GBG.VisualPlayable
{
    public class BlendSpace1D : IBlendSpace
    {
        AnimationMixerPlayable IBlendSpace.Mixer => _mixer;
   
        private AnimationMixerPlayable _mixer;
    }
}