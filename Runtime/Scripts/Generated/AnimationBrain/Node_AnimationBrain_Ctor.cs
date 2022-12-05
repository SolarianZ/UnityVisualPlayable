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
using GBG.VisualPlayable;

namespace GBG.VisualPlayable.VisualNode
{
	// GBG.VisualPlayable.AnimationBrain..ctor
[UnitCategory("Visual Playable/Animation Brain")]
[UnitTitle("Create Animation Brain")]
internal class Node_AnimationBrain_Ctor : Unit
{
	// Input trigger
[PortLabelHidden][DoNotSerialize]
public ControlInput InputTrigger { get; private set; }

	// Output trigger
[PortLabelHidden][DoNotSerialize]
public ControlOutput OutputTrigger { get; private set; }

	// Input params
	// Input parameter: animator
[PortLabel("Animator")]
[DoNotSerialize]
public ValueInput Input_animator { get; private set; }

	// Input parameter: name
[PortLabel("Name")]
[DoNotSerialize]
public ValueInput Input_name { get; private set; }

	// Output params
	// Output(return value): AnimationBrain
[PortLabel("Animation Brain")]
[DoNotSerialize]
public ValueOutput Output_AnimationBrain { get; private set; }

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_animator = ValueInput<Animator>(nameof(Input_animator));
Input_name = ValueInput<String>(nameof(Input_name));

	// Output parameters
Output_AnimationBrain=ValueOutput<AnimationBrain>(nameof(Output_AnimationBrain));

	// Requirements
Requirement(Input_animator, InputTrigger);
Requirement(Input_name, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

	// Assignments
Assignment(InputTrigger, Output_AnimationBrain);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_animator = flow.GetValue<Animator>(Input_animator);
var Value_Input_name = flow.GetValue<String>(Input_name);

	// Output parameter values
var Value_Output_AnimationBrain = new AnimationBrain(Value_Input_animator, Value_Input_name);
flow.SetValue(Output_AnimationBrain, Value_Output_AnimationBrain);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

