using DKEngine.Core;
using DKEngine.Core.Components;
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
        protected override void Init()
        {
            this.Name = "Player";
            //this.TypeName = "mario";
            this.Transform.Position = new Vector3(50, -10, 0);
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("idle", Database.GetGameObjectMaterial("mario"));
            this.Animator.AddAnimation("right_move", Database.GetGameObjectMaterial("mario_move_right"));
            this.Animator.AddAnimation("left_move", Database.GetGameObjectMaterial("mario_move_left"));
            this.Animator.AddAnimation("right_jump", Database.GetGameObjectMaterial("mario_jump_right"));
            this.Animator.AddAnimation("left_jump", Database.GetGameObjectMaterial("mario_jump_left"));
            this.InitNewScript<CharacterController>();
            //this.InitNewComponent<PlayerControl>();
        }
    }
}
