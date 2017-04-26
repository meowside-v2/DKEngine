using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using DKEngine;

namespace MarIO.Assets.Scripts
{
    class GUIUpdateScript : Script
    {
        TextBlock Time;
        TextBlock Coins;
        TextBlock World;
        TextBlock Lives;
        TextBlock Score;

        public GUIUpdateScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            World = GameObject.Find<TextBlock>("txt_World");
            Time = GameObject.Find<TextBlock>("txt_Time");
            Score = GameObject.Find<TextBlock>("txt_Score");
            /*Coins = GameObject.Find<TextBlock>("txt_World");
            Lives = GameObject.Find<TextBlock>("txt_World");*/

            World.Text = Engine.SceneName;
            Time.Text = string.Format("{0:000}", Shared.TimeLeft.TotalSeconds);
            Score.Text = string.Format("{0:00000000}", Shared.Points);
        }

        protected override void Update()
        {
            Time.Text = string.Format("{0:000}", Shared.TimeLeft.TotalSeconds);
            Score.Text = string.Format("{0:00000000}", Shared.Points);
        }
    }
}
