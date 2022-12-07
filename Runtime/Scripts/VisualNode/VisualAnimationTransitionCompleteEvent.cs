using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitTitle("On Animation Transition Complete")]
    [UnitCategory("Events/Visual Playable")]
    public class VisualAnimationTransitionCompleteEvent : VisualAnimationEventBase
    {
        public static readonly string EventName = nameof(VisualAnimationTransitionCompleteEvent);

        protected override string EventHookName => EventName;
    }
}