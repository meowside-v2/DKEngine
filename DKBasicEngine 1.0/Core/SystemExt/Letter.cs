﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    internal class Letter : GameObject
    {
        private TextBlock _Parent { get { return (TextBlock)Parent; } }

        internal float VertOffset = 0;
        internal float HorOffset = 0;

        public override bool HasShadow { get { return _Parent.TextShadow; } }
        
        internal override float X { get { return HorOffset + Position.X * _Parent.FontSize * Parent.ScaleX + Parent.X; } }
        internal override float Y { get { return VertOffset + Position.Y * _Parent.FontSize * Parent.ScaleY + Parent.Y; } }
        internal override float Z { get { return Parent.Z + Position.Z; } }

        internal override float Width { get { return Model.Width * _Parent.FontSize * Parent.ScaleX; } }
        internal override float Height { get { return Model.Height * _Parent.FontSize * Parent.ScaleX; } }
        internal override float Depth { get { return 0; } }

        internal override float ScaleX { get { return _Parent.FontSize * Parent.ScaleX; } }
        internal override float ScaleY { get { return _Parent.FontSize * Parent.ScaleY; } }
        internal override float ScaleZ { get { return _Parent.FontSize * Parent.ScaleZ; } }
        
        public Color? Foreground { get { return _Parent.Foreground; } }
        
        public Letter(TextBlock Parent, Position Transform, Material sourceModel)
        {
            Model = sourceModel;

            this.Position = Transform;
            this.Parent = Parent;

            Animator = new Animator(this);

            /*lock (Engine.ToStart)
            {
                lock (Engine.ToRender)
                {
                    Engine.ToStart.Add(this);
                    Engine.ToRender.Add(this);
                }
            }*/
        }

        public override void Destroy()
        {
            //Engine.UpdateEvent -= this._UpdateDel;
            Engine.ToRender.Remove(this);
            Engine.ToUpdate.Remove(this);
            _Parent._text.Remove(this);

            Parent = null;
            Animator = null;
            Model = null;
        }

        internal override void Render()
        {
            Model?.Render(this, Foreground);
        }
    }
}
