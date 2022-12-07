using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitTitle("On Animation Clip Exit")]
    [UnitCategory("Events/Visual Playable")]
    public class VisualAnimationClipExitEvent : VisualAnimationEventBase
    {
        public static readonly string EventName = nameof(VisualAnimationClipExitEvent);

        protected override string EventHookName => EventName;
    }
}