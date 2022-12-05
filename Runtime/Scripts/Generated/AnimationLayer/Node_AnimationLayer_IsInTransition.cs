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
	// GBG.VisualPlayable.AnimationLayer.IsInTransition
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Is In Transition")]
internal class Node_AnimationLayer_IsInTransition : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabel("True")]
[DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }
[PortLabel("False")]
[DoNotSerialize]
public ControlOutput FailureOutputTrigger { get; private set; }

	// Input params
	// Input instance
[PortLabel("Animation Layer")]
[DoNotSerialize]
public ValueInput Input_AnimationLayer { get; private set; }

	// Output params
	// Output(return value): Boolean
[PortLabel("In Transition")]
[DoNotSerialize]
public ValueOutput Output_Boolean { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));
FailureOutputTrigger = ControlOutput(nameof(FailureOutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));

	// Output parameters
Output_Boolean=ValueOutput<Boolean>(nameof(Output_Boolean));

	// Requirements

	// Successions
Succession(InputTrigger, OutputTrigger);
Succession(InputTrigger, FailureOutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_Boolean);

}

private ControlOutput OnTriggered(Flow flow)
{
var failed = false;
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);

	// Output parameter values
var Value_Output_Boolean = Value_Input_AnimationLayer.IsInTransition();
failed = !Value_Output_Boolean;
flow.SetValue(Output_Boolean, Value_Output_Boolean);
} catch(Exception e) {
Debug.LogException(e); }

if(failed) { return FailureOutputTrigger; }
return OutputTrigger;
}

}
}

