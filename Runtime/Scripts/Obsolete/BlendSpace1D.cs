using UnityEngine.Animations;

namespace GBG.VisualPlayable
{
    // TODO: BlendSpace1D
    internal class BlendSpace1D : IBlendSpace
    {
        AnimationMixerPlayable IBlendSpace.Mixer => _mixer;

        private AnimationMixerPlayable _mixer;


        public void SetParameter(float value) { }
    }
}