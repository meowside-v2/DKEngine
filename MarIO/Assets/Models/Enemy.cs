using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Scripts;
using System.Collections.Generic;

namespace MarIO.Assets.Models
{
    internal class Enemy : AnimatedObject
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

        private Dictionary<EnemyType, string> EnemyTypeNames = new Dictionary<EnemyType, string>()
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

        protected override void Initialize()
        {
            this.Name = "Enemy";
            //this.TypeName = EnemyTypeNames[Type];

            switch (Type)
            {
                case EnemyType.Goomba:
                    this.InitNewComponent<Collider>();
                    this.Collider.Area = new System.Drawing.RectangleF(0, 0, 16, 16);

                    this.InitNewScript<GoombaController>();
                    this.InitNewComponent<Animator>();
                    this.Animator.AddAnimation("default", Database.GetGameObjectMaterial(EnemyTypeNames[Type]));
                    break;

                case EnemyType.GoombaBlue:
                    break;

                case EnemyType.GoombaSilver:
                    break;

                case EnemyType.KoopaTroopa:
                    break;

                case EnemyType.KoopaParatroopa:
                    break;

                case EnemyType.PiranhaPlant:
                    break;

                case EnemyType.Spiny:
                    break;

                case EnemyType.BuzzyBeatle:
                    break;

                case EnemyType.BuzzyBeatleBlue:
                    break;

                case EnemyType.BuzzyBeatleSilver:
                    break;

                case EnemyType.FireBar:
                    break;

                case EnemyType.BulletBill:
                    break;

                case EnemyType.BillBlasterLarge:
                    break;

                case EnemyType.BillBlasterSmall:
                    break;

                default:
                    break;
            }
        }
    }
}