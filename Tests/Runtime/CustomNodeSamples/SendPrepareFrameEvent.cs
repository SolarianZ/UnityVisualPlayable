using Unity.VisualScripting;
using UnityEngine.Playables;

namespace GBG.VisualPlayable.Tests.NodeSamples
{
    [UnitCategory("Visual Playable")]
    [UnitTitle("Send Prepare Frame Event")]
    // [UnitSubtitle("Send Prepare Frame Event")]
    public class SendPrepareFrameEvent : Unit
    {
        [DoNotSerialize]
        [PortLabelHidden]
        public ControlInput InputTrigger { get; private set; }

        [DoNotSerialize]
        [PortLabelHidden]
        public ControlOutput OutputTrigger { get; private set; }

        [PortLabel("Playable")]
        [DoNotSerialize]
        public ValueInput InputPlayable;

        [PortLabel("Frame Data")]
        [DoNotSerialize]
        public ValueInput InputFrameData;


        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            InputPlayable = ValueInput<Playable>(nameof(InputPlayable));
            InputFrameData = ValueInput<FrameData>(nameof(InputFrameData));

            Requirement(InputPlayable, InputTrigger);
            Requirement(InputFrameData, InputTrigger);

            // Specifies that the input trigger port's input exits at the output trigger port.
            // Not setting your succession also dims connected nodes, but the execution still completes.
            Succession(InputTrigger, OutputTrigger);

            // Specifies that data is written to the output value when the inputTrigger is triggered.
            // Assignment(InputTrigger, OutputValue);
        }

        private ControlOutput OnTriggered(Flow flow)
        {
            var prepareFrameArgs = new PrepareFrameArgs(
                flow.GetValue<Playable>(InputPlayable),
                flow.GetValue<FrameData>(InputFrameData)
            );
            EventBus.Trigger(PrepareFrameEvent.HookName, prepareFrameArgs);
            return OutputTrigger;
        }
    }
}