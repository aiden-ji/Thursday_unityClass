// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
    [ActionTarget(typeof(PlayMakerFSM), "gameObject,fsmName")]
	[Tooltip("Get the value of a Float Variable from another FSM.")]
	public class GetFsmFloat : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
        public FsmOwnerDefault gameObject;
		
        [UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;
		
        [RequiredField]
		[UIHint(UIHint.FsmFloat)]
        [Tooltip("The name of the FSM variable to get.")]
        public FsmString variableName;
		
        [RequiredField]
		[UIHint(UIHint.Variable)]
        [Tooltip("Store the value in a Float variable in this FSM.")]
		public FsmFloat storeValue;
		
        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

        private GameObject goLastFrame;
        private string fsmNameLastFrame;
        private PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			storeValue = null;
		}

		public override void OnEnter()
		{
			DoGetFsmFloat();
			
			if (!everyFrame)
				Finish();
		}
		
		public override void OnUpdate()
		{
			DoGetFsmFloat();
		}

        private void DoGetFsmFloat()
		{
			if (storeValue.IsNone) return;

			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null) return;

            if (go != goLastFrame || fsmName.Value != fsmNameLastFrame)
            {
                goLastFrame = go;
                fsmNameLastFrame = fsmName.Value;
                // only get the fsm component if go or fsm name has changed
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}
			
			if (fsm == null) return;
			
			FsmFloat fsmFloat = fsm.FsmVariables.GetFsmFloat(variableName.Value);
			
			if (fsmFloat == null) return;
			
			storeValue.Value = fsmFloat.Value;
		}

#if UNITY_EDITOR

        public override string AutoName()
        {
            return ActionHelpers.AutoName(this, variableName);
        }

#endif
    }
}