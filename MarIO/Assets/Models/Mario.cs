using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            left.Transform.Position += new Vector3(-1, 0, 0);
            left.Transform.Dimensions = new Vector3(1, 16, 0);
            left.Name = "Left_Trigger";
            left.InitNewScript<LeftMarioChecker>();
            left.Model = new Material(Color.Black, left);

            Trigger right = new Trigger(this);
            right.Transform.Position += new Vector3(32, 0, 0);
            right.Transform.Dimensions = new Vector3(1, 16, 0);
            right.Name = "Right_Trigger";
            right.InitNewScript<RightMarioChecker>();
            right.Model = new Material(Color.Black, right);

            Trigger top = new Trigger(this);
            top.Transform.Position += new Vector3(0, -1, 0);
            top.Transform.Dimensions = new Vector3(16, 1, 0);
            top.Name = "Top_Trigger";
            top.InitNewScript<TopMarioChecker>();
            top.Model = new Material(Color.Black, top);

            Trigger bottom = new Trigger(this);
            bottom.Transform.Position += new Vector3(0, 32, 0);
            bottom.Transform.Dimensions = new Vector3(16, 1, 0);
            bottom.Name = "Bottom_Trigger";
            bottom.InitNewScript<BottomMarioChecker>();
            bottom.Model = new Material(Color.Black, bottom);

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
