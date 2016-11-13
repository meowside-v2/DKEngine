using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DKBasicEngine_1_0
{
    public class Keyboard
    {
        [DllImport("user32.dll")]
        public static extern ushort GetKeyState(short nVirtKey);

        public const ushort keyDownBit = 0x80;

        public bool IsKeyPressed(ConsoleKey key)
        {
            return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
        }

        private int? numInt = 0;
        private int? specInt = 0;
        private int? alphaInt = 0;

        public Keyboard(int? numericalInterval, int? specialInterval, int? alphaInterval)
        {
            numInt = numericalInterval;
            specInt = specialInterval;
            alphaInt = alphaInterval;

            if (numericalInterval != null)
            {
                checkerNumber.AutoReset = true;
                checkerNumber.Interval = (int)numericalInterval;
                checkerNumber.Elapsed += NumKeysEvent;
            }

            if (alphaInterval != null)
            {
                checkerAlpha.AutoReset = true;
                checkerAlpha.Interval = (int)alphaInterval;
                checkerAlpha.Elapsed += AlphaKeysEvent;
            }

            if (specialInterval != null)
            {
                checkerSpecial.AutoReset = true;
                checkerSpecial.Interval = (int)specialInterval;
                checkerSpecial.Elapsed += OtherKeyEvent;
            }
        }


        public void Start()
        {
            if (numInt != null)
                checkerNumber.Start();

            if (specInt != null)
                checkerSpecial.Start();

            if (alphaInt != null)
                checkerAlpha.Start();
        }

        public void Abort()
        {
            if (numInt != null)
                checkerNumber.Stop();

            if (specInt != null)
                checkerSpecial.Stop();

            if (alphaInt != null)
                checkerAlpha.Stop();
        }

        Timer checkerNumber = new Timer();
        Timer checkerAlpha = new Timer();
        Timer checkerSpecial = new Timer();


        //
        //  Alpha keys
        //
        public Action onAKey { get; set; }
        public Action onBKey { get; set; }
        public Action onCKey { get; set; }
        public Action onDKey { get; set; }
        public Action onEKey { get; set; }
        public Action onFKey { get; set; }
        public Action onGKey { get; set; }
        public Action onHKey { get; set; }
        public Action onIKey { get; set; }
        public Action onJKey { get; set; }
        public Action onKKey { get; set; }
        public Action onLKey { get; set; }
        public Action onMKey { get; set; }
        public Action onNKey { get; set; }
        public Action onOKey { get; set; }
        public Action onPKey { get; set; }
        public Action onQKey { get; set; }
        public Action onRKey { get; set; }
        public Action onSKey { get; set; }
        public Action onTKey { get; set; }
        public Action onUKey { get; set; }
        public Action onVKey { get; set; }
        public Action onWKey { get; set; }
        public Action onXKey { get; set; }
        public Action onYKey { get; set; }
        public Action onZKey { get; set; }

        //
        //  Numerical keys
        //

        public Action onN0ArrowKey { get; set; }
        public Action onN1ArrowKey { get; set; }
        public Action onN2ArrowKey { get; set; }
        public Action onN3ArrowKey { get; set; }
        public Action onN4ArrowKey { get; set; }
        public Action onN5ArrowKey { get; set; }
        public Action onN6ArrowKey { get; set; }
        public Action onN7ArrowKey { get; set; }
        public Action onN8ArrowKey { get; set; }
        public Action onN9ArrowKey { get; set; }

        //
        //  Special keys
        //

        public Action onEnterKey { get; set; }
        public Action onEndKey { get; set; }
        public Action onDeleteKey { get; set; }
        public Action onEscKey { get; set; }
        public Action onSpaceKey { get; set; }
        public Action onBackSpaceKey { get; set; }
        public Action onInsertKey { get; set; }
        public Action onPageUpKey { get; set; }
        public Action onPageDownKey { get; set; }
        public Action onUpArrowKey { get; set; }
        public Action onDownArrowKey { get; set; }
        public Action onLeftArrowKey { get; set; }
        public Action onRightArrowKey { get; set; }

        public void onAlphaNumericalKeys(Action ac)
        {
            onNumericalKeys(ac);
            OnAlphaKeys(ac);
        }

        public void onNumericalKeys(Action ac)
        {
            onN0ArrowKey = ac;
            onN1ArrowKey = ac;
            onN2ArrowKey = ac;
            onN3ArrowKey = ac;
            onN4ArrowKey = ac;
            onN5ArrowKey = ac;
            onN6ArrowKey = ac;
            onN7ArrowKey = ac;
            onN8ArrowKey = ac;
            onN9ArrowKey = ac;
        }

        public void OnAlphaKeys(Action ac)
        {
            onAKey = ac;
            onBKey = ac;
            onCKey = ac;
            onDKey = ac;
            onEKey = ac;
            onFKey = ac;
            onGKey = ac;
            onHKey = ac;
            onIKey = ac;
            onJKey = ac;
            onKKey = ac;
            onLKey = ac;
            onMKey = ac;
            onNKey = ac;
            onOKey = ac;
            onPKey = ac;
            onQKey = ac;
            onRKey = ac;
            onSKey = ac;
            onTKey = ac;
            onUKey = ac;
            onVKey = ac;
            onWKey = ac;
            onXKey = ac;
            onYKey = ac;
            onZKey = ac;
        }

        private void AlphaKeysEvent(object sender, ElapsedEventArgs e)
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            if (this.IsKeyPressed(ConsoleKey.A))
            {
                onAKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.B))
            {
                onBKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.C))
            {
                onCKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.D))
            {
                onDKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.E))
            {
                onEKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.F))
            {
                onFKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.G))
            {
                onGKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.H))
            {
                onHKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.I))
            {
                onIKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.J))
            {
                onJKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.K))
            {
                onKKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.L))
            {
                onLKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.M))
            {
                onMKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.N))
            {
                onNKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.O))
            {
                onOKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.P))
            {
                onPKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Q))
            {
                onQKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.R))
            {
                onRKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.S))
            {
                onSKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.T))
            {
                onTKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.U))
            {
                onUKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.V))
            {
                onVKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.W))
            {
                onWKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.X))
            {
                onXKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Y))
            {
                onYKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Z))
            {
                onZKey?.Invoke();
            }
        }

        private void NumKeysEvent(object sender, ElapsedEventArgs e)
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            if (this.IsKeyPressed(ConsoleKey.NumPad0))
            {
                onN0ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad1))
            {
                onN1ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad2))
            {
                onN2ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad3))
            {
                onN3ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad4))
            {
                onN4ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad5))
            {
                onN5ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad6))
            {
                onN6ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad7))
            {
                onN7ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad8))
            {
                onN8ArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.NumPad9))
            {
                onN9ArrowKey?.Invoke();
            }
        }

        private void OtherKeyEvent(object sender, ElapsedEventArgs e)
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            if (this.IsKeyPressed(ConsoleKey.Enter))
            {
                onEnterKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.End))
            {
                onEndKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Delete))
            {
                onDeleteKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Escape))
            {
                onEscKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Spacebar))
            {
                onSpaceKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Backspace))
            {
                onBackSpaceKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.Insert))
            {
                onInsertKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.PageUp))
            {
                onPageUpKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.PageDown))
            {
                onPageDownKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.UpArrow))
            {
                onUpArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.DownArrow))
            {
                onDownArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.LeftArrow))
            {
                onLeftArrowKey?.Invoke();
            }
            if (this.IsKeyPressed(ConsoleKey.RightArrow))
            {
                onRightArrowKey?.Invoke();
            }
        }
    }
}
