using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitTitle("On Animation Clip Enter")]
    [UnitCategory("Events/Visual Playable")]
    public class VisualAnimationClipEnterEvent : VisualAnimationEventBase
    {
        public static readonly string EventName = nameof(VisualAnimationClipEnterEvent);

        protected override string EventHookName => EventName;
    }
}