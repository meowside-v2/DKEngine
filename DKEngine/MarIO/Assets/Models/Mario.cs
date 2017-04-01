using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Mario : GameObject
    {
        public enum State
        {
            Small,
            Super,
            Fire,
            Invincible
        };

        protected override void Init()
        {
            this.Name = "Player";

            Trigger left = new Trigger(this);
            left.Name = "Left_Trigger";
            left.InitNewScript<LeftMarioChecker>();

            Trigger right = new Trigger(this);
            right.Name = "Right_Trigger";
            right.InitNewScript<RightMarioChecker>();

            Trigger top = new Trigger(this);
            top.Name = "Top_Trigger";
            top.InitNewScript<TopMarioChecker>();

            Trigger bottom = new Trigger(this);
            bottom.Name = "Bottom_Trigger";
            bottom.InitNewScript<BottomMarioChecker>();

            //this.TypeName = "mario";
            this.Transform.Position = new Vector3(50, -10, 0);
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("idle", Database.GetGameObjectMaterial("mario"));
            this.Animator.AddAnimation("right_move", Database.GetGameObjectMaterial("mario_move_right"));
            this.Animator.AddAnimation("left_move", Database.GetGameObjectMaterial("mario_move_left"));
            this.Animator.AddAnimation("right_jump", Database.GetGameObjectMaterial("mario_jump_right"));
            this.Animator.AddAnimation("left_jump", Database.GetGameObjectMaterial("mario_jump_left"));
            this.InitNewScript<CharacterController>();
        }
    }
}
