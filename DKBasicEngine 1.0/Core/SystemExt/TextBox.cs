using System;
using System.Diagnostics;
using System.Linq;

namespace DKBasicEngine_1_0
{
    public class TextBox : TextBlock, IControl
    {
        public bool IsFocused { get; set; }
        public int FocusElementID { get; private set; }

        private TimeSpan TimeOut = new TimeSpan(0, 0, 0, 0, 50);
        private Stopwatch TimeOutStopwatch = new Stopwatch();

        private short MaxTextLenght = 64;

        public TextBox()
        {
            /*FocusElementID = Engine.Scene.PageControls.Count;
            Engine.Scene.PageControls.Add(this);*/
        }

        public TextBox(GameObject Parent)
            :base(Parent)
        {
            /*FocusElementID = Engine.Scene.PageControls.Count;
            Engine.Scene.PageControls.Add(this);*/
        }

        public Type AllowedChars { get; set; }

        public enum Type
        {
            All,
            AlphaNumerical,
            Alpha,
            Numerical
        };

        public override string Text
        {
            set
            {
                if (TextControl(value))
                {
                    _textStr = value;
                    _changed = true;
                }
                
            }

            get
            {
                return _textStr;
            }
        }
        
        public override void Update()
        {
            if (IsFocused)
            {

                if (Console.KeyAvailable)
                {
                    if (TimeOut < TimeOutStopwatch.Elapsed)
                        TimeOutStopwatch.Reset();

                    if (TimeOutStopwatch.ElapsedMilliseconds == 0)
                    {
                        char key = Console.ReadKey(true).KeyChar;

                        while (Console.KeyAvailable) Console.ReadKey();

                        if (key == '\b')
                        {
                            if (Text.Length > 0)
                            {
                                Text = Text.Remove(Text.Length - 1, 1);
                            }

                            TimeOutStopwatch.Start();
                        }

                        else if (Text.Length < MaxTextLenght)
                        {
                            Text += key;

                            TimeOutStopwatch.Start();
                        }
                    }
                }

                else if (TimeOutStopwatch.IsRunning)
                  TimeOutStopwatch.Reset();
            }

            base.Update();
        }

        private bool TextControl(string key)
        {
            if (AllowedChars == Type.All)
                return true;

            else if (AllowedChars == Type.Alpha)
                return key.All(Char.IsLetter);

            else if (AllowedChars == Type.Numerical)
                return key.All(Char.IsNumber);

            else if (AllowedChars == Type.AlphaNumerical)
                return key.All(Char.IsLetterOrDigit);

            return true;
        }

        private bool TextControl(char key)
        {
            if (AllowedChars == Type.All)
                return true;

            else if (AllowedChars == Type.Alpha)
                return Char.IsLetter(key);

            else if (AllowedChars == Type.Numerical)
                return Char.IsNumber(key);

            else if (AllowedChars == Type.AlphaNumerical)
                return Char.IsLetterOrDigit(key);

            return true;
        }
    }
}
