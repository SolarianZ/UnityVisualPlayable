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
	// GBG.VisualPlayable.AnimationLayer.PlayPlayable
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Play Playable")]
internal class Node_AnimationLayer_PlayPlayable : Unit
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
	// Input parameter: tag
[PortLabel("Tag")]
[DoNotSerialize]
public ValueInput Input_tag { get; private set; }

	// Input parameter: playable
[PortLabel("Playable")]
[DoNotSerialize]
public ValueInput Input_playable { get; private set; }

	// Input parameter: speed
[PortLabel("Speed")]
[DoNotSerialize]
public ValueInput Input_speed { get; private set; }

	// Input parameter: fixedOffsetTime
[PortLabel("Fixed Offset Time")]
[DoNotSerialize]
public ValueInput Input_fixedOffsetTime { get; private set; }

	// Output params

protected override void Definition()
{
	// InputTrigger
InputTrigger = ControlInput(nameof(InputTrigger), OnTriggered);
	// OutputTrigger
OutputTrigger = ControlOutput(nameof(OutputTrigger));

	// Input parameters
Input_AnimationLayer = ValueInput<AnimationLayer>(nameof(Input_AnimationLayer));
Input_tag = ValueInput<String>(nameof(Input_tag));
Input_playable = ValueInput<Playable>(nameof(Input_playable));
Input_speed = ValueInput<Single>(nameof(Input_speed), 1f);
Input_fixedOffsetTime = ValueInput<Single>(nameof(Input_fixedOffsetTime), 0f);


	// Requirements
Requirement(Input_tag, InputTrigger);
Requirement(Input_playable, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);
var Value_Input_tag = flow.GetValue<String>(Input_tag);
var Value_Input_playable = flow.GetValue<Playable>(Input_playable);
var Value_Input_speed = flow.GetValue<Single>(Input_speed);
var Value_Input_fixedOffsetTime = flow.GetValue<Single>(Input_fixedOffsetTime);

Value_Input_AnimationLayer.PlayPlayable(Value_Input_tag, Value_Input_playable, Value_Input_speed, Value_Input_fixedOffsetTime);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

