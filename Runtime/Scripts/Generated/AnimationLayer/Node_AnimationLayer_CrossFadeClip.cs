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
	// GBG.VisualPlayable.AnimationLayer.CrossFadeClip
[UnitCategory("Visual Playable/Animation Layer")]
[UnitTitle("Cross Fade Clip")]
internal class Node_AnimationLayer_CrossFadeClip : Unit
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

	// Input parameter: speed
[PortLabel("Speed")]
[DoNotSerialize]
public ValueInput Input_speed { get; private set; }

	// Input parameter: fixedFadeTime
[PortLabel("Fixed Fade Time")]
[DoNotSerialize]
public ValueInput Input_fixedFadeTime { get; private set; }

	// Input parameter: fixedOffsetTime
[PortLabel("Fixed Offset Time")]
[DoNotSerialize]
public ValueInput Input_fixedOffsetTime { get; private set; }

	// Input parameter: frozeSource
[PortLabel("Froze Source")]
[DoNotSerialize]
public ValueInput Input_frozeSource { get; private set; }

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
Input_speed = ValueInput<Single>(nameof(Input_speed), 1f);
Input_fixedFadeTime = ValueInput<Single>(nameof(Input_fixedFadeTime), 0.25f);
Input_fixedOffsetTime = ValueInput<Single>(nameof(Input_fixedOffsetTime), 0f);
Input_frozeSource = ValueInput<Boolean>(nameof(Input_frozeSource), false);


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
var Value_Input_speed = flow.GetValue<Single>(Input_speed);
var Value_Input_fixedFadeTime = flow.GetValue<Single>(Input_fixedFadeTime);
var Value_Input_fixedOffsetTime = flow.GetValue<Single>(Input_fixedOffsetTime);
var Value_Input_frozeSource = flow.GetValue<Boolean>(Input_frozeSource);

Value_Input_AnimationLayer.CrossFadeClip(Value_Input_tag, Value_Input_clip, Value_Input_speed, Value_Input_fixedFadeTime, Value_Input_fixedOffsetTime, Value_Input_frozeSource);
} catch(Exception e) {
Debug.LogException(e); }

return OutputTrigger;
}

}
}

