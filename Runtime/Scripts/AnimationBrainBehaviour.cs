using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    public class AnimationBrainBehaviour : PlayableBehaviour
    {
        private AnimationBrain _brain;


        public void Initialize(AnimationBrain brain)
        {
            _brain = brain;
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);

            _brain.UpdateLayers(info.deltaTime);
        }
    }
}