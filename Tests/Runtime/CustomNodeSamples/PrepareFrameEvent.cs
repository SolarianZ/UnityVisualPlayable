using Unity.VisualScripting;
using UnityEngine.Playables;

// https://docs.unity3d.com/Packages/com.unity.visualscripting@1.7/manual/vs-create-own-custom-event-node.html
namespace GBG.VisualPlayable.Tests.NodeSamples
{
    public readonly struct PrepareFrameArgs
    {
        public readonly Playable Playable;

        public readonly FrameData FrameData;


        public PrepareFrameArgs(Playable playable, FrameData frameData)
        {
            Playable = playable;
            FrameData = frameData;
        }
    }

    [UnitCategory("Events/Visual Playable")]
    [UnitTitle("On Prepare Frame")]
    // [UnitSubtitle("On Prepare Frame")]
    public class PrepareFrameEvent : EventUnit<PrepareFrameArgs>
    {
        public static readonly string HookName = $"{nameof(VisualPlayable)}.{nameof(PrepareFrameEvent)}";

        [PortLabel("Playable")]
        [DoNotSerialize]
        public ValueOutput OutputPlayable { get; private set; }

        [PortLabel("Frame Data")]
        [DoNotSerialize]
        public ValueOutput OutputFrameData { get; private set; }

        protected override bool register => true;


        public override EventHook GetHook(GraphReference reference) => new EventHook(HookName);

        protected override void Definition()
        {
            base.Definition();

            OutputPlayable = ValueOutput<Playable>(nameof(OutputPlayable));
            OutputFrameData = ValueOutput<FrameData>(nameof(OutputFrameData));
        }

        protected override void AssignArguments(Flow flow, PrepareFrameArgs args)
        {
            flow.SetValue(OutputPlayable, args.Playable);
            flow.SetValue(OutputFrameData, args.FrameData);
        }
    }
}