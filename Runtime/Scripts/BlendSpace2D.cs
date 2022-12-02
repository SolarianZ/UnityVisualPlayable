using UnityEngine.Animations;

namespace GBG.VisualPlayable
{
    // TODO: BlendSpace2D
    public class BlendSpace2D : IBlendSpace
    {
        AnimationMixerPlayable IBlendSpace.Mixer => _mixer;

        private AnimationMixerPlayable _mixer;


        public void SetParameterX(float value) { }

        public void SetParameterY(float value) { }

        public void SetParameters(float x, float y) { }
    }
}