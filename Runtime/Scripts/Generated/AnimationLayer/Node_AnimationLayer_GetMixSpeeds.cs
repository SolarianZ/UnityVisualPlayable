// AUTO GENERATED CODE
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantUsingDirective
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using Unity.VisualScripting;

namespace GBG.VisualPlayable.VisualNode
{
	// GBG.VisualPlayable.AnimationLayer.GetMixSpeeds
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Get Mix Speeds")]
internal class Node_AnimationLayer_GetMixSpeeds : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabelHidden][DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }

	// Input params
	// Input instance
[PortLabel("Animation Layer")]
[DoNotSerialize]
public ValueInput Input_AnimationLayer { get; private set; }
	// Input parameter: speeds
[PortLabel("Speeds")]
[DoNotSerialize]
public ValueInput Input_speeds { get; private set; }

	// Output params

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));
Input_speeds = ValueInput<IList<Double>>(nameof(Input_speeds));


	// Requirements
Requirement(Input_speeds, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);
var Value_Input_speeds = flow.GetValue<IList<Double>>(Input_speeds);

Value_Input_AnimationLayer.GetMixSpeeds(Value_Input_speeds);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

