using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab
{
    public class Jel
    {
        public static string Shifr_Jel(string text, int key)
        {
            char[,] to_shifr=new char[text.Length/key+1, key * 2 - 2];
            int ind = 0;

            for (int j = 0; (j < text.Length / key + 1) && (text.Length != ind); j++)
                for (int i = 0; (i < (key * 2 - 2)) && (text.Length != ind); i++,ind++)
                {
                    to_shifr[j,i] = text[ind];
                }
            string shifr="";
            for (int i = 0; (i < text.Length / key + 1) && (to_shifr[i, 0] != '\0'); i++)
                shifr = shifr + to_shifr[i,0];

            for (int i = 1; i < key; i++)
                for (int j = 0; (j < (text.Length / key + 1) && (to_shifr[j, i]!='\0')); j++)
                    if ((i < key-1) && (to_shifr[j, key * 2 - 2 - i]!='\0'))
                        shifr = shifr + to_shifr[j, i] + to_shifr[j, key * 2 - 2 - i];
                    else
                        shifr = shifr + to_shifr[j, i];

            return shifr;
        }

        public static string Deshifr_Jel(string text, int key)
        {
            char[,] fence = new char[key,text.Length];

            bool downwardDirection = false;
            uint row = 0, column = 0;

            for (int i = 0; i < text.Length; ++i)
            {
                if (row == 0 || row == key - 1)
                    downwardDirection = !downwardDirection;

                fence[row, column] = '~';
                ++column;

                if (downwardDirection)
                    ++row;
                else
                    --row;
            }

            int index = 0;
            for (uint i = 0; i < key; ++i)
                for (uint j = 0; j < text.Length; ++j)
                    if (fence[i, j] == '~')
                        fence[i, j] = text[index++];

            string plainText = "";
            downwardDirection = false;
            row = 0;
            column = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                if (row == 0 || row == key - 1)
                    downwardDirection = !downwardDirection;

                plainText += fence[row, column];
                ++column;

                if (downwardDirection)
                    ++row;
                else
                    --row;
            }

            return plainText;
        }
    }


    public class Col
    {
        public static string Shifr_Col(string text, string key)
        {
            char[,] to_shifr = new char[text.Length/key.Length+2, key.Length];
            for (int i=1, ind=0;(i< text.Length / key.Length + 2) && (ind < text.Length); i++)
            {
                for (int j = 0; (j < key.Length)&& (ind<text.Length); j++,ind++)
                    to_shifr[i, j] = text[ind];
            }

            for (int i = 0; i < key.Length; i++)
            {
                to_shifr[0, i] = key[i];
            }

            for (int i=0;i<key.Length-1;i++)
            {
                int ind = i;
                for (int j = i+1; j < key.Length; j++)
                {
                    if (to_shifr[0, ind] > to_shifr[0, j])
                        ind = j;
                }
                for (int j=0;j< text.Length / key.Length + 2;j++)
                {
                    char need = to_shifr[j, ind];
                    to_shifr[j, ind] = to_shifr[j, i];
                    to_shifr[j, i] = need;
                }

            }
            string shifr = "";
            for (int i = 0; i < key.Length; i++)
                for (int j = 1; j < text.Length / key.Length + 2; j++)
                    if (to_shifr[j, i] != '\0')
                        shifr = shifr + to_shifr[j, i];

            return shifr;
        }

        public static string Deshifr_Col(string text, string key)
        {
            char[,] to_shifr;
            int y;
            if (text.Length % key.Length == 0)
            {
                y = text.Length / key.Length + 2;
                to_shifr = new char[y, key.Length];
            }
            else
            {
                y = text.Length / key.Length + 3;
                to_shifr = new char[y, key.Length];
            }

            for (int i=0; i!=key.Length;i++)
            {
                to_shifr[0,i]= (char)(i);
                to_shifr[1, i] = key[i];
            }


            for (int i=0, w=0 ; i<key.Length;i++)
            {
                int ind = i;
                for(int j=ind+1;j!=key.Length;j++)
                {
                    if (to_shifr[1, ind]>to_shifr[1,j])
                    {
                        ind = j;
                    }
                }
                int nind = 0;
                while (true)
                    if (to_shifr[1, ind] == key[nind])
                    {
                        break;
                    }
                    else nind++;
                //ind
                for (int k = 2;k < y;k++)
                {
                    if ((k-1)*(nind+1)<=text.Length)
                    {
                        to_shifr[k,ind]= text[w];
                        w++;
                    }
                    else
                    {
                        break;
                    }
                }
                for (int m = 0;m<y;m++)
                {
                    char need = to_shifr[m, i];
                    to_shifr[m, i] = to_shifr[m, ind];
                    to_shifr[m, ind] = need;
                }
            }

            for(int i=0;i<key.Length;i++)
            {
                for(int j=0;j<key.Length;j++)
                {
                    if ((char)(i)==to_shifr[0,j])
                        for (int m = 0; m < y; m++)
                        {
                            char need = to_shifr[m, i];
                            to_shifr[m, i] = to_shifr[m, j];
                            to_shifr[m, j] = need;
                        }
                }
            }


            text = "";
            for(int x=2;x<y;x++)
            {
                for(int i=0;i<key.Length;i++)
                {
                        text += to_shifr[x, i];
                    
                }
            }
            return text;
        }
    }

    class Turning_grille_cipher
    {
        private static int number_squares = 5;

        public string CheckSource(string src)
        {
            if (src.Length < number_squares * number_squares)
            {
                for (int i = src.Length; i < number_squares * number_squares; i++)
                    src = src + 'k';
            }
            if (src.Length > number_squares * number_squares)
            {
                src = src.Substring(0, number_squares * number_squares);
            }
            return src;
        }

        public bool CheckKey(string key)
        {
            bool[,] mask = new bool[number_squares, number_squares];
            bool[,] grid = new bool[number_squares, number_squares];
            int ind = 0;
            if (key.Length != number_squares * number_squares)
                return false;

            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++, ind++)
                {
                    if (key[ind] == '1')
                    {
                        mask[i, j] = true;
                        grid[i, j] = true;
                    }
                    else
                    {
                        mask[i, j] = false;
                        grid[i, j] = false;
                    }
                }
            //The first rotate
            ind = 0;
            if (number_squares % 2 != 0)
                mask[number_squares / 2, number_squares / 2] = false;
            for (int i = 0; i < number_squares; i++)
                for (int j = number_squares - 1; j >= 0; j--, ind++)
                {
                    if (key[ind] == '1')
                    {
                        if (!mask[j, i])
                            mask[j, i] = true;
                        else
                            return false;
                    }
                }
            //The second rotate
            ind = 0;
            if (number_squares % 2 != 0)
                mask[number_squares / 2, number_squares / 2] = false;
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = number_squares - 1; j >= 0; j--, ind++)
                {
                    if (key[ind] == '1')
                    {
                        if (!mask[i, j])
                            mask[i, j] = true;
                        else
                            return false;
                    }
                }
            //The third rotate
            ind = 0;
            if (number_squares % 2 != 0)
                mask[number_squares / 2, number_squares / 2] = false;
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = 0; j < number_squares; j++, ind++)
                {
                    if (key[ind] == '1')
                    {
                        if (!mask[j, i])
                            mask[j, i] = true;
                        else
                            return false;
                    }
                }
            return true;
        }

        public static string Encrypt(string src, string key)
        {
            bool[,] grid = new bool[number_squares, number_squares];
            int ind = 0;
            //Initializing mask matrix
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++, ind++)
                {
                    if (key[ind] == '1')
                        grid[i, j] = true;
                    else
                        grid[i, j] = false;
                }

            char[,] cipher_matrix = new char[number_squares, number_squares];
            ind = 0;
            //The first round
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++)
                {
                    if (grid[i, j])
                    {
                        cipher_matrix[i, j] = src[ind];
                        ind++;
                    }
                }

            grid[2, 2] = false;

            //The second round
            for (int i = 0; i < number_squares; i++)
                for (int j = number_squares - 1; j >= 0; j--)
                {
                    if (grid[j, i])
                    {
                        cipher_matrix[i, number_squares - 1 - j] = src[ind];
                        ind++;
                    }
                }

            //The third round
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = number_squares - 1; j >= 0; j--)
                {
                    if (grid[i, j])
                    {
                        cipher_matrix[number_squares - 1 - i, number_squares - 1 - j] = src[ind];
                        ind++;
                    }
                }

            //The fourth round
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = 0; j < number_squares; j++)
                {
                    if (grid[j, i])
                    {
                        cipher_matrix[number_squares - 1 - i, j] = src[ind];
                        ind++;
                    }
                }

            string result = "";
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++)
                    result = result + cipher_matrix[i, j];

            return result;
        }

        public static string Decrypt(string src, string key)
        {
            char[,] cipher_matrix = new char[number_squares, number_squares];
            bool[,] grid = new bool[number_squares, number_squares];
            int ind = 0;
            string res = "";
            //Initializing mask matrix
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++, ind++)
                {
                    if (key[ind] == '1')
                        grid[i, j] = true;
                    else
                        grid[i, j] = false;
                }

            //Filling cipher_matrix with source string
            ind = 0;
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++, ind++)
                    cipher_matrix[i, j] = src[ind];

            //The first round
            for (int i = 0; i < number_squares; i++)
                for (int j = 0; j < number_squares; j++)
                {
                    if (grid[i, j])
                        res = res + cipher_matrix[i, j];
                }

            grid[2, 2] = false;

            //The second round
            for (int i = 0; i < number_squares; i++)
                for (int j = number_squares - 1; j >= 0; j--)
                {
                    if (grid[j, i])
                        res = res + cipher_matrix[i, number_squares - 1 - j];
                }

            //The third round
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = number_squares - 1; j >= 0; j--)
                {
                    if (grid[i, j])
                        res = res + cipher_matrix[number_squares - 1 - i, number_squares - 1 - j];
                }

            //The fourth round
            for (int i = number_squares - 1; i >= 0; i--)
                for (int j = 0; j < number_squares; j++)
                {
                    if (grid[j, i])
                        res = res + cipher_matrix[number_squares - 1 - i, j];
                }

            return res;
        }
    }

    public class Vij
    {
        public static string Shifr_Vij(string text,string key)
        {
            char[,] to_shifr = new char[2, text.Length];
            for (int i=0,j=0;i<text.Length;i++,j++)
            {
                if (j == key.Length)
                    j = 0;
                to_shifr[0, i] = text[i];
                to_shifr[1, i] = key[j];
            }

            string shifr = "";
            for (int i = 0; i < text.Length; i++)
                if(to_shifr[0,i]+(to_shifr[1,i]-65)<91)
                    shifr = shifr+Convert.ToChar(to_shifr[0, i] + (to_shifr[1, i] - 65));
                else
                    shifr = shifr + Convert.ToChar(to_shifr[0, i] + (to_shifr[1, i])-91);

            return shifr;
        }

        public static string Deshifr_Vij(string text, string key)
        {
            char[,] to_shifr = new char[2, text.Length];
            for (int i = 0, j = 0; i < text.Length; i++, j++)
            {
                if (j == key.Length)
                    j = 0;
                to_shifr[0, i] = text[i];
                to_shifr[1, i] = key[j];
            }
            string shifr = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (to_shifr[0, i] >= to_shifr[1, i])
                    shifr = shifr + Convert.ToChar(65 + (to_shifr[0, i] - to_shifr[1, i]));
                else
                    shifr = shifr+ Convert.ToChar(91-(to_shifr[1, i] - to_shifr[0, i]));
            }
            return shifr;
        }
    }


    public partial class UserControl1: UserControl
    {
        TextBox textBox3 = new TextBox();
        public UserControl1()
        {
            InitializeComponent();
            textBox3.Location = new System.Drawing.Point(80, 110);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(268, 10);
            textBox3.TabIndex = 6;
            this.Controls.Add(textBox3);

        }

        private void button1_Click(object sender, EventArgs e)
        {
                string text = textBox1.Text;
                //text = text.Replace(" ", "");
                //text = text.Replace("\n", "");
                text = text.ToUpper();
                


                switch (listBox1.SelectedIndex)
                {
                    case 0:
                        {
                            int key = Int32.Parse(textBox3.Text);
                            textBox2.Text=Jel.Shifr_Jel(text, key);
                            break;
                        }
                    case 1:
                        {
                            string key= textBox3.Text.ToString();
                            textBox2.Text = Col.Shifr_Col(text, key);
                            break;
                        }
                    case 2:
                        {
                            string key = textBox3.Text.ToString();
                            key = key.ToUpper();
                            textBox2.Text = Turning_grille_cipher.Encrypt(text,key);
                            break;
                        }
                    case 3:
                        {
                            string key = textBox3.Text.ToString();
                            key = key.ToUpper();
                            textBox2.Text = Vij.Shifr_Vij(text, key);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            //text = text.Replace(" ", "");
            //text = text.Replace("\n", "");
            text = text.ToUpper();



            switch (listBox1.SelectedIndex)
            {
                case 0:
                    {
                        int key = Int32.Parse(textBox3.Text);
                        textBox2.Text = Jel.Deshifr_Jel(text, key);
                        break;
                    }
                case 1:
                    {
                        string key = textBox3.Text.ToString(); ;
                        textBox2.Text = Col.Deshifr_Col(text, key);
                        break;
                    }
                case 2:
                    {
                        string key = textBox3.Text.ToString(); ;
                        key = key.ToUpper();
                        textBox2.Text = Turning_grille_cipher.Decrypt(text, key);
                        break;
                    }
                case 3:
                    {
                        string key = textBox3.Text.ToString(); ;
                        key = key.ToUpper();
                        textBox2.Text = Vij.Deshifr_Vij(text, key);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

        }
    }
}
