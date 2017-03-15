using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using System.Diagnostics;
using DKEngine;
using MarIO.Assets.Models;

namespace MarIO.Assets.Scripts
{
    class PipePort : Script
    {
        GameObject Player;
        Block Pipe;

        public PipePort(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if(e.Parent == Player)
            {
                ConsoleKey tmp = Pipe.Type == Block.BlockType.Pipe1 ? ConsoleKey.D : ConsoleKey.S;

                if (Engine.Input.IsKeyDown(tmp))
                {
                    Debug.Write("Mario Action Do");
                }
            }
        }

        protected override void Start()
        {
            Player = GameObject.Find("Player");
            Pipe = (Block)Parent;
        }

        protected override void Update()
        { }
    }
}
