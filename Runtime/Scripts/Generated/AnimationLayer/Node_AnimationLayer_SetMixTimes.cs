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
	// GBG.VisualPlayable.AnimationLayer.SetMixTimes
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Set Mix Times")]
internal class Node_AnimationLayer_SetMixTimes : Unit
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
	// Input parameter: times
[PortLabel("Times")]
[DoNotSerialize]
public ValueInput Input_times { get; private set; }

	// Output params

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));
Input_times = ValueInput<IReadOnlyList<Single>>(nameof(Input_times));


	// Requirements
Requirement(Input_times, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);
var Value_Input_times = flow.GetValue<IReadOnlyList<Single>>(Input_times);

Value_Input_AnimationLayer.SetMixTimes(Value_Input_times);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

