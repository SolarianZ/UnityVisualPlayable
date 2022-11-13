using Unity.VisualScripting;
using UnityEngine.Playables;

// https://docs.unity3d.com/Packages/com.unity.visualscripting@1.7/manual/vs-create-own-custom-event-node.html
namespace GBG.VisualPlayable.Tests.NodeSamples
{
    [UnitTitle("On Prepare Frame")]
    [UnitCategory("Events/Visual Playable")]
    public class PrepareFrameEventNode : EventUnit<FrameData>
    {
        public static readonly string HookName = $"{nameof(VisualPlayable)}.{nameof(PrepareFrameEventNode)}";

        [PortLabel("Frame Data")]
        [DoNotSerialize]
        public ValueOutput OutputFrameData { get; private set; }

        protected override bool register => true;


        public override EventHook GetHook(GraphReference reference) => new EventHook(HookName);

        protected override void Definition()
        {
            base.Definition();

            OutputFrameData = ValueOutput<FrameData>(nameof(OutputFrameData));
        }

        protected override void AssignArguments(Flow flow, FrameData data)
        {
            flow.SetValue(OutputFrameData, data);
        }
    }
}