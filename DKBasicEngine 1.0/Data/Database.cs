using DKBasicEngine_1_0.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DKBasicEngine_1_0
{
    public static class Database
    {
        public enum Font
        {
            Num0,
            Num1,
            Num2,
            Num3,
            Num4,
            Num5,
            Num6,
            Num7,
            Num8,
            Num9,
            A,
            B,
            C,
            D,
            E,
            F,
            G,
            H,
            I,
            J,
            K,
            L,
            M,
            N,
            O,
            P,
            Q,
            R,
            S,
            T,
            U,
            V,
            W,
            X,
            Y,
            Z,
            minus,
            space,
            NumberOfTypes
        };

        public static Dictionary<char, Font> font = new Dictionary<char, Font>()
        {
            { '0' , Font.Num0 },
            { '1' , Font.Num1 },
            { '2' , Font.Num2 },
            { '3' , Font.Num3 },
            { '4' , Font.Num4 },
            { '5' , Font.Num5 },
            { '6' , Font.Num6 },
            { '7' , Font.Num7 },
            { '8' , Font.Num8 },
            { '9' , Font.Num9 },
            { 'A' , Font.A },
            { 'B' , Font.B },
            { 'C' , Font.C },
            { 'D' , Font.D },
            { 'E' , Font.E },
            { 'F' , Font.F },
            { 'G' , Font.G },
            { 'H' , Font.H },
            { 'I' , Font.I },
            { 'J' , Font.J },
            { 'K' , Font.K },
            { 'L' , Font.L },
            { 'M' , Font.M },
            { 'N' , Font.N },
            { 'O' , Font.O },
            { 'P' , Font.P },
            { 'Q' , Font.Q },
            { 'R' , Font.R },
            { 'S' , Font.S },
            { 'T' , Font.T },
            { 'U' , Font.U },
            { 'V' , Font.V },
            { 'W' , Font.W },
            { 'X' , Font.X },
            { 'Y' , Font.Y },
            { 'Z' , Font.Z },
            { '-' , Font.minus },
            { ' ' , Font.space }
        };

        public static List<Material> letterMaterial = new List<Material>();

        private static void CreateLetterReferences()
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(Resources.FontFile)))
            {
                int lenght = br.ReadInt32();

                for (int index = 0; index < lenght; index++)
                {
                    int temp = br.ReadInt32();
                    byte[] byteArray = br.ReadBytes(temp);

                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        letterMaterial.Add(new Material((Bitmap)Image.FromStream(ms)));
                    }
                }
            }
        }

        private static Dictionary<string, int> GameObjects = new Dictionary<string, int>();

        private static List<Material> GameObjectsMaterial = new List<Material>();

        public static void InitDatabase()
        {
            AddNewGameObject("border", new Material(Resources.border));

            CreateLetterReferences();
        }

        public static void AddNewGameObject(string ObjectName, Material Object)
        {
            GameObjects.Add(ObjectName, GameObjectsMaterial.Count);
            GameObjectsMaterial.Add(Object);
        }

        public static Material GetGameObjectMaterial(string Key)
        {
            Material retValue = null;

            try
            {
                retValue = GameObjectsMaterial[GameObjects[Key]];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Object not found\n" + ex);
            }

            return retValue;
        }
    }
}
