using UnityEngine;
using HutongGames.PlayMaker;
using DG.Tweening;
using Doozy.PlaymakerActions;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DOTween")]
    [Tooltip("Pauses all tweens with the given ID")]
    [HelpUrl("http://dotween.demigiant.com/documentation.php")]
    public class DOTweenControlMethodsPauseById : FsmStateAction
    {
        [Tooltip("Select the tween ID type to use")]
        public DOTweenActionsEnums.TweenId tweenIdType;

        [UIHint(UIHint.FsmString)]
        [Tooltip("Use a String as the tween ID")]
        public FsmString stringAsId;

        [UIHint(UIHint.Tag)]
        [Tooltip("Use a Tag as the tween ID")]
        public FsmString tagAsId;

        [UIHint(UIHint.FsmGameObject)]
        [Tooltip("Use a GameObject as the tween ID")]
        public FsmGameObject gameObjectAsId;

        [ActionSection("Debug Options")]
        [UIHint(UIHint.FsmBool)]
        public FsmBool debugThis;

        public override void Reset()
        {
            base.Reset();

            stringAsId = new FsmString { UseVariable = false };
            tagAsId = new FsmString { UseVariable = false };
            gameObjectAsId = new FsmGameObject { UseVariable = false, Value = null };

            debugThis = new FsmBool { Value = false };
        }

        public override void OnEnter()
        {
            int numberOfTweensPaused = 0;

            switch (tweenIdType)
            {
                case DOTweenActionsEnums.TweenId.UseString: if (string.IsNullOrEmpty(stringAsId.Value) == false) numberOfTweensPaused = DOTween.Pause(stringAsId.Value); break;
                case DOTweenActionsEnums.TweenId.UseTag: if (string.IsNullOrEmpty(tagAsId.Value) == false) numberOfTweensPaused = DOTween.Pause(tagAsId.Value); break;
                case DOTweenActionsEnums.TweenId.UseGameObject: if(gameObjectAsId.Value != null) numberOfTweensPaused = DOTween.Pause(gameObjectAsId.Value); break;
            }

            if (debugThis.Value) Debug.Log("GameObject [" + State.Fsm.GameObjectName + "] FSM [" + State.Fsm.Name + "]  State [" + State.Name + "] - DOTween Control Methods Pause By Id - SUCCESS! - Paused " + numberOfTweensPaused + " tweens");

            Finish();
        }


    }
}
