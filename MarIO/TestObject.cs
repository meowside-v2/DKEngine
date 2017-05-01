using DKEngine;
using DKEngine.Core;
using DKEngine.Core.Components;
using MarIO.Assets.Models;
using System;

namespace MarIO
{
    internal class TestObject : GameObject
    {
        protected override void Initialize()
        {
            this.Name = "changer";
            this.InitNewScript<ChangerScript>();
        }
    }

    internal class ChangerScript : Script
    {
        private GameObject Ref;
        private int change = 0;

        public ChangerScript(GameObject Parent) : base(Parent)
        {
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Ref = GameObject.Find<GameObject>("changed");
        }

        protected override void Update()
        {
            if (Engine.Input.IsKeyPressed(ConsoleKey.Enter))
            {
                change++;

                ((Block)Ref).Type = (Block.BlockType)(change % (int)Block.BlockType.NumberOfObjects);
            }
        }
    }

    internal class ChangedObject : Block
    {
        public ChangedObject()
        {
            Name = "changed";
        }
    }
}