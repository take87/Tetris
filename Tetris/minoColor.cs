using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tetris
{
    class minoColor
    {
        Color[] Col =
        {
            Color.FromArgb(255,255,255),    // 空白  白
            Color.FromArgb(0, 255, 255),    // I     水
            Color.FromArgb(255, 255, 0),    // O     黄
            Color.FromArgb(0, 221, 0),      // S     緑
            Color.FromArgb(255, 0, 0),      // Z     赤
            Color.FromArgb(30, 128, 255),   // J     青
            Color.FromArgb(255, 140, 0),    // L     橙
            Color.DarkViolet,    // T     紫
            Color.FromArgb(0, 0, 0)         // 壁    黒
        };

        public void setColor(int type, Color c)
        {
            Col[type] = c;
        }
        public Color getColor(int type, int alpha = 255)
        {
            return Color.FromArgb(alpha, Col[type]);
        }
    }
}
