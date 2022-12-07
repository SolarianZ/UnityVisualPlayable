using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitTitle("On Animation Transition Start")]
    [UnitCategory("Events/Visual Playable")]
    public class VisualAnimationTransitionStartEvent : VisualAnimationEventBase
    {
        public static readonly string EventName = nameof(VisualAnimationTransitionStartEvent);

        protected override string EventHookName => EventName;
    }
}