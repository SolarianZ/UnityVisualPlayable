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
	// GBG.VisualPlayable.AnimationBrain.IsPlaying
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Is Playing")]
internal class Node_AnimationBrain_IsPlaying : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabelHidden][DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }

	// Input params
	// Input instance
[PortLabel("Animation Brain")]
[DoNotSerialize]
public ValueInput Input_AnimationBrain { get; private set; }

	// Output params
	// Output(return value): Boolean
[PortLabel("Boolean")]
[DoNotSerialize]
public ValueOutput Output_Boolean { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationBrain = ValueInput<AnimationBrain>(nameof(Input_AnimationBrain));

	// Output parameters
Output_Boolean=ValueOutput<Boolean>(nameof(Output_Boolean));

	// Requirements

	// Successions
Succession(InputTrigger, OutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_Boolean);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationBrain = flow.GetValue<AnimationBrain>(Input_AnimationBrain);

	// Output parameter values
var Value_Output_Boolean = Value_Input_AnimationBrain.IsPlaying();
flow.SetValue(Output_Boolean, Value_Output_Boolean);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

