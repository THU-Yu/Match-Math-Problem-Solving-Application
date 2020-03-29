using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfProject
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public class Picture //七段数码管图片
    {
        public string matches;
        public Picture()
        {
            matches = String.Empty;
        }
        public void Initialize(char s)
        {
            for (int i = 0; i < 7; i ++)
            {
                matches += s;
            }
        }

        public void SetMatches(string s)
        {
            matches = String.Copy(s);
        }
        public char GetMatches(int i)
        {
            return matches[i];
        }
        public void ChangeMatchesData(int i, char s)
        {
            matches = matches.Substring(0, i) + s + matches.Substring(i + 1);
        }
    }

    public class Expression //表达式节点
    {
        string expression;
        public Picture[] picture;
        int first_sign_pos; //第一个运算符位
        int second_sign_pos; //第二个运算符位
        public int SearchLevel;
        public Object Father; //父亲表达式
        public Expression()
        {
            expression = String.Empty;
            picture = new Picture[13];
            for (int i = 0;i < 13;i ++)
            {
                picture[i] = new Picture();
            }
            first_sign_pos = 0;
            second_sign_pos = 0;
        }
        public void Initialize()
        {
            for (int i = 0; i < 13; i++)
            {
                if (i == 3 || i == 7)
                {
                    picture[i].Initialize('6');
                }
                else
                {
                    picture[i].Initialize('0');
                }
            }
            //找到第一个运算符和第二个运算符在表达式的位置
            if (expression.Contains('+'))
            {
                if (expression.IndexOf('+') < expression.IndexOf('='))
                {
                    first_sign_pos = expression.IndexOf('+');
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = expression.IndexOf('+');
                }
            }
            else if (expression.Contains('*'))
            {
                if (expression.IndexOf('*') < expression.IndexOf('='))
                {
                    first_sign_pos = expression.IndexOf('*');
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = expression.IndexOf('*');
                }
            }
            else if (expression.Contains('-'))
            {
                int sub_pos = expression.IndexOf('-');
                if (sub_pos == 0)
                {
                    sub_pos = expression.IndexOf('-', 1);
                }

                if (sub_pos < expression.IndexOf('='))
                {
                    first_sign_pos = sub_pos;
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    if (sub_pos - 1 == expression.IndexOf('-'))
                    {
                        sub_pos = expression.IndexOf('-', sub_pos + 1);
                    }
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = sub_pos;
                }
            }
        }
        public string GetExpression()
        {
            return expression;
        }
        public void SetExpression(string e) //设置新的表达式并更新符号位资讯
        {
            expression = String.Copy(e);
            if (expression.Contains('+'))
            {
                if (expression.IndexOf('+') < expression.IndexOf('='))
                {
                    first_sign_pos = expression.IndexOf('+');
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = expression.IndexOf('+');
                }
            }
            else if (expression.Contains('*'))
            {
                if (expression.IndexOf('*') < expression.IndexOf('='))
                {
                    first_sign_pos = expression.IndexOf('*');
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = expression.IndexOf('*');
                }
            }
            else if (expression.Contains('-'))
            {
                int sub_pos = expression.IndexOf('-');
                if (sub_pos == 0)
                {
                    sub_pos = expression.IndexOf('-', 1);
                }

                if (sub_pos < expression.IndexOf('='))
                {
                    first_sign_pos = sub_pos;
                    second_sign_pos = expression.IndexOf('=');
                }
                else
                {
                    if (sub_pos - 1 == expression.IndexOf('-'))
                    {
                        sub_pos = expression.IndexOf('-', sub_pos + 1);
                    }
                    first_sign_pos = expression.IndexOf('=');
                    second_sign_pos = sub_pos;
                }
            }
        }
        public void ResetExpression(Dictionary<string, char> dict) //根据图片信息重新设置表达式
        {
            string newExpression = String.Empty;
            for (int i = 0; i < 13; i ++)
            {
                if (picture[i].matches == "0000000" || picture[i].matches == "0066666")
                {
                    continue;
                }
                else
                {
                    if (dict.ContainsKey(picture[i].matches)) //判断七段数码管信息是否属于数字或符号
                    {
                        newExpression += dict[picture[i].matches];
                    }
                    else
                    {
                        newExpression += '$';
                    }
                }
            }
            expression = String.Copy(newExpression);
        }
        public Picture[] GetPicture()
        {
            return picture;
        }
        public void SetPicture(Dictionary<char, string> dict) //根据表达式来设置图片
        {
            for (int i = 0; i < 13; i ++)
            {
                if (i == 3 || i == 7)
                {
                    picture[i].SetMatches("0066666");
                }
                else
                {
                    picture[i].SetMatches("0000000");
                }
            }

            int Num1length = first_sign_pos;
            int Num2length = second_sign_pos - first_sign_pos - 1;
            int Num3length = expression.Length - second_sign_pos - 1;

            //数字靠右对齐
            switch(Num1length)
            {
                case 1:
                    picture[2].SetMatches(dict[expression[0]]);
                    break;
                case 2:
                    picture[1].SetMatches(dict[expression[0]]);
                    picture[2].SetMatches(dict[expression[1]]);
                    break;
                case 3:
                    picture[0].SetMatches(dict[expression[0]]);
                    picture[1].SetMatches(dict[expression[1]]);
                    picture[2].SetMatches(dict[expression[2]]);
                    break;
                default:
                    break;
            }
            switch (Num2length)
            {
                case 1:
                    picture[6].SetMatches(dict[expression[first_sign_pos + 1]]);
                    break;
                case 2:
                    picture[5].SetMatches(dict[expression[first_sign_pos + 1]]);
                    picture[6].SetMatches(dict[expression[first_sign_pos + 2]]);
                    break;
                case 3:
                    picture[4].SetMatches(dict[expression[first_sign_pos + 1]]);
                    picture[5].SetMatches(dict[expression[first_sign_pos + 2]]);
                    picture[6].SetMatches(dict[expression[first_sign_pos + 3]]);
                    break;
                default:
                    break;
            }
            switch (Num3length)
            {
                case 1:
                    picture[12].SetMatches(dict[expression[second_sign_pos + 1]]);
                    break;
                case 2:
                    picture[11].SetMatches(dict[expression[second_sign_pos + 1]]);
                    picture[12].SetMatches(dict[expression[second_sign_pos + 2]]);
                    break;
                case 3:
                    picture[10].SetMatches(dict[expression[second_sign_pos + 1]]);
                    picture[11].SetMatches(dict[expression[second_sign_pos + 2]]);
                    picture[12].SetMatches(dict[expression[second_sign_pos + 3]]);
                    break;
                case 4:
                    picture[9].SetMatches(dict[expression[second_sign_pos + 1]]);
                    picture[10].SetMatches(dict[expression[second_sign_pos + 2]]);
                    picture[11].SetMatches(dict[expression[second_sign_pos + 3]]);
                    picture[12].SetMatches(dict[expression[second_sign_pos + 4]]);
                    break;
                case 5:
                    picture[8].SetMatches(dict[expression[second_sign_pos + 1]]);
                    picture[9].SetMatches(dict[expression[second_sign_pos + 2]]);
                    picture[10].SetMatches(dict[expression[second_sign_pos + 3]]);
                    picture[11].SetMatches(dict[expression[second_sign_pos + 4]]);
                    picture[12].SetMatches(dict[expression[second_sign_pos + 5]]);
                    break;
                default:
                    break;
            }

            //设置运算符位
            picture[3].SetMatches(dict[expression[first_sign_pos]]);
            picture[7].SetMatches(dict[expression[second_sign_pos]]);
        }
        public void CopyPicture(Picture[] pic) //拷贝图片信息
        {
            for (int i = 0; i < 13; i ++)
            {
                picture[i].matches = String.Copy(pic[i].matches);
            }
        }
        public bool IsLegal() //判断是否为合法字符
        {
            char[] stringlist = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '*', '=' };
            if (expression == String.Empty) //字符串不为空
            {
                return false;
            }
            foreach (char c in expression) //是否属于数字和符号
            {
                for (int i = 0; i < 14; i ++)
                {
                    if (c == stringlist[i])
                    {
                        break;
                    }
                    else if (i == 13)
                    {
                        return false;
                    }
                }
            }

            if (expression[0] == '+' || expression[0] == '*' || expression[0] == '=') //首位不能以+、*、=开头
            {
                return false;
            }

            if (!expression.Contains('=')) //表达式必须存在等号
            {
                return false;
            }
            else if (expression.Split('=').Length - 1 > 1) //不能存在超过一个等号
            {
                return false;
            }
            else
            {
                int equal_pos = expression.IndexOf('=');
                if (equal_pos + 1 < expression.Length)
                {
                    if (expression[equal_pos + 1] == '+' || expression[equal_pos + 1] == '*' || expression[equal_pos + 1] == '=') //等号后不能跟+、*、=
                    {
                        return false;
                    }
                    else if (expression[equal_pos + 1] == '-')
                    {
                        if (equal_pos + 2 < expression.Length)
                        {
                            if (expression[equal_pos + 2] == '-' || expression[equal_pos + 2] == '*' || expression[equal_pos + 2] == '+')
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }

                if (expression.Contains('+'))
                {
                    int plus_pos = expression.IndexOf('+');
                    if (expression[plus_pos + 1] == '-' || expression[plus_pos + 1] == '+' || expression[plus_pos + 1] == '*' || expression[plus_pos + 1] == '=') //加号后不能跟-、+、*、=
                    {
                        return false;
                    }
                    else if (expression.Split('+').Length - 1 > 1) //不能有多个加号
                    {
                        return false;
                    }
                }
                else if (expression.Contains('*'))
                {
                    int multi_pos = expression.IndexOf('*');
                    if (expression[multi_pos + 1] == '-' || expression[multi_pos + 1] == '+' || expression[multi_pos + 1] == '*' || expression[multi_pos + 1] == '=') //乘号后不能跟-、+、*、=
                    {
                        return false;
                    }
                    else if (expression.Split('*').Length - 1 > 1) //不能有多个乘号
                    {
                        return false;
                    }
                }
                if (expression.Contains('-'))
                {
                    int sub_pos = expression.IndexOf('-');
                    if (sub_pos == 0)
                    {
                        if (expression[1] == '-' || expression[1] == '+' || expression[1] == '*' || expression[1] == '=')
                        {
                            return false;
                        }
                        sub_pos = expression.IndexOf('-', 1);
                    }
                    if (!expression.Contains('*') && !expression.Contains('+')) //减号为主要运算符的情况下
                    {
                        if (sub_pos > equal_pos)
                        {

                            if (sub_pos - 1 == equal_pos)
                            {
                                sub_pos = expression.IndexOf('-', sub_pos + 1);
                            }
                            if (sub_pos == -1) //除了第一个数的负号和等号后的负号外，应该要有一个减号运算符
                            {
                                return false;
                            }
                        }
                        if (expression[sub_pos + 1] == '-' || expression[sub_pos + 1] == '+' || expression[sub_pos + 1] == '*' || expression[sub_pos + 1] == '=') //乘号后不能跟-、+、*、=
                        {
                            return false;
                        }
                        else if (expression.Split('-').Length - 1 > 3) //减号数量不能大于三
                        {
                            return false;
                        }
                        sub_pos = -1;
                        for (int i = 0; i < expression.Split('-').Length - 1; i ++)
                        {
                            sub_pos = expression.IndexOf('-', sub_pos + 1);
                            if (expression[sub_pos + 1] == '-' || expression[sub_pos + 1] == '+' || expression[sub_pos + 1] == '*' || expression[sub_pos + 1] == '=') //乘号后不能跟-、+、*、=
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (sub_pos + 1 < expression.Length)
                        {
                            if (expression[sub_pos + 1] == '-' || expression[sub_pos + 1] == '+' || expression[sub_pos + 1] == '*' || expression[sub_pos + 1] == '=')
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }

    public partial class MainWindow : Window
    {
        string Display_string = String.Empty;
        Picture[] matches_picture = new Picture[13];
        Dictionary<char, string> num_dict = new Dictionary<char, string>();
        Dictionary<string, char> picture_dict = new Dictionary<string, char>();
        Expression mainexpression = new Expression();
        Image[,] images;
        Stack OpenStack = new Stack();
        Stack CloseStack = new Stack();
        Stack AnswerStack = new Stack();

        public MainWindow()
        {
            InitializeComponent();
            //timer.Start();
            this.Difficulty.Items.Add("（空）");
            this.Difficulty.Items.Add("简单");
            this.Difficulty.Items.Add("中等");
            this.Difficulty.Items.Add("困难");
            this.Question.IsEnabled = false;
            images = new Image[13, 7] { { this.image00, this.image01, this.image02, this.image03, this.image04, this.image05, this.image06 },
                                        { this.image10, this.image11, this.image12, this.image13, this.image14, this.image15, this.image16 },
                                        { this.image20, this.image21, this.image22, this.image23, this.image24, this.image25, this.image26 },
                                        { this.image30, this.image31, new Image(), new Image(), new Image(), new Image(), new Image()},
                                        { this.image40, this.image41, this.image42, this.image43, this.image44, this.image45, this.image46 },
                                        { this.image50, this.image51, this.image52, this.image53, this.image54, this.image55, this.image56 },
                                        { this.image60, this.image61, this.image62, this.image63, this.image64, this.image65, this.image66 },
                                        { this.image70, this.image71, new Image(), new Image(), new Image(), new Image(), new Image()},
                                        { this.image80, this.image81, this.image82, this.image83, this.image84, this.image85, this.image86 },
                                        { this.image90, this.image91, this.image92, this.image93, this.image94, this.image95, this.image96 },
                                        { this.image100, this.image101, this.image102, this.image103, this.image104, this.image105, this.image106 },
                                        { this.image110, this.image111, this.image112, this.image113, this.image114, this.image115, this.image116 },
                                        { this.image120, this.image121, this.image122, this.image123, this.image124, this.image125, this.image126 }};
            num_dict.Add('0', "1110111");
            num_dict.Add('1', "0010010");
            num_dict.Add('2', "0111101");
            num_dict.Add('3', "0111011");
            num_dict.Add('4', "1011010");
            num_dict.Add('5', "1101011");
            num_dict.Add('6', "1101111");
            num_dict.Add('7', "0110010");
            num_dict.Add('8', "1111111");
            num_dict.Add('9', "1111011");
            num_dict.Add('+', "1126666");
            num_dict.Add('-', "0136666");
            num_dict.Add('*', "1146666");
            num_dict.Add('=', "1156666");

            picture_dict.Add("1110111", '0');
            picture_dict.Add("0010010", '1');
            picture_dict.Add("0111101", '2');
            picture_dict.Add("0111011", '3');
            picture_dict.Add("1011010", '4');
            picture_dict.Add("1101011", '5');
            picture_dict.Add("1101111", '6');
            picture_dict.Add("0110010", '7');
            picture_dict.Add("1111111", '8');
            picture_dict.Add("1111011", '9');
            picture_dict.Add("1126666", '+');
            picture_dict.Add("0001000", '-');
            picture_dict.Add("0136666", '-');
            picture_dict.Add("1146666", '*');
            picture_dict.Add("1156666", '=');
            picture_dict.Add("0066666", ' ');

            this.Num.Items.Add('1');
            this.Num.Items.Add('2');
        }

        public static void Display_Expression(Expression ex, Image[,] images) //显示表达式
        {
            Picture[] pic = ex.GetPicture();
            //火柴区初始化
            BitmapImage img = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/火柴黑白.PNG", UriKind.RelativeOrAbsolute));
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    images[i, j].Source = img;
                    images[i, j].Visibility = Visibility.Hidden;
                }
            }
            TransformGroup tg = images[3, 0].RenderTransform as TransformGroup;
            var tgnew = tg.CloneCurrentValue();
            if (tgnew != null)
            {
                RotateTransform rt = tgnew.Children[2] as RotateTransform;
                TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                images[3, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle = 0;
                tltf.Y = 0;
            }
            images[3, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
            tg = images[3, 1].RenderTransform as TransformGroup;
            tgnew = tg.CloneCurrentValue();
            if (tgnew != null)
            {
                RotateTransform rt = tgnew.Children[2] as RotateTransform;
                TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                images[3, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle = 90;
                tltf.Y = 0;
            }
            images[3, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
            tg = images[7, 0].RenderTransform as TransformGroup;
            tgnew = tg.CloneCurrentValue();
            if (tgnew != null)
            {
                RotateTransform rt = tgnew.Children[2] as RotateTransform;
                TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                images[7, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle = 90;
                tltf.Y = 0;
            }
            images[7, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
            tg = images[7, 1].RenderTransform as TransformGroup;
            tgnew = tg.CloneCurrentValue();
            if (tgnew != null)
            {
                RotateTransform rt = tgnew.Children[2] as RotateTransform;
                TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                images[7, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                rt.Angle = 90;
                tltf.Y = 0;
            }
            images[7, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

            //开始设置图片
            img = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/火柴.PNG", UriKind.RelativeOrAbsolute));
            for (int i = 0; i < 13; i++)
            {
                if (i == 3 || i == 7)
                {
                    if (pic[i].GetMatches(2) == pic[i].GetMatches(3) && pic[i].GetMatches(2) != '6')
                    {
                        switch(pic[i].GetMatches(2))
                        {
                            case '2':
                                if (pic[i].GetMatches(0) == '1')
                                {
                                    images[i, 0].Source = img;
                                    images[i, 0].Visibility = Visibility.Visible;
                                }
                                if (pic[i].GetMatches(1) == '1')
                                {
                                    images[i, 1].Source = img;
                                    images[i, 1].Visibility = Visibility.Visible;
                                }
                                tg = images[i, 0].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = -45;
                                    if (i == 7)
                                    {
                                        tltf.Y = 5;
                                    }
                                }
                                images[i, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                                    if (i == 7)
                                    {
                                        tltf.Y = -22;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                            case '4':
                                if (pic[i].GetMatches(0) == '1')
                                {
                                    images[i, 0].Source = img;
                                    images[i, 0].Visibility = Visibility.Visible;
                                }
                                if (pic[i].GetMatches(1) == '1')
                                {
                                    images[i, 1].Source = img;
                                    images[i, 1].Visibility = Visibility.Visible;
                                }
                                tg = images[i, 0].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 0;
                                    if (i == 7)
                                    {
                                        tltf.Y = 5;
                                    }
                                }
                                images[i, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 45;
                                    if (i == 7)
                                    {
                                        tltf.Y = -22;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                        }
                    }
                    else
                    {
                        switch (pic[i].GetMatches(2))
                        {
                            case '2':
                                if (pic[i].GetMatches(0) == '1')
                                {
                                    images[i, 0].Source = img;
                                    images[i, 0].Visibility = Visibility.Visible;
                                }
                                if (pic[i].GetMatches(1) == '1')
                                {
                                    images[i, 1].Source = img;
                                    images[i, 1].Visibility = Visibility.Visible;
                                }
                                tg = images[i, 0].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 0;
                                    if (i == 7)
                                    {
                                        tltf.Y = 5;
                                    }
                                }
                                images[i, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                                    if (i == 7)
                                    {
                                        tltf.Y = -22;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                            case '3':
                                images[i, 1].Source = img;
                                images[i, 1].Visibility = Visibility.Visible;
                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    if (i == 7)
                                    {
                                        tltf.Y = -22;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                            case '4':
                                if (pic[i].GetMatches(0) == '1')
                                {
                                    images[i, 0].Source = img;
                                    images[i, 0].Visibility = Visibility.Visible;
                                }
                                if (pic[i].GetMatches(1) == '1')
                                {
                                    images[i, 1].Source = img;
                                    images[i, 1].Visibility = Visibility.Visible;
                                }
                                tg = images[i, 0].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle -= 45;
                                    if (i == 7)
                                    {
                                        tltf.Y = 10;
                                    }
                                }
                                images[i, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                                    if (i == 7)
                                    {
                                        rt.Angle = -45;
                                        tltf.Y = -15;
                                    }
                                    else
                                    {
                                        rt.Angle -= 45;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                            case '5':
                                images[i, 0].Source = img;
                                images[i, 1].Source = img;
                                images[i, 0].Visibility = Visibility.Visible;
                                images[i, 1].Visibility = Visibility.Visible;
                                tg = images[i, 0].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 0].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 90;
                                    if (i == 3)
                                    {
                                        tltf.Y = -10;
                                    }
                                }
                                images[i, 0].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性

                                tg = images[i, 1].RenderTransform as TransformGroup;
                                tgnew = tg.CloneCurrentValue();
                                if (tgnew != null)
                                {
                                    RotateTransform rt = tgnew.Children[2] as RotateTransform;
                                    TranslateTransform tltf = tgnew.Children[3] as TranslateTransform;
                                    images[i, 1].RenderTransformOrigin = new Point(0.5, 0.5);
                                    rt.Angle = 90;
                                    if (i == 3)
                                    {
                                        tltf.Y = 20;
                                    }
                                }
                                images[i, 1].RenderTransform = tgnew;// 重新给图像赋值Transform变换属性
                                break;
                            default:
                                break;
                        }
                    }
                }
                else if (pic[i].GetMatches(2) == '3')
                {
                    images[i, 3].Source = img;
                    images[i, 3].Visibility = Visibility.Visible;
                }
                else
                {
                    for (int j = 0; j < 7; j++)
                     {
                        if (pic[i].GetMatches(j) == '1')
                        {
                            images[i, j].Source = img;
                            images[i, j].Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        public static bool Check(Expression ex)
        {
            string Answer = ex.GetExpression();
            if (!ex.IsLegal()) //表达式不合法
            {
                return false;
            }
            else
            {
                if (Answer.Contains('+'))
                {
                    int plus_pos = Answer.IndexOf('+');
                    int equal_pos = Answer.IndexOf('=');
                    if (plus_pos < equal_pos)
                    {
                        return (Convert.ToInt32(Answer.Substring(0, plus_pos)) + Convert.ToInt32(Answer.Substring(plus_pos + 1, equal_pos - plus_pos - 1)) == Convert.ToInt32(Answer.Substring(equal_pos + 1)));
                    }
                    else
                    {
                        return (Convert.ToInt32(Answer.Substring(equal_pos + 1, plus_pos - equal_pos - 1)) + Convert.ToInt32(Answer.Substring(plus_pos + 1)) == Convert.ToInt32(Answer.Substring(0, equal_pos)));
                    }
                }
                else if (Answer.Contains('*'))
                {
                    int multi_pos = Answer.IndexOf('*');
                    int equal_pos = Answer.IndexOf('=');
                    if (multi_pos < equal_pos)
                    {
                        return (Convert.ToInt32(Answer.Substring(0, multi_pos)) * Convert.ToInt32(Answer.Substring(multi_pos + 1, equal_pos - multi_pos - 1)) == Convert.ToInt32(Answer.Substring(equal_pos + 1)));
                    }
                    else
                    {
                        return (Convert.ToInt32(Answer.Substring(equal_pos + 1, multi_pos - equal_pos - 1)) * Convert.ToInt32(Answer.Substring(multi_pos + 1)) == Convert.ToInt32(Answer.Substring(0, equal_pos)));
                    }
                }
                else if (Answer.Contains('-'))
                {
                    int sub_pos = Answer.IndexOf('-');
                    if (sub_pos == 0)
                    {
                        sub_pos = Answer.IndexOf('-', 1);
                    }
                    int equal_pos = Answer.IndexOf('=');
                    if (sub_pos < equal_pos)
                    {
                        return (Convert.ToInt32(Answer.Substring(0, sub_pos)) - Convert.ToInt32(Answer.Substring(sub_pos + 1, equal_pos - sub_pos - 1)) == Convert.ToInt32(Answer.Substring(equal_pos + 1)));
                    }
                    else
                    {
                        if (sub_pos - 1 == equal_pos)
                        {
                            sub_pos = Answer.IndexOf('-', sub_pos + 1);
                        }
                        return (Convert.ToInt32(Answer.Substring(equal_pos + 1, sub_pos - equal_pos - 1)) - Convert.ToInt32(Answer.Substring(sub_pos + 1)) == Convert.ToInt32(Answer.Substring(0, equal_pos)));
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public static void Createnewnode(int i, int j, int type, char sign, Dictionary<char, string> dict, Dictionary<string, char> pic_dict, ref Expression TempOfExpression, ref Stack OpenStack)
        {

            Expression CreateNewNode;
            switch (type)
            {
                case 0://符号位减一根
                    CreateNewNode = new Expression();
                    CreateNewNode.SetExpression(TempOfExpression.GetExpression());
                    CreateNewNode.CopyPicture(TempOfExpression.picture);
                    CreateNewNode.picture[i].ChangeMatchesData(j, '0');
                    //以下是符号减变换的特殊情况
                    if (CreateNewNode.picture[i].matches[0] == '0' && CreateNewNode.picture[i].matches[1] == '0')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, '6');
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '0' && CreateNewNode.picture[i].matches[1] == '1' && CreateNewNode.picture[i].matches[2] == '5')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, '3');
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '0' && CreateNewNode.picture[i].matches[1] == '1' && CreateNewNode.picture[i].matches[2] == '2')
                    {
                        if (CreateNewNode.picture[i].matches[3] == '6')
                        {
                            CreateNewNode.picture[i].ChangeMatchesData(2, '3');
                        }
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '1' && CreateNewNode.picture[i].matches[1] == '0' && CreateNewNode.picture[i].matches[2] == '2')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(3, '4');
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '1' && CreateNewNode.picture[i].matches[1] == '0' && CreateNewNode.picture[i].matches[2] == '4')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(3, '2');
                    }
                    CreateNewNode.ResetExpression(pic_dict);
                    CreateNewNode.SearchLevel = TempOfExpression.SearchLevel + 1;
                    CreateNewNode.Father = TempOfExpression;
                    OpenStack.Push(CreateNewNode);
                    break;
                case 1://数字位减一根
                    CreateNewNode = new Expression();
                    CreateNewNode.SetExpression(TempOfExpression.GetExpression());
                    CreateNewNode.CopyPicture(TempOfExpression.picture);
                    CreateNewNode.picture[i].ChangeMatchesData(j, '0');
                    if (CreateNewNode.picture[i].matches[0] == '0' && CreateNewNode.picture[i].matches[1] == '0' && CreateNewNode.picture[i].matches[2] > '1')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, '6');
                    }
                    CreateNewNode.ResetExpression(pic_dict);
                    CreateNewNode.SearchLevel = TempOfExpression.SearchLevel + 1;
                    CreateNewNode.Father = TempOfExpression;
                    OpenStack.Push(CreateNewNode);
                    break;
                case 2://符号位加一根
                    CreateNewNode = new Expression();
                    CreateNewNode.SetExpression(TempOfExpression.GetExpression());
                    CreateNewNode.CopyPicture(TempOfExpression.picture);
                    CreateNewNode.picture[i].ChangeMatchesData(j, '1');
                    //以下是符号加变换的特殊情况
                    if (CreateNewNode.picture[i].matches[0] == '1' && CreateNewNode.picture[i].matches[1] == '0' && CreateNewNode.picture[i].matches[2] == '6')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, '4');
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '0' && CreateNewNode.picture[i].matches[1] == '1' && CreateNewNode.picture[i].matches[2] == '6')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, '3');
                    }
                    else if (CreateNewNode.picture[i].matches[0] == '1' && CreateNewNode.picture[i].matches[1] == '1' && CreateNewNode.picture[i].matches[2] == '3')
                    {
                        CreateNewNode.picture[i].ChangeMatchesData(2, sign);
                    }
                    else if (CreateNewNode.picture[i].matches[2] == '2' || CreateNewNode.picture[i].matches[2] == '4')
                    {
                        if (CreateNewNode.picture[i].matches[3] != '6')
                        {
                            if (CreateNewNode.picture[i].matches[2] != CreateNewNode.picture[i].matches[3])
                            {
                                CreateNewNode.picture[i].ChangeMatchesData(2, CreateNewNode.picture[i].matches[3]);
                            }
                            else
                            {
                                CreateNewNode.picture[i].ChangeMatchesData(3, '6');
                            }
                        }
                    }
                    CreateNewNode.ResetExpression(pic_dict);
                    CreateNewNode.SearchLevel = TempOfExpression.SearchLevel + 1;
                    CreateNewNode.Father = TempOfExpression;
                    OpenStack.Push(CreateNewNode);
                    break;
                case 3://数字位加一根
                    CreateNewNode = new Expression();
                    CreateNewNode.SetExpression(TempOfExpression.GetExpression());
                    CreateNewNode.CopyPicture(TempOfExpression.picture);
                    CreateNewNode.picture[i].ChangeMatchesData(j, '1');
                    CreateNewNode.ResetExpression(pic_dict);
                    CreateNewNode.SearchLevel = TempOfExpression.SearchLevel + 1;
                    CreateNewNode.Father = TempOfExpression;
                    OpenStack.Push(CreateNewNode);
                    break;
            }
        }
        public static Stack DFS(int deapth, string question, Dictionary<char, string> dict, Dictionary<string, char> pic_dict, Stack OpenStack, ref Stack CloseStack, ref Stack AnswerStack)
        {
            //初始化起始节点
            Expression Ques = new Expression();
            Expression TempOfExpression;
            Ques.SetExpression(question);
            Ques.SetPicture(dict);
            Ques.SearchLevel = 0;
            OpenStack.Push(Ques);

            //DFS开始
            while (OpenStack.Count != 0)
            {
                TempOfExpression = (Expression)OpenStack.Pop();
                Picture[] pic = TempOfExpression.GetPicture();

                //判断当前搜索层度，偶数层则创建减一根节点，奇数层创建加一根节点
                if (TempOfExpression.SearchLevel % 2 == 0)
                {
                    //检查是否为正确表达式
                    if (Check(TempOfExpression))
                    {
                        AnswerStack.Push(TempOfExpression);
                    }

                    //如果到达搜索最大深度限制，不再扩展节点
                    if (TempOfExpression.SearchLevel == deapth)
                    {
                        CloseStack.Push(TempOfExpression);
                    }
                    else
                    {
                        for (int i = 0; i < 13; i ++)
                        {
                            if (i == 3 || i == 7)
                            {
                                for (int j = 0; j < 2; j ++)
                                {
                                    if (pic[i].matches[j] == '1')
                                    {
                                        Createnewnode(i, j, 0, '0', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < 7; j ++)
                                {
                                    if (pic[i].matches[j] == '1')
                                    {
                                        Createnewnode(i, j, 1, '0', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                    }
                                }
                            }
                        }
                        CloseStack.Push(TempOfExpression);
                    }
                }
                else
                {
                    for (int i = 0; i < 13; i++)
                    {
                        //如果当前为空数字且后一位也是空数字或空符号，不允许对这个位置增加火柴
                        if (i < 12)
                        {
                            if (pic[i].matches == "0000000" && (pic[i + 1].matches == "0000000" || pic[i + 1].matches[6] == '6'))
                            {
                                continue;
                            }
                        }
                        if (i == 3 || i == 7)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                if (pic[i].matches[j] == '0')
                                {
                                    if (pic[i].matches == "0136666")
                                    {
                                        //减号加一根可以变为加号或等号
                                        Createnewnode(i, j, 2, '2', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                        Createnewnode(i, j, 2, '5', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                    }
                                    else
                                    {
                                        Createnewnode(i, j, 2, '0', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < 7; j++)
                            {
                                if (pic[i].matches[j] == '0')
                                {
                                    Createnewnode(i, j, 3, '0', dict, pic_dict, ref TempOfExpression, ref OpenStack);
                                }
                            }
                        }
                    }
                    CloseStack.Push(TempOfExpression);
                }
            }
            return AnswerStack;
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            WelcomeWindow welcome = new WelcomeWindow();
            welcome.Show();
        }

        private void Difficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //清空答案
            this.Answer.Items.Clear();
            this.outputtext.Text = String.Empty;

            //清除题目选择
            this.Question.SelectedIndex = -1;
            if (this.Difficulty.SelectedIndex == 0)
            {                
                this.inputtext.IsEnabled = true;
                this.inputtext.Text = String.Empty;
                this.Difficulty.SelectedIndex = -1;
                this.Question.Items.Clear();
            }
            else if (this.Difficulty.SelectedIndex == 1)
            {
                this.inputtext.IsEnabled = false;
                this.Question.IsEnabled = true;
                this.Question.Items.Clear();
                this.Question.Items.Add("（空）");
                this.Question.Items.Add("6+4=4");
                this.Question.Items.Add("2+3=6");
                this.Question.Items.Add("1-2=5");
                this.Question.Items.Add("7+2=1");
                this.Question.Items.Add("6+8=17");
                this.Question.Items.Add("-1+2=9");
                this.Question.Items.Add("3-2=0");
                this.Question.Items.Add("9-8=3");
                this.Question.Items.Add("7-2=-3");
                this.Question.Items.Add("-1-2=-5");
            }
            else if (this.Difficulty.SelectedIndex == 2)
            {
                this.inputtext.IsEnabled = false;
                this.Question.IsEnabled = true;
                this.Question.Items.Clear();
                this.Question.Items.Add("（空）");
                this.Question.Items.Add("66+4=73");
                this.Question.Items.Add("3*3=6");
                this.Question.Items.Add("29-30=-4");
                this.Question.Items.Add("71-67=-76");
                this.Question.Items.Add("7*8=53");
                this.Question.Items.Add("48-10=29");
                this.Question.Items.Add("9*8=48");
                this.Question.Items.Add("7*7=1");
                this.Question.Items.Add("4*3=8");
                this.Question.Items.Add("5*5=38");
            }
            else if (this.Difficulty.SelectedIndex == 3)
            {
                this.inputtext.IsEnabled = false;
                this.Question.IsEnabled = true;
                this.Question.Items.Clear();
                this.Question.Items.Add("（空）");
                this.Question.Items.Add("99*19=1584");
                this.Question.Items.Add("17*18=190");
                this.Question.Items.Add("20*4=120");
                this.Question.Items.Add("98*10=300");
                this.Question.Items.Add("25*25=575");
            }
        }

        private void Question_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //清空答案
            this.outputtext.Text = String.Empty;
            this.Answer.Items.Clear();
            if (this.Question.SelectedIndex <= 0)
            {
                if (this.Question.SelectedIndex == 0)
                {
                    this.Question.SelectedIndex = -1;
                }
                this.inputtext.Text = String.Empty;
                this.inputtext.IsEnabled = true;
            }
            else
            {
                this.inputtext.Text = this.Question.SelectedItem.ToString();
            }
        }

        private void Display_question_Click(object sender, RoutedEventArgs e)
        {
            mainexpression.SetExpression(this.inputtext.Text);
            if (!mainexpression.IsLegal())
            {
                MessageBox.Show("输入不合法，请重新输入！");
            }
            else
            {
                mainexpression.SetPicture(num_dict);
                Display_Expression(mainexpression, images);
            }            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            //初始化环境
            this.Answer.Items.Clear();
            this.outputtext.Text = String.Empty;
            while (CloseStack.Count != 0)
            {
                CloseStack.Pop();
            }
            while (AnswerStack.Count != 0)
            {
                AnswerStack.Pop();
            }
            bool SameAnswer = false;
            mainexpression.SetExpression(this.inputtext.Text);
            if (!mainexpression.IsLegal())
            {
                MessageBox.Show("输入不合法，请重新输入！");
            }
            else
            {
                mainexpression.SetPicture(num_dict);
                Display_Expression(mainexpression, images);

                //设置最大搜索深度
                int deapth = 2;
                if (this.Num.SelectedIndex < 0)
                {
                    deapth = 2;
                }
                else
                {
                    deapth += this.Num.SelectedIndex * 2;
                }

                //开始DFS
                AnswerStack = DFS(deapth, mainexpression.GetExpression(), num_dict, picture_dict, OpenStack, ref CloseStack, ref AnswerStack);

                //设置搜索到的答案
                string answer = String.Empty;
                if (AnswerStack.Count == 0)
                {
                    MessageBox.Show("无解！");
                }
                else
                {
                    for (int i = 0; i < AnswerStack.Count; i ++)
                    {
                        Expression Ex = (Expression)AnswerStack.ToArray()[i];

                        //判断表达答案是否和题目一样
                        if (Ex.GetExpression() == mainexpression.GetExpression())
                        {
                            SameAnswer = true;
                        }
                        else
                        {
                            SameAnswer = false;
                        }

                        //判断答案是否重复
                        if (answer != String.Empty)
                        {
                            foreach (string s in answer.Split('\n'))
                            {
                                if (Ex.GetExpression() == s)
                                {
                                    SameAnswer = true;
                                    break;
                                }
                            }
                        }
                        if (!SameAnswer)
                        {
                            answer += Ex.GetExpression();
                            answer += '\n';
                        }
                    }
                    this.outputtext.Text = answer;
                    if (answer == String.Empty)
                    {

                        MessageBox.Show("除原题等式外无其他解！");
                    }
                    else
                    {
                        MessageBox.Show("搜索成功！");
                        this.DisplayAnswer.IsEnabled = true;
                    }
                }

                //添加答案演示选单
                foreach (string s in answer.Split('\n'))
                {
                    if (s == String.Empty)
                    {
                        break;
                    }
                    this.Answer.Items.Add(s);
                }
            }
            this.inputtext.IsEnabled = true;
        }

        private void Answer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Display_Expression(mainexpression, images);
        }

        private void Inputtext_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.outputtext.Text = String.Empty;
            this.Answer.Items.Clear();
            this.DisplayAnswer.IsEnabled = false;
            this.Search.IsEnabled = true;
            if (this.inputtext.Text == String.Empty)
            {
                this.Search.IsEnabled = false;
            }
        }
        //DoEvent()、ExitFram()和Delay()三个函数用来控制演示间隔
        public void DoEvent() 
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }
        public object ExitFrame(object f)
        {
            ((DispatcherFrame) f).Continue = false;
            return null;
        }
        public bool Delay(int delayTime)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = spand.Seconds;
                DoEvent();
            }
            while (s < delayTime);
            return true;
        }
        private void DisplayAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (this.Answer.SelectedIndex == -1)
            {
                MessageBox.Show("请选择要演示的解答！");
            }
            else
            {
                Expression Ex;
                int minSearchLevel = 100;
                int pos = 0;

                //找到移动次数最少的解法
                for (int i = 0; i < AnswerStack.Count; i++)
                {
                    Ex = (Expression)AnswerStack.ToArray()[i];
                    if (Ex.GetExpression() == (string)this.Answer.SelectedItem)
                    {
                        if (Ex.SearchLevel < minSearchLevel)
                        {
                            minSearchLevel = Ex.SearchLevel;
                            pos = i;
                        }
                    }
                }
                Ex = (Expression)AnswerStack.ToArray()[pos];
                Stack st = new Stack();
                while (Ex.Father != null)
                {
                    st.Push(Ex);
                    Ex = (Expression)Ex.Father;
                }
                st.Push(Ex);
                while (st.Count != 0)
                {
                    Ex = (Expression)st.Pop();
                    Display_Expression(Ex, images);
                    Delay(1);
                }
                MessageBox.Show("演示完成！");
            }
        }

    }
}
