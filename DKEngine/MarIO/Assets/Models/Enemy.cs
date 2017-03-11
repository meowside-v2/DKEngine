using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarIO.Assets.Models
{
    class Enemy : GameObject
    {
        enum EnemyType
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

        Dictionary<EnemyType, string> EnemyTypeNames = new Dictionary<EnemyType, string>()
        {
            { EnemyType.Goomba, "" },
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

        public Enemy()
            :base()
        { }

        public Enemy(GameObject Parent)
            :base(Parent)
        { }

        protected override void Init()
        {
            this.Name = "Enemy";
        }
    }
}
