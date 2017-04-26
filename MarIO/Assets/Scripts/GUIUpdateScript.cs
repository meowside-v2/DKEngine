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

        public GUIUpdateScript(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            World = GameObject.Find<TextBlock>("txt_World");
            Time = GameObject.Find<TextBlock>("txt_Time");
            /*Coins = GameObject.Find<TextBlock>("txt_World");
            Lives = GameObject.Find<TextBlock>("txt_World");*/

            World.Text = Engine.SceneName;
            Time.Text = string.Format("{0:00}:{1:00}", Shared.TimeLeft.TotalMinutes, Shared.TimeLeft.Seconds);
        }

        protected override void Update()
        {

        }
    }
}
