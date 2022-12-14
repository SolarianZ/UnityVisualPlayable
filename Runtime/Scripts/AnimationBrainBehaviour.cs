using GBG.VisualPlayable.Attribute;
using UnityEngine.Playables;

namespace GBG.VisualPlayable
{
    [ExcludeFromNodeGenerate]
    public class AnimationBrainBehaviour : PlayableBehaviour
    {
        private AnimationBrain _brain;


        public void Initialize(AnimationBrain brain)
        {
            _brain = brain;
        }

        public override void PrepareFrame(Playable playable, FrameData frameData)
        {
            base.PrepareFrame(playable, frameData);

            _brain.UpdateLayers(frameData.deltaTime);
        }
    }
}