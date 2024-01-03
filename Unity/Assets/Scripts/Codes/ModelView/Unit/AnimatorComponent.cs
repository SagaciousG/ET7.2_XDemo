using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	public enum ParametersType
	{
		[ClipParametersType(AnimatorControllerParameterType.Bool)]
		run,
	}
	
	public enum BattleParametersType
	{
		[ClipParametersType(10, 0)]
		toState,
		[ClipParametersType(AnimatorControllerParameterType.Bool)]
		run,
		[ClipParametersType(AnimatorControllerParameterType.Bool)]
		death,
		[ClipParametersType(AnimatorControllerParameterType.Trigger)]
		getHurt,
		[ClipParametersType(AnimatorControllerParameterType.Bool)]
		block,
		[ClipParametersType(AnimatorControllerParameterType.Trigger)]
		attack,
		[ClipParametersType(AnimatorControllerParameterType.Int)]
		skill
	}
	
	[ComponentOf()]
	public class AnimatorComponent : Entity, IAwake, IUpdate, IDestroy
	{
		public Dictionary<string, AnimationClip> animationClips = new Dictionary<string, AnimationClip>();
		public HashSet<string> Parameter = new HashSet<string>();

		public ParametersType MotionType;
		public bool isStop;
		public float stopSpeed;
		public Animator Animator;
	}
}