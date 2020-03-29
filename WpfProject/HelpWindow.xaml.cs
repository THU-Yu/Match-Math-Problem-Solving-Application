using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfProject
{
    /// <summary>
    /// HelpWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            this.HelpInfo.Text = "欢迎使用本解题系统，本解题系统由YHChen独立完成！\n" +
                                 "使用步骤：\n" +
                                 "1.在开始界面点击开始按钮进入解题画面；点击离开结束程序。\n" +
                                 "2.解题画面中，上方图片区展示了数学表达式的火柴表示；下方为用户操作区，一共分为以下几个功能：\n" +
                                 "  (1)用户输入区：本区支持用户输入待求解的表达式，本系统可支援两位数内的加减乘运算。如果使用题库功能，输入区会被锁死，只需消除选择题库内容即可解锁。\n" +
                                 "  (2)输出显示区：本区会显示所有可能的解，以数学表达式的形式显示，此功能不支援用户修改。\n" +
                                 "  (3)按钮区：按钮的功能如叙述一样，其中显示解答是显示搜索出来的解答过程，需要选择想要演示的解答。\n" +
                                 "  (4)题库区：题库区可以选择试题难度和对应的题目，各难度试题会随用户选择而更新，如果想要消除选择，只需要选择“（空）”选项即可。\n" +
                                 "  (5)题目限制区：可以提供用户选择最多移动一根火柴或者最多移动两根火柴，若用户没有选择，则默认移动一根。\n" +
                                 "  (6)解答选择区：可以提供用户选择想演示的解答选项。\n" +
                                 "3.求解一根火柴的情况可以近乎瞬间得到答案，在求解两根火柴的情况就需要等待一段时间，会根据数字的多寡而改变搜索时长，搜索时长最久不会超过30秒。\n" +
                                 "\n\nVersion1.0.0";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
