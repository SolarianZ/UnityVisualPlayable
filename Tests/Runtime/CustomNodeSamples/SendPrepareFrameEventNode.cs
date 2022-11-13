using Unity.VisualScripting;
using UnityEngine.Playables;

namespace GBG.VisualPlayable.Tests.NodeSamples
{
    [UnitTitle("Send Prepare Frame Event")]
    [UnitCategory("Visual Playable")]
    public class SendPrepareFrameEventNode : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }

        [PortLabel("Frame Data")]
        [DoNotSerialize]
        public ValueInput InputFrameData;

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }


        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), Trigger);
            InputFrameData = ValueInput<FrameData>(nameof(InputFrameData));
            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            Succession(InputTrigger, OutputTrigger);
        }

        private ControlOutput Trigger(Flow flow)
        {
            EventBus.Trigger(PrepareFrameEventNode.HookName, flow.GetValue<FrameData>(InputFrameData));
            return OutputTrigger;
        }
    }
}