using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using System;

namespace Framed.PlaymakerActions {

    [ActionCategory(ActionCategory.GameLogic)]
    public abstract class AbstractCalculateDamage : FsmStateAction
    {
        [HutongGames.PlayMaker.Tooltip("The color of the paintball dealing the damage.")]
        public FsmGameObject inputPaintBall;
        [HutongGames.PlayMaker.Tooltip("Store the damage done in this float variable.")]
        [UIHint(UIHint.Variable)]
        public FsmFloat outputDamage;

        public abstract float CalculateDamage(Color color);

        public override void OnEnter()
        {
            outputDamage.Value = CalculateDamage(inputPaintBall.Value.GetComponent<Renderer>().material.color);

            Finish();
        }
    }

    public class CalculateFireDemonDamage : AbstractCalculateDamage
    {
        public override float CalculateDamage(Color color)
        {
            return 2.0f * color.r - 0.5f * color.g + color.b;
        }
    }


    public class CalculateWaterDemonDamage : AbstractCalculateDamage
    {
        public override float CalculateDamage(Color color)
        {
            return 2.0f * color.b - 0.5f * color.r + color.g;
        }
    }

    public class CalculateMotherDemonDamage : AbstractCalculateDamage
    {
        public override float CalculateDamage(Color color)
        {
            return 2.0f * color.g - 0.5f * color.b + color.r;
        }
    }
}