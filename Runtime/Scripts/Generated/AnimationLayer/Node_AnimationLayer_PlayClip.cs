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
	// GBG.VisualPlayable.AnimationLayer.PlayClip
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Play Clip")]
internal class Node_AnimationLayer_PlayClip : Unit
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

	// Input parameter: clip
[PortLabel("Clip")]
[DoNotSerialize]
public ValueInput Input_clip { get; private set; }

	// Input parameter: clipSpeed
[PortLabel("Clip Speed")]
[DoNotSerialize]
public ValueInput Input_clipSpeed { get; private set; }

	// Input parameter: clipOffsetTime
[PortLabel("Clip Offset Time")]
[DoNotSerialize]
public ValueInput Input_clipOffsetTime { get; private set; }

	// Input parameter: isFixedTime
[PortLabel("Is Fixed Time")]
[DoNotSerialize]
public ValueInput Input_isFixedTime { get; private set; }

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
Input_clip = ValueInput<AnimationClip>(nameof(Input_clip));
Input_clipSpeed = ValueInput<Single>(nameof(Input_clipSpeed), 1f);
Input_clipOffsetTime = ValueInput<Single>(nameof(Input_clipOffsetTime), 0f);
Input_isFixedTime = ValueInput<Boolean>(nameof(Input_isFixedTime), false);


	// Requirements
Requirement(Input_tag, InputTrigger);
Requirement(Input_clip, InputTrigger);

	// Successions
Succession(InputTrigger, OutputTrigger);

}

private ControlOutput OnTriggered(Flow flow)
{
try{
	// Input parameter values
var Value_Input_AnimationLayer = flow.GetValue<AnimationLayer>(Input_AnimationLayer);
var Value_Input_tag = flow.GetValue<String>(Input_tag);
var Value_Input_clip = flow.GetValue<AnimationClip>(Input_clip);
var Value_Input_clipSpeed = flow.GetValue<Single>(Input_clipSpeed);
var Value_Input_clipOffsetTime = flow.GetValue<Single>(Input_clipOffsetTime);
var Value_Input_isFixedTime = flow.GetValue<Boolean>(Input_isFixedTime);

Value_Input_AnimationLayer.PlayClip(Value_Input_tag, Value_Input_clip, Value_Input_clipSpeed, Value_Input_clipOffsetTime, Value_Input_isFixedTime);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

