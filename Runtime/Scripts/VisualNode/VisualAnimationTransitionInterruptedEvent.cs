using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitTitle("On Animation Transition Interrupted")]
    [UnitCategory("Events/Visual Playable")]
    public class VisualAnimationTransitionInterruptedEvent : VisualAnimationEventBase
    {
        public static readonly string EventName = nameof(VisualAnimationTransitionInterruptedEvent);

        protected override string EventHookName => EventName;
    }
}