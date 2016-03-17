using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace changeMusicsName
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAddFile_Click(object sender, EventArgs e)
        {
            //创建浏览文件夹对话框，完成操作
            FolderBrowserDialog fole = new FolderBrowserDialog();

            //设置是否显示“新建文件夹”按钮（false：不显示；ture：显示）
            fole.ShowNewFolderButton = true;

            //设置对话框标题栏
            fole.Description = "请选择媒体文件夹";

            //如果用户选好文件夹，点击了“确定”按钮，则：
            if(fole.ShowDialog() == DialogResult.OK)
            {
                //实例化目录信息类（用到文件夹全路径）
                DirectoryInfo dir = new DirectoryInfo(fole.SelectedPath);

                textBoxFile.Text = fole.SelectedPath.ToString();

                //获取总文件数（MP3、wma）
                int nFileCount = 0;
                FileInfo[] file = dir.GetFiles();

                for(int i=0;i<file.Length;i++)
                {
                    if(file[i].Extension.ToLower().Equals(".mp3") || file[i].Extension.ToLower().Equals(".wma"))
                    {
                        nFileCount++;
                    }
                }

                //在label框中显示找到的文件数目：
                labelFileCount.Text = "歌曲总数是：" + nFileCount.ToString() + "首";

                //遍历文件夹中所有文件，将文件名添加到ListBox控件中
                foreach(FileInfo info in dir.GetFiles())
                {
                    if(info.Extension.ToLower().Equals(".mp3") || info.Extension.ToLower().Equals(".wma"))
                    {
                        listBoxFileName.Items.Add(info.FullName);
                    }
                }
            }
        }

        private void buttonChangeName_Click(object sender, EventArgs e)
        {
            DateTime StartTime = DateTime.Now;

            for(int i = 0;i<listBoxFileName.Items.Count;i++)
            {
                //去掉文件名前面的路径部分，留下文件名
                string oldName = listBoxFileName.Items[i].ToString();
                string fileName = oldName.Substring(oldName.LastIndexOf('\\') + 1);

                //获取歌手和歌曲名和文件格式
                string singer = fileName.Substring(0, fileName.IndexOf('-')).Trim();
                string musicName = fileName.Substring(fileName.IndexOf('-') + 1, (fileName.LastIndexOf('.') - fileName.IndexOf('-')) - 1).Trim();
                string format = fileName.Substring(fileName.LastIndexOf('.'));
                string newName = oldName.Substring(0, oldName.LastIndexOf('\\') + 1) + musicName + '-' + singer + format;

                //更改歌曲名称
                File.Move(oldName, newName);
            }

            //计算花费的时间
            DateTime endTime = DateTime.Now;
            TimeSpan span = endTime - StartTime;
            labelTime.Text = "花费时间为：" + span.TotalMilliseconds.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            textBoxFile.Text = "";
            listBoxFileName.Items.Clear();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            MessageBox.Show("你确定要退出？");
            this.Close();
        }
    }
}
