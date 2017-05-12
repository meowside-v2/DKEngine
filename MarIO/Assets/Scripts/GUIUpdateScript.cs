using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;

namespace MarIO.Assets.Scripts
{
    public class GUIUpdateScript : Script
    {
        private TextBlock Time;
        private TextBlock Coins;
        private TextBlock World;
        private TextBlock Lives;
        private TextBlock Score;

        public GUIUpdateScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            this.World = GameObject.Find<TextBlock>("txt_World");
            this.Time = GameObject.Find<TextBlock>("txt_Time");
            this.Score = GameObject.Find<TextBlock>("txt_Score");
            this.Coins = GameObject.Find<TextBlock>("txt_Coins");
            this.Lives = GameObject.Find<TextBlock>("txt_Lives");

            this.World.Text = Engine.SceneName;
            this.Time.Text = string.Format("{0:000}", Shared.Mechanics.TimeLeft.TotalSeconds);
            this.Score.Text = string.Format("{0:00000000}", Shared.Mechanics.GameScore);
            this.Coins.Text = string.Format("*{0:00}", Shared.Mechanics.CoinsCount);
            this.Lives.Text = string.Format("*{0:00}", Shared.Mechanics.Lives);
        }

        protected override void Update()
        {
            this.Time.Text = string.Format("{0:000}", Shared.Mechanics.TimeLeft.TotalSeconds);
            this.Score.Text = string.Format("{0:00000000}", Shared.Mechanics.GameScore);
            this.Coins.Text = string.Format("*{0:00}", Shared.Mechanics.CoinsCount);
            this.Lives.Text = string.Format("*{0:00}", Shared.Mechanics.Lives);
        }
    }
}