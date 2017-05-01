using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;
using System.Diagnostics;

namespace MarIO.Assets.Scripts
{
    internal class PipePort : Script
    {
        private GameObject Player;
        private Block Pipe;

        public PipePort(GameObject Parent) : base(Parent)
        { }

        protected override void OnColliderEnter(Collider e)
        {
            if (e.Parent == Player)
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