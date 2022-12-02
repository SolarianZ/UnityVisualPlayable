using Unity.VisualScripting;
using UnityEngine;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitCategory("Visual Playable")]
    [UnitTitle("Create Animation Brain")]
    // [UnitSubtitle("Create Animation Brain")]
    public class NodeAnimationBrain_Create : Unit
    {
        [PortLabelHidden]
        [DoNotSerialize]
        public ControlInput InputTrigger { get; private set; }

        [PortLabelHidden]
        [DoNotSerialize]
        public ControlOutput OutputTrigger { get; private set; }

        [PortLabel("Animator")]
        [DoNotSerialize]
        public ValueInput InputAnimator { get; private set; }

        [PortLabel("Name")]
        [DoNotSerialize]
        public ValueInput InputName { get; private set; }

        [PortLabel("Brain")]
        [DoNotSerialize]
        public ValueOutput OutputBrain { get; private set; }


        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
            OutputTrigger = ControlOutput(nameof(OutputTrigger));

            InputAnimator = ValueInput<Animator>(nameof(InputAnimator));
            InputName = ValueInput<string>(nameof(InputName));

            OutputBrain = ValueOutput<AnimationBrain>(nameof(OutputBrain));

            Requirement(InputAnimator, InputTrigger);
            Requirement(InputName, InputTrigger);

            // Specifies that the input trigger port's input exits at the output trigger port.
            // Not setting your succession also dims connected nodes, but the execution still completes.
            Succession(InputTrigger, OutputTrigger);

            // Specifies that data is written to the output value when the input trigger is triggered.
            Assignment(InputTrigger, OutputBrain);
        }

        private ControlOutput OnTriggered(Flow flow)
        {
            var animator = flow.GetValue<Animator>(InputAnimator);
            var name = flow.GetValue<string>(InputName);

            var brain = new AnimationBrain(animator, name);

            flow.SetValue(OutputBrain, brain);

            return OutputTrigger;
        }
    }
}