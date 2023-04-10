using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class mainForm : Form
    {
        const int FIELD_HEGHT = 20;  // フィールドの高さ
        const int FIELD_WIDTH = 10;  // フィールドの幅
        const int FIELD_WALL = 2;    // 壁の厚さ
        const int FIELD_FLOOR = 2;   // 床の厚さ 
        const int FIELD_SPACE = 2;   // 上の空間
        const int SQUARE = 20;       // 1マスの大きさ
        const int INITIAL_X = 3;  // 初期X座標, ミノを中央から出現させる
        const int INITIAL_Y = 0;  // 初期Y座標
        int x = INITIAL_X;  // 現在のX座標
        int y = INITIAL_Y;  // 現在のY座標
        int[] next = new int[14];  // NEXT
        int tonextCount = 0;       // nextToNEXT()実行回数
        int dir = 0;  // ミノの向き
        const int NEXT_DISPLAY = 5;  // next表示数
        int hold = 0;
        bool useHold = false;
        private System.Windows.Forms.PictureBox[] nextView;
        /* インスタンス */
        minoColor MinoColor = new minoColor();
        bool DEBUG_MODE = false;
        bool gameStart = false;  // ゲーム開始フラグ

        // field[縦, 横]
        int[,] field = new int[FIELD_HEGHT + FIELD_SPACE + FIELD_FLOOR, FIELD_WIDTH + FIELD_WALL * 2];

        public mainForm()
        {
            InitializeComponent();
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            // 壁と床をつくる
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < FIELD_WALL; j++)
                {
                    field[i, j] = 8;  // 左壁
                    field[i, field.GetLength(1) - 1 - j] = 8;  // 右壁
                }
            }

            for (int i = 0; i < field.GetLength(1); i++)
            {
                for (int j = 0; j < FIELD_FLOOR; j++)
                {
                    field[field.GetLength(0) - 1 - j, i] = 8;  // 床
                }
            }

            this.nextView = new System.Windows.Forms.PictureBox[]
                { next_1, next_2, next_3, next_4, next_5 };
            for (int i = 0; i < nextView.Length; i++)
            {
                // 一旦全部非表示にする
                nextView[i].Visible = false;
            }
            for (int i = 1; i <= NEXT_DISPLAY; i++)
            {
                // NEXT表示数で設定した分だけ表示
                nextView[i - 1].Visible = true;
            }

            x = INITIAL_X;
            y = INITIAL_Y;

            nextDecide();
            minoDrawing(next[0], 0);
            nextDrawing();
            holdDrawing();

            // 自由落下有効
            if (!DEBUG_MODE) freeFall.Enabled = true;
            // デバッグモード
            if (DEBUG_MODE)
            {
                this.Text += " - Debug Mode";
            }

            gameStart = true;
        }

        void draw()
        {
            Bitmap canvas = new Bitmap(view_field.Width, view_field.Height);
            Graphics g = Graphics.FromImage(canvas);

            for (int i = 0; i < field.GetLength(0); i++)  // x軸
            {
                for (int j = 0; j < field.GetLength(1); j++)  // y軸
                {
                    if(field[i, j] != 0)
                    {
                        // テトリミノの描画
                        Color color = MinoColor.getColor(field[i, j]);
                        SolidBrush brush = new SolidBrush(color);
                        g.FillRectangle(brush, j * SQUARE - FIELD_WALL * SQUARE, (i - FIELD_SPACE) * SQUARE, SQUARE, SQUARE);
                        brush.Dispose();
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (mino[next[0], dir, i, j] != 0)
                    {
                        // ゴーストを描画
                        Color color = MinoColor.getColor(mino[next[0], dir, i, j], 100);
                        SolidBrush brush = new SolidBrush(color);
                        g.FillRectangle(brush, (x + j) * SQUARE, (minoGhost() + i - 1) * SQUARE, SQUARE, SQUARE);
                        brush.Dispose();
                    }
                }
            }

            // マス線を描画
            Color line = Color.FromArgb(80, 220, 220, 220);
            Pen p = new Pen(line, 1);
            // 横
            for(int i = 1; i < field.GetLength(0); i++)
            {
                g.DrawLine(p, 0, SQUARE * i, SQUARE * FIELD_HEGHT, SQUARE * i);
            }
            // 縦
            for(int i = 1; i < field.GetLength(1); i++)
            {
                g.DrawLine(p, SQUARE * i, 0, SQUARE * i, SQUARE * FIELD_HEGHT);
            }

            p.Dispose();
            g.Dispose();
            view_field.Image = canvas;
        }

        void minoDrawing(int m, int d)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // 空白のときに処理を行わないため
                    if (mino[m, d, i, j] != 0)
                    {
                        field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL] = mino[m, d, i, j];
                    }

                }
            }

            draw();

        }

        void nextDrawing()
        {
            const int SQUARE_NEXT = 10;        // NEXTの1マスの大きさ
            int[,] nextField = new int[6, 6];  // 描画フィールド

            for(int n = 1; n <= NEXT_DISPLAY; n++)
            {
                for(int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        nextField[i + 1, j + 1] = mino[next[n], 0, i, j];
                    }
                }

                Bitmap canvas = new Bitmap(view_field.Width, view_field.Height);
                Graphics g = Graphics.FromImage(canvas);

                for(int i = 0; i < nextField.GetLength(0); i++)
                {
                    for(int j = 0; j < nextField.GetLength(1); j++)
                    {
                        if (nextField[i, j] != 0)
                        {
                            // テトリミノの描画
                            Color color = MinoColor.getColor(nextField[i, j]);
                            SolidBrush brush = new SolidBrush(color);
                            g.FillRectangle(brush, j * SQUARE_NEXT - 5, i * SQUARE_NEXT - 5, SQUARE_NEXT, SQUARE_NEXT);
                            brush.Dispose();
                        }
                    }
                }
                g.Dispose();
                nextView[n - 1].Image = canvas;
            }
        }

        void holdDrawing()
        {
            const int SQUARE_HOLD = 10;
            int[,] holdField = new int[6, 6];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    holdField[i + 1, j + 1] = mino[hold, 0, i, j];
                }
            }

            Bitmap canvas = new Bitmap(view_hold.Width, view_hold.Height);
            Graphics g = Graphics.FromImage(canvas);

            for (int i = 0; i < holdField.GetLength(0); i++)
            {
                for (int j = 0; j < holdField.GetLength(1); j++)
                {
                    if (holdField[i, j] != 0)
                    {
                        // テトリミノの描画
                        Color color = MinoColor.getColor(holdField[i, j]);
                        SolidBrush brush = new SolidBrush(color);
                        g.FillRectangle(brush, j * SQUARE_HOLD - 5, i * SQUARE_HOLD - 5, SQUARE_HOLD, SQUARE_HOLD);
                        brush.Dispose();
                    }
                }
            }
            g.Dispose();
            view_hold.Image = canvas;
        }

        int[,,,] mino =
        {
            // ミノの色番号とミノの形の配列番号をそろえるためのdummy
            {
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
            },

            // Iミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 },
                    { 0, 0, 1, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 0, 0, 0 }
                },
                // 左
                {
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 },
                    { 0, 1, 0, 0 }
                }
            },

            // Oミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 0, 0, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 0, 0, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 2, 2, 0 },
                    { 0, 0, 0, 0 }
                }
            },

            // Sミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 0, 3, 3, 0 },
                    { 3, 3, 0, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 3, 0, 0 },
                    { 0, 3, 3, 0 },
                    { 0, 0, 3, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 3, 3, 0 },
                    { 3, 3, 0, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 3, 0, 0, 0 },
                    { 3, 3, 0, 0 },
                    { 0, 3, 0, 0 }
                }
            },

            // Zミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 4, 4, 0, 0 },
                    { 0, 4, 4, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 4, 0 },
                    { 0, 4, 4, 0 },
                    { 0, 4, 0, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 4, 4, 0, 0 },
                    { 0, 4, 4, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 0, 4, 0, 0 },
                    { 4, 4, 0, 0 },
                    { 4, 0, 0, 0 }
                }
            },

            // Jミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 5, 0, 0, 0 },
                    { 5, 5, 5, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 5, 5, 0 },
                    { 0, 5, 0, 0 },
                    { 0, 5, 0, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 5, 5, 5, 0 },
                    { 0, 0, 5, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 0, 5, 0, 0 },
                    { 0, 5, 0, 0 },
                    { 5, 5, 0, 0 }
                }
            },

            // Lミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 6, 0 },
                    { 6, 6, 6, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 6, 0, 0 },
                    { 0, 6, 0, 0 },
                    { 0, 6, 6, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 6, 6, 6, 0 },
                    { 6, 0, 0, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 6, 6, 0, 0 },
                    { 0, 6, 0, 0 },
                    { 0, 6, 0, 0 }
                }
            },

            // Tミノ
            {
                // 上
                {
                    { 0, 0, 0, 0 },
                    { 0, 7, 0, 0 },
                    { 7, 7, 7, 0 },
                    { 0, 0, 0, 0 }
                },
                // 右
                {
                    { 0, 0, 0, 0 },
                    { 0, 7, 0, 0 },
                    { 0, 7, 7, 0 },
                    { 0, 7, 0, 0 }
                },
                // 下
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 7, 7, 7, 0 },
                    { 0, 7, 0, 0 }
                },
                // 左
                {
                    { 0, 0, 0, 0 },
                    { 0, 7, 0, 0 },
                    { 7, 7, 0, 0 },
                    { 0, 7, 0, 0 }
                }
            }
        };

        void nextDecide()
        {
            int[] val = { 1, 2, 3, 4, 5, 6, 7 };
            int loop = next.Length / 7;  // ループする回数

            for (int i = 0; i < loop; i++)
            {
                // 各巡最初のNEXTが存在しないときに処理を行う
                if (next[i * 7] == 0)
                {
                    // 配列valをシャッフルして配列resに格納
                    // ここでランダムにする
                    int[] res = val.OrderBy(i => Guid.NewGuid()).ToArray();

                    // NEXTに適用
                    for (int j = 0; j < 7; j++)
                    {
                        next[i * 7 + j] = res[j];
                    }
                }
            }

        }

        void nextToNext()
        {
            // NEXTを次に進める
            for (int i = 0; i < next.Length - 1; i++)
            {
                next[i] = next[i + 1];
            }

            // ループ処理が終わったらNEXTの最後尾を0にする
            next[next.Length - 1] = 0;

            tonextCount++;

            if (tonextCount == 7)
            {
                // 7回実行する度にnextDecide()を更新
                nextDecide();
                tonextCount = 0;
            }
        }

        void minoMoving(int move, int pwr)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    // 現在位置に描画しているミノを一旦削除
                    int target = field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL];

                    if (target == mino[next[0], dir, i, j])
                    {
                        field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL] = 0;
                    }
                }
            }

            if(minoMoveCheck(move, pwr))
            {
                switch (move)
                {
                    case 0: // 上
                        y += -pwr;
                        break;
                    case 1: // 右
                        x += pwr;
                        break;
                    case 2: // 下
                        y += pwr;
                        break;
                    case 3: // 左
                        x += -pwr;
                        break;
                }
            }
            else if(move == 2)
            {
                // 下に何かある場合設置処理
                minoDrop();
            }
            

            // 新しく描画する
            minoDrawing(next[0], dir);
        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            // デバッグ用・NEXT操作
            if (DEBUG_MODE)
            {
                for(int i = 0; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        int target = field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL];
                        if(target == mino[next[0], dir, i, j])
                        {
                            field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL] = 0;
                        }
                    }
                }
            }
            if (gameStart)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        while (minoMoveCheck(2, 1)) minoMoving(2, 1);
                        minoMoving(2, 1);
                        if (!DEBUG_MODE)
                        {
                            freeFall.Stop();
                            freeFall.Start();
                        }
                        break;
                    case Keys.Left:
                        minoMoving(3, 1);
                        break;
                    case Keys.Right:
                        minoMoving(1, 1);
                        break;
                    case Keys.Down:
                        minoMoving(2, 1);
                        if (!DEBUG_MODE)
                        {
                            freeFall.Stop();
                            freeFall.Start();
                        }
                        break;
                    case Keys.Z:
                        minoTurn(0);
                        break;
                    case Keys.X:
                        minoTurn(1);
                        break;
                    case Keys.C:
                        holdControl();
                        break;
                }
            }
            
        }

        /*
         * ミノ移動チェック
         * 
         * 現在のミノを1マスずつチェックし、今いる場所から各方向へ移動出来るかを確認して
         * 大丈夫そうならtrueを返す
         * -------------------------
         * direction    移動方向
         * power        移動距離
         * srsx         スーパーローテーション用X座標オフセット
         * srsy         スーパーローテーション用Y座標オフセット
         */

        bool minoMoveCheck(int direction, int power, int srsx = 0, int srsy = 0)
        {
            int diry = 0;
            int dirx = 0;
            bool[] chked = { false, false, false, false }; // チェック済みの例

            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    // ミノの向きによってチェックする順番を決める
                    switch (direction)
                    {
                        case 0:
                            diry = i;
                            dirx = j;
                            break;
                        case 1:
                            diry = j;
                            dirx = 3 - i;
                            break;
                        case 2:
                            diry = 3 - i;
                            dirx = j;
                            break;
                        case 3:
                            diry = j;
                            dirx = i;
                            break;
                    }

                    // 現在ミノが空白の場合スキップ
                    if(mino[next[0], dir, diry, dirx] != 0)
                    {
                        // powerの値分進んで障害があればFalseを返す
                        switch (direction)
                        {
                            case 0:
                                if(field[diry + y - power + FIELD_SPACE - 1 + srsy, dirx + x + FIELD_WALL + srsx]
                                    != 0 && !chked[j])
                                {
                                    return false;
                                }
                                break;
                            case 1:
                                if (field[diry + y + FIELD_SPACE - 1 + srsy, dirx + x + power + FIELD_WALL + srsx]
                                    != 0 && !chked[j])
                                {
                                    return false;
                                }
                                break;
                            case 2:
                                if (field[diry + y + power + FIELD_SPACE - 1 + srsy, dirx + x + FIELD_WALL + srsx]
                                    != 0 && !chked[j])
                                {
                                    return false;
                                }
                                break;
                            case 3:
                                if (field[diry + FIELD_SPACE - 1 + srsy, dirx + x - power + FIELD_WALL + srsx]
                                    != 0 && !chked[j])
                                {
                                    return false;
                                }
                                break;
                        }
                        // 一度チェックした列は以降スキップする
                        chked[j] = true;
                    }
                }
            }
            return true;
        }

        /*
         * ミノの回転
         * 
         * 左回転でdirの値を1減らし、右回転で1増やすことで回転する
         * -------------------------------------
         * turn    回転方向(0…左回転, 1…右回転)
         */
        void minoTurn(int turn)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    // 現在位置に描画しているミノを一旦削除
                    int target = field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL];
                    if(target == mino[next[0], dir, i, j])
                    {
                        field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL] = 0;
                    }
                }
            }

            int dirTmp = dir;  // 以前のdirを控えておく

            switch (turn)
            {
                case 0:
                    dir--;
                    break;
                case 1:
                    dir++;
                    break;
            }
            // 下限、上限を超えないようにする
            if (dir < 0) dir = 3;
            if (dir > 3) dir = 0;

            // 通常の回転ができるかチェック
            if (!minoDuplicateCheck())
            {
                // できないならスーパーローテーションを試す
                if (!minoSuperRotation(dirTmp))
                {
                    // それでもできないなら回転前の状態に戻す
                    dir = dirTmp;
                }
            }

            minoDrawing(next[0], dir);
        }

        void minoDelete()
        {
            int line = 0;    // 消去ライン数

            // フィールド全体を確認する
            for(int i = 0; i < field.GetLength(0) - FIELD_FLOOR - FIELD_SPACE; i++)
            {
                // 各列チェック
                bool flag = false;
                for(int j = 0; j < FIELD_WIDTH; j++)
                {
                    if (field[i + FIELD_SPACE, j + FIELD_WALL] == 0) break;
                    if (j == FIELD_WIDTH - 1) flag = true;
                }

                if (flag)
                {
                    // 1段下げる
                    for(int j = i + FIELD_SPACE; j > 0; j--)
                    {
                        for(int k = 0; k < FIELD_WIDTH; k++)
                        {
                            // 消した列に上の列をコピー
                            field[j, k + FIELD_WALL] = field[j - 1, k + FIELD_WALL];
                            // 上の列を空白にする
                            field[j - 1, k + FIELD_WALL] = 0;
                        }
                    }
                }

                line++;
            }
        }

        void holdControl()
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    // 現在位置に描画しているミノを一旦削除
                    int target = field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL];
                    if(target == mino[next[0], dir, i, j])
                    {
                        field[i + y + FIELD_SPACE - 1, j + x + FIELD_WALL] = 0;
                    }
                }
            }

            if(hold == 0)
            {
                // ホールドに現在ミノを格納
                hold = next[0];
                // NEXTを進める
                nextToNext();
                nextDrawing();
            }
            else
            {
                // ホールドと現在ミノを交換
                int tmp = next[0];
                next[0] = hold;
                hold = tmp;
            }

            // 座標・向きのリセット
            x = INITIAL_X;
            y = INITIAL_Y;
            dir = 0;

            holdDrawing();
            minoDrawing(next[0], 0);

            // ホールド使用フラグON
            useHold = true;
        }

        bool minoDuplicateCheck(int srsy = 0, int srsx = 0)
        {
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    // 現在位置と操作中のミノが重複しないかチェック
                    if(field[i + y + FIELD_SPACE - 1 + srsy, j + x + FIELD_WALL + srsx] != 0 &&
                        mino[next[0], dir, i, j] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /*
         *スーパーローテーションシステム(SRS)
         *------------------------------------------
         * dirOld  回転前の向き
         */
        bool minoSuperRotation(int dirOld)
        {
            int movex = 0;   // X座標
            int movey = 0;   // Y座標

            // Iミノ以外
            if(next[0] != 1)
            {
                // 1. 軸を左右に動かす
                // 0が90度(B)の場合は左, -90度(D)の場合は右へ移動
                // 0が0度(A), 180度(C)の場合は回転した方向の逆へ移動
                switch (dir)
                {
                    case 1: // 右向き
                        movex = -1;
                        break;
                    case 3: // 左向き
                        movex = 1;
                        break;
                    case 0: // 上向き
                    case 2: // 下向き
                        switch (dirOld)
                        {
                            case 1: // 回転前が右向き
                                movex = 1;
                                break;
                            case 3: // 回転前が左向き
                                movex = -1;
                                break;
                        }
                        break;
                }
                if (!minoDuplicateCheck(movey, movex))
                {
                    // 2.その状態から軸を上下に動かす
                    // 0が90度, -90度(D)の場合は上へ移動
                    // 0が0度(A), 180度(C)の場合は下へ移動
                    switch (dir)
                    {
                        case 1:
                        case 3:
                            movey = -1;
                            break;
                        case 0:
                        case 2:
                            movey = 1;
                            break;
                    }
                    if (!minoDuplicateCheck(movey, movex))
                    {
                        // 3. 元に戻し、軸を上下に2マス動かす
                        // θが90度(B), -90度(D)の場合は下へ移動
                        // θが0度(A), 180度(C)の場合は上へ移動
                        movex = 0;
                        movey = 0;
                        switch (dir)
                        {
                            case 1:
                            case 3:
                                movey = 2;
                                break;
                            case 0:
                            case 2:
                                movey = -2;
                                break;
                        }
                        if (!minoDuplicateCheck(movey, movex))
                        {
                            // 4. その状態から軸を左右に動かす
                            // θが90度(B)の場合は左, -90度(D)の場合は右へ移動
                            // θが0度(A), 180度(C)の場合は回転した方向の逆へ移動
                            switch (dir)
                            {
                                case 1:
                                    movex = -1;
                                    break;
                                case 3:
                                    movex = 1;
                                    break;
                                case 0:
                                case 2:
                                    switch (dirOld)
                                    {
                                        case 1: // 回転前が右向き
                                            movex = 1;
                                            break;
                                        case 3: // 回転前が左向き
                                            movex = -1;
                                            break;
                                    }
                                    break;
                            }
                            if (!minoDuplicateCheck(movey, movex))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            // Iミノの場合
            else
            {
                int pt1x; // 1のX移動量
                int pt2x; // 2のX移動量

                // 1. 軸を左右に動かす
                // θが90度の場合は右, -90度(D)の場合は左に移動(枠にくっつく)
                // θが0度(A), 180度(C)の売位は回転した方向の逆へ移動 0度は2マス移動
                switch (dir)
                {
                    case 1:
                        movex = 1;
                        break;
                    case 3:
                        movex = -1;
                        break;
                    case 0:
                    case 2:
                        switch (dirOld)
                        {
                            case 1:
                                movex = -1;
                                break;
                            case 3:
                                movex = 1;
                                break;
                        }
                        if (dir == 0) movex *= 2; // 0度は2マス移動
                        break;
                }
                pt1x = movex;
                if(!minoDuplicateCheck(movey, movex))
                {
                    // 2. 軸を左右に動かす
                    // θが90度(B)の場合は左, -90度(D)の場合は右へ移動(枠にくっつく)
                    // θが0度(A), 180度(C)の場合は回転した方向へ移動 180度は2マス移動
                    switch (dir)
                    {
                        case 1:
                            movex = -1;
                            break;
                        case 3:
                            movex = 1;
                            break;
                        case 0:
                        case 2:
                            switch (dirOld)
                            {
                                case 1:
                                    movex = 1;
                                    break;
                                case 3:
                                    movex = 1;
                                    break;
                            }
                            if (dir == 2) movex *= 2;  // 180度は2マス移動
                            break;
                    }
                    pt2x = movex;
                    if(!minoDuplicateCheck(movey, movex))
                    {
                        // 3. 軸を上下に動かす
                        // θが90度(B)の場合は1を下、-90度(D)の場合は1を上へ移動
                        // θが0度(A)、180度(C)の場合は
                        // 回転前のミノが右半分にある(B)なら1を上へ
                        // 回転前のミノが左半分にある(D)なら2を下へ移動
                        // 左回転なら2マス動かす
                        switch (dir)
                        {
                            case 1:
                                movex = pt1x;
                                movey = 1;
                                break;
                            case 3:
                                movex = pt1x;
                                movey = -1;
                                break;
                            case 0:
                            case 2:
                                switch (dirOld)
                                {
                                    case 1:
                                        movex = pt1x;
                                        movey = -1;
                                        break;
                                    case 3:
                                        movex = pt2x;
                                        movey = 1;
                                        break;
                                }
                                break;
                        }
                        // 左回転
                        if(dirOld == 0 && dir == 3 || dirOld == 3 && dir == 2 || 
                            dirOld == 2 && dir == 1 || dirOld == 1 && dir == 0)
                        {
                            movey *= 2;
                        }
                        if(!minoDuplicateCheck(movey, movex))
                        {
                            // 4. 軸を上下に動かす
                            // θが90度(B)の場合は2の上、-90度(D)の場合は2を下へ移動
                            // θが0度(A), 180度(C)の場合は
                            // 回転前のミノが右半分にある(B)なら2を下へ
                            // 回転前のミノが左半分にある(D)なら1を上へ移動
                            // 右回転なら2マス動かす
                            switch (dir)
                            {
                                case 1:
                                    movex = pt2x;
                                    movey = -1;
                                    break;
                                case 3:
                                    movex = pt2x;
                                    movey = 1;
                                    break;
                                case 0:
                                case 2:
                                    switch (dirOld)
                                    {
                                        case 1:
                                            movex = pt2x;
                                            movey = 1;
                                            break;
                                        case 3:
                                            movex = pt1x;
                                            movey = -1;
                                            break;
                                    }
                                    break;
                            }
                            // 右回転
                            if (dirOld == 3 && dir == 0 || dirOld == 0 && dir == 1 || 
                                dirOld == 1 && dir == 2 || dirOld == 2 && dir == 3)
                            {
                                movey *= 2;
                            }
                            if(!minoDuplicateCheck(movey, movex))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            x += movex;
            y += movey;

            return true;
        }

        int minoGhost()
        {
            int ghostY = 0;
            // 下方向に壁やミノがあるか確認し続けて、ぶつかった所のY座標を返す
            while (minoMoveCheck(2, 1, 0, ghostY)) ghostY++;
            return ghostY + y;
        }

        private void freeFall_Tick(object sender, EventArgs e)
        {
            if (!DEBUG_MODE) minoMoving(2, 1);
        }

        async void GameOver()
        {
            gameStart = false;

            await Task.Delay(1000);

            // フィールドが落下する
            int i = 1;
            while(view_field.Top < this.Height)
            {
                view_field.Top += i;
                if (i < 40) i += i;
                await Task.Delay(20);
            }

            gameOverText.Visible = true;
        }

        void minoDrop()
        {
            if (gameStart)
            {
                // フィールドに描画
                minoDrawing(next[0], dir);

                // NEXTを進める
                nextToNext();

                // NEXTを更新
                nextDrawing();

                // ライン消去
                minoDelete();

                // 座標の初期化
                x = INITIAL_X;
                y = INITIAL_Y;
                dir = 0;

                // ホールドフラグの初期化
                useHold = false;

                // この時点でミノが重複していたらゲームオーバー
                if (!minoDuplicateCheck()) GameOver();

                if (!DEBUG_MODE)
                {
                    freeFall.Stop();
                    freeFall.Start();
                }
            }
        }
    }
}
