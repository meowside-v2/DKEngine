using DKEngine.Core;
using DKEngine.Core.Components;
using DKEngine.Core.UI;
using MarIO.Assets.Models.Miscellaneous;
using MarIO.Assets.Scripts;

namespace MarIO.Assets.Models
{
    public class GUIUpdater : GameObject
    {
        protected override void Initialize()
        {
            Name = "GUI";

            this.IsGUI = true;

            /*------------ TIME TEXT ----------------*/

            #region TIME

            TextBlock _time = new TextBlock(this)
            {
                IsGUI = true,
                TextShadow = true,
                Text = "TIME",
                FontSize = 2
            };
            _time.Transform.Dimensions = new Vector3(100, 20, 1);
            _time.Transform.Position += new Vector3(16, 4, 128);

            TextBlock Time = new TextBlock(this)
            {
                Name = "txt_Time",
                IsGUI = true,
                TextShadow = true,
                Text = "",
                FontSize = 2
            };
            Time.Transform.Dimensions = new Vector3(100, 20, 1);
            Time.Transform.Position += new Vector3(22, 16, 128);

            #endregion TIME

            /*------------ SCORE TEXT ----------------*/

            #region SCORE

            TextBlock Score = new TextBlock(this)
            {
                Name = "txt_Score",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Right
            };
            Score.Transform.Dimensions = new Vector3(100, 20, 1);
            Score.Transform.Position += new Vector3(-16, 4, 128);

            #endregion SCORE

            /*------------ COINS TEXT ----------------*/

            #region COINS

            Coin UICoin = new Coin(this)
            {
                HasShadow = true
            };
            UICoin.Transform.Position += new Vector3(75, 4, 128);

            TextBlock _coins = new TextBlock(this)
            {
                Name = "txt_Coins",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 1.5f
            };
            _coins.Transform.Dimensions = new Vector3(100, 20, 1);
            _coins.Transform.Position += new Vector3(85, 4, 128);

            #endregion COINS

            /*------------ LIVES TEXT ----------------*/

            #region LIVES

            Heart UIHeart = new Heart(this)
            {
                HasShadow = true
            };
            UIHeart.Transform.Position += new Vector3(73, 16, 128);

            TextBlock _lives = new TextBlock(this)
            {
                Name = "txt_Lives",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 1.5f
            };
            _lives.Transform.Dimensions = new Vector3(100, 20, 1);
            _lives.Transform.Position += new Vector3(85, 18, 128);

            #endregion LIVES

            /*------------ WORLD TEXT ----------------*/

            #region WORLD

            TextBlock _world = new TextBlock(this)
            {
                Text = "WORLD",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            _world.Transform.Dimensions = new Vector3(50, 20, 1);
            _world.Transform.Position += new Vector3(-90, 4, 128);

            TextBlock World = new TextBlock(this)
            {
                Name = "txt_World",
                Text = "",
                IsGUI = true,
                TextShadow = true,
                FontSize = 2,
                HAlignment = Text.HorizontalAlignment.Right,
                TextHAlignment = Text.HorizontalAlignment.Center
            };
            World.Transform.Dimensions = new Vector3(50, 20, 1);
            World.Transform.Position += new Vector3(-90, 16, 128);

            #endregion WORLD

            this.InitNewScript<GUIUpdateScript>();
        }
    }
}