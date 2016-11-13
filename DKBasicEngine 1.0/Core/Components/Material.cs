using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKBasicEngine_1_0
{
    public class Material
    {
        public int width;
        public int height;

        public Color[,] colorMap;

        public Material(Bitmap source)
        {
            colorMap = new Color[source.Width, source.Height];

            width = source.Width;
            height = source.Height;

            for (int row = 0; row < source.Height; row++)
            {
                for (int column = 0; column < source.Width; column++)
                {
                    colorMap[column, row] = source.GetPixel(column, row);
                }
            }
        }

        public Material(Color clr, int width, int height)
        {
            this.width = width;
            this.height = height;

            colorMap = new Color[width, height];

            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    colorMap[column, row] = clr;
                }
            }
        }

        public string PixelToString(int x, int y)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", this.colorMap[x, y].A,
                                                              this.colorMap[x, y].R,
                                                              this.colorMap[x, y].G,
                                                              this.colorMap[x, y].B);
        }

        public object DeepCopy()
        {
            return this.MemberwiseClone();
        }

        public void Render(int x, int y, byte[] imageBuffer, bool[] imageBufferKey, double scaleX, double scaleY, Color? clr = null)
        {
            int rowInBuffer = 0;
            int columnInBuffer = 0;

            double plusX = 1 / scaleX;
            double plusY = 1 / scaleY;

            if (clr == null)
            {
                for (double row = 0; row < this.height; row += plusY)
                {
                    if (y + row > Shared.RenderHeight) return;

                    for (double column = 0; column < this.width; column += plusX)
                    {
                        if (x + column > Shared.RenderWidth) break;

                        if (IsOnScreen(x + columnInBuffer, y + rowInBuffer))
                        {
                            int offset = (((3 * (y + rowInBuffer)) * Shared.RenderWidth) + (3 * (x + columnInBuffer)));
                            int keyOffset = ((y + rowInBuffer) * Shared.RenderWidth + x + columnInBuffer);

                            if (!imageBufferKey[keyOffset])
                            {
                                Color temp = colorMap[(int)column, (int)row];

                                if (temp.A != 0)
                                {
                                    imageBuffer[offset] = temp.B;
                                    imageBuffer[offset + 1] = temp.G;
                                    imageBuffer[offset + 2] = temp.R;

                                    imageBufferKey[keyOffset] = true;
                                }
                            }
                        }

                        columnInBuffer++;
                    }

                    rowInBuffer++;
                    columnInBuffer = 0;
                }
            }

            else
            {
                for (double row = 0; row < this.height; row += (1 / scaleY))
                {
                    if (y + row > Shared.RenderHeight) return;

                    for (double column = 0; column < this.width; column += (1 / scaleX))
                    {
                        if (x + column > Shared.RenderWidth) break;

                        if (IsOnScreen(x + columnInBuffer, y + rowInBuffer))
                        {
                            int offset = (((3 * (y + rowInBuffer)) * Shared.RenderWidth) + (3 * (x + columnInBuffer)));
                            int keyOffset = ((y + rowInBuffer) * Shared.RenderWidth + x + columnInBuffer);

                            if (!imageBufferKey[keyOffset])
                            {
                                Color temp = colorMap[(int)column, (int)row];

                                if (temp.A != 0)
                                {
                                    imageBuffer[offset] = ((Color)clr).B;
                                    imageBuffer[offset + 1] = ((Color)clr).G;
                                    imageBuffer[offset + 2] = ((Color)clr).R;

                                    imageBufferKey[keyOffset] = true;
                                }
                            }
                        }

                        columnInBuffer++;
                    }

                    rowInBuffer++;
                    columnInBuffer = 0;
                }
            }
        }

        private bool IsOnScreen(int x, int y)
        {
            return x >= 0 && x < Shared.RenderWidth && y >= 0 && y < Shared.RenderHeight;
        }
    }
}
