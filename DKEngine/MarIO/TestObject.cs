using DKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKEngine.Core.Components;
using DKEngine;
using MarIO.Assets.Models;

namespace MarIO
{
    class TestObject : GameObject
    {
        protected override void Init()
        {
            this.Name = "changer";
            this.InitNewScript<ChangerScript>();
        }
    }

    internal class ChangerScript : Script
    {
        GameObject Ref;
        int change = 0;

        public ChangerScript(GameObject Parent) : base(Parent)
        {
        }

        protected override void OnColliderEnter(Collider e)
        { }

        protected override void Start()
        {
            Ref = GameObject.Find("changed");
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

    class ChangedObject : Block
    {
        public ChangedObject()
        {
            Name = "changed";
        }
    }
}
