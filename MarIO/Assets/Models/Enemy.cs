using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System.Collections.Generic;
using System;

namespace MarIO.Assets.Models
{
    internal abstract class Enemy : AnimatedObject
    {
        public enum EnemyType
        {
            Goomba,
            GoombaBlue,
            GoombaSilver,
            KoopaTroopa,
            KoopaParatroopa,
            PiranhaPlant,
            Spiny,
            BuzzyBeatle,
            BuzzyBeatleBlue,
            BuzzyBeatleSilver,
            FireBar,
            BulletBill,
            BillBlasterLarge,
            BillBlasterSmall
        }

        protected static Dictionary<EnemyType, string> EnemyTypeNames = new Dictionary<EnemyType, string>()
        {
            { EnemyType.Goomba, "goomba" },
            { EnemyType.GoombaBlue, "" },
            { EnemyType.GoombaSilver, "" },
            { EnemyType.KoopaTroopa, "" },
            { EnemyType.KoopaParatroopa, "" },
            { EnemyType.PiranhaPlant, "" },
            { EnemyType.Spiny, "" },
            { EnemyType.BuzzyBeatle, "" },
            { EnemyType.BuzzyBeatleBlue, "" },
            { EnemyType.BuzzyBeatleSilver, "" },
            { EnemyType.FireBar, "" },
            { EnemyType.BulletBill, "" },
            { EnemyType.BillBlasterLarge, "" },
            { EnemyType.BillBlasterSmall, "" }
        };

        public EnemyType Type { get; set; }

        public Enemy()
            : base()
        { }

        public Enemy(GameObject Parent)
            : base(Parent)
        { }
    }

    class Goomba : Enemy
    {
        protected override void Initialize()
        {
            this.Name = "Goomba";
            this.Type = EnemyType.Goomba;

            this.InitNewComponent<Collider>();
            this.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);

            this.InitNewScript<GoombaController>();
            this.InitNewComponent<Animator>();
            this.Animator.AddAnimation("default", Database.GetGameObjectMaterial(EnemyTypeNames[Type]));
            this.Animator.AddAnimation("dead", Database.GetGameObjectMaterial(EnemyTypeNames[Type] + "_dead"));
        }
    }
}