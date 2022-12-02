using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GBG.VisualPlayable.VisualNode
{
    [UnitCategory("Visual Playable")]
    [UnitTitle("Add Animation Layer")]
    public class NodeAnimationBrain_AddLayer : Unit
    {
        [PortLabelHidden]
        [DoNotSerialize]
        public ControlInput InputTrigger { get; private set; }

        [PortLabel("Success")]
        [DoNotSerialize]
        public ControlOutput OutputTrigger { get; private set; }

        [PortLabel("Failure")]
        [DoNotSerialize]
        public ControlOutput FailureOutputTrigger { get; private set; }

        [PortLabel("Brain")]
        [DoNotSerialize]
        public ValueInput InputBrain { get; private set; }

        [PortLabel("Layer Name")]
        [DoNotSerialize]
        public ValueInput InputLayerName { get; private set; }

        [PortLabel("Weight")]
        [DoNotSerialize]
        public ValueInput InputWeight { get; private set; }

        [PortLabel("Additive")]
        [DoNotSerialize]
        public ValueInput InputAdditive { get; private set; }

        [PortLabel("Mask")]
        [DoNotSerialize]
        public ValueInput InputMask { get; private set; }

        [PortLabel("Layer Index")]
        [DoNotSerialize]
        public ValueOutput OutputLayerIndex { get; private set; }

        [PortLabel("Layer")]
        [DoNotSerialize]
        public ValueOutput OutputLayer { get; private set; }


        protected override void Definition()
        {
            InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
            OutputTrigger = ControlOutput(nameof(OutputTrigger));
            FailureOutputTrigger = ControlOutput(nameof(FailureOutputTrigger));

            InputBrain = ValueInput<AnimationBrain>(nameof(InputBrain));
            InputLayerName = ValueInput<string>(nameof(InputLayerName));
            InputWeight = ValueInput<float>(nameof(InputWeight), 1.0f);
            InputAdditive = ValueInput<bool>(nameof(InputAdditive), false);
            InputMask = ValueInput<AvatarMask>(nameof(InputMask), null);

            OutputLayerIndex = ValueOutput<byte>(nameof(OutputLayerIndex));
            OutputLayer = ValueOutput<AnimationLayer>(nameof(OutputLayer));


            Requirement(InputBrain, InputTrigger);
            Requirement(InputLayerName, InputTrigger);

            // Specifies that the input trigger port's input exits at the output trigger port.
            // Not setting your succession also dims connected nodes, but the execution still completes.
            Succession(InputTrigger, OutputTrigger);
            Succession(InputTrigger, FailureOutputTrigger);

            // Specifies that data is written to the output value when the input trigger is triggered.
            // Assignment(InputTrigger, OutputLayerIndex);
        }

        private ControlOutput OnTriggered(Flow flow)
        {
            var brain = flow.GetValue<AnimationBrain>(InputBrain);
            var layerName = flow.GetValue<string>(InputLayerName);
            var weight = flow.GetValue<float>(InputWeight);
            var additive = flow.GetValue<bool>(InputAdditive);
            var mask = flow.GetValue<AvatarMask>(InputMask);

            var failed = false;
            byte layerIndex = 0;
            try
            {
                layerIndex = brain.AddLayer(layerName, weight, additive, mask);
            }
            catch (ArgumentNullException ane)
            {
                failed = true;
                Debug.LogException(ane);
            }
            catch (ArgumentException ae)
            {
                failed = true;
                Debug.LogException(ae);
            }
            catch (NotSupportedException nse)
            {
                failed = true;
                Debug.LogException(nse);
            }

            if (failed)
            {
                return FailureOutputTrigger;
            }

            flow.SetValue(OutputLayerIndex, layerIndex);
            flow.SetValue(OutputLayer, brain.GetLayer(layerIndex));

            return OutputTrigger;
        }
    }
}