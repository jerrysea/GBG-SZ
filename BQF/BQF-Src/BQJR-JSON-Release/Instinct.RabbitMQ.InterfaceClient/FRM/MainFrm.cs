using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using Instinct.RabbitMQ.InterfaceClient.Util;
using System.Text;
using System.Xml;

namespace Instinct.RabbitMQ.InterfaceClient.Frm
{
    public partial class MainFrm : Form
    {
        private bool isxml;
        private DAL.MQClient client;
       
        #region 初始化
        public MainFrm()
        {
            InitializeComponent();
            Init();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.comboBox1.DataSource = getMethods();
            this.isxml = false;
            this.toolStripComboBox1.SelectedIndex = 1;
        }
        #endregion

        #region 配置
        private List<String> getMethods()
        {
            List<String> alist = new List<String>();
            string appstr = Util.GlobalParameters.MethodNames;
            if (appstr != null)
            {
                alist.AddRange(appstr.Split('|'));
            }
            return alist;
        }
        #endregion

        #region 事件
        /// <summary>
        /// fraud check 调用 单笔调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string inputtext = string.Empty;
            string methodname = string.Empty;
            string outputmsg = string.Empty;

            inputtext = getInputText();
            if (string.IsNullOrEmpty(inputtext))
            {
                Util.MessageUtil.ShowWarning("输入值为空！！！\n 请在输入框输入正确的申请记录。");
                this.richTextBox1.Focus();
                return;
            }
            methodname = getMethodName();
            if (string.IsNullOrEmpty(methodname))
            {
                Util.MessageUtil.ShowWarning("函数为空！！！\n 请选择一个函数。");
                this.comboBox1.Focus();
                return;
            }
            try
            {
                this.client.PressInQueue(inputtext, methodname);

                Util.MessageUtil.ShowWarning("成功入列！！！");
            }
            catch (Exception ex)
            {
                Util.MessageUtil.ShowError("错误信息:\n" + ex.ToString());
                return;
            }
        }

        /// <summary>
        /// 设定richtext 格式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cmbvalue = this.comboBox1.Text;
            if (cmbvalue != null && cmbvalue.ToUpper().IndexOf("XML") > -1)
            {
                this.richTextBox1.ConfigurationManager.Language = "xml";
                this.richTextBox2.ConfigurationManager.Language = "xml";
                this.isxml = true;
            }
            else
            {
                this.richTextBox1.ConfigurationManager.Language = "default";
                this.richTextBox2.ConfigurationManager.Language = "default";
                this.isxml = false;
            }
        }
        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Util.MessageUtil.ConfirmYesNo("你真的要退出吗？"))
            {
                e.Cancel = false;
                //this.client.Exit();                
            }
            else
            {
                e.Cancel = true;
            }
        }
        /// <summary>
        /// 关闭主线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
        /// <summary>
        /// 开启监视
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripButton1.Text == "开启")
            {
                toolStripButton1.Text = "关闭";
                this.groupBox2.Enabled = true;
                if (toolStripComboBox1.Text.Trim() == "同步")
                {
                    Util.GlobalParameters.MqSynchronization = true;
                }
                else
                {
                    Util.GlobalParameters.MqSynchronization = false;
                }
                this.client = new DAL.MQClient(this.richTextBox2, AppendRichText);
                this.client.Listening();
            }
            else
            {
                toolStripButton1.Text = "开启";
                try
                {
                    this.client.Exit();
                }
                catch (Exception ex)
                { }
                this.groupBox2.Enabled = false;
                this.client = null;
            }
        }
        /// <summary>
        /// 转Json
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string jsonstr = SCM.RabbitMQClient.Common.JsonXmlObjectParser.XmlToJson(this.richTextBox1.Text);
            this.richTextBox2.Text = jsonstr;
        }
        /// <summary>
        /// 转XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string xmlstr = SCM.RabbitMQClient.Common.JsonXmlObjectParser.JsonToXml(this.richTextBox1.Text);
            this.richTextBox2.Text = xmlstr;
        }
        /// <summary>
        /// 验证XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string errmsg = string.Empty;
            string content = this.richTextBox1.Text;
            if (string.IsNullOrEmpty(content))
            {
                Util.MessageUtil.ShowWarning("输入内容不能为空！");
                return;
            }
            else
            {
                try
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(content);
                    Util.MessageUtil.ShowTips("内容验证正确！");

                }
                catch (Exception ex)
                {
                    Util.MessageUtil.ShowWarning("验证失败！\n" + ex.ToString());
                    return;
                }
            }
        }
        #endregion

        #region  参数化值
        private string getInputText()
        {
            return this.richTextBox1.Text.ToString();
        }        
        private string getMethodName()
        {
            return this.comboBox1.Text;
        }
        #endregion                      

        #region 函数
        private void AppendRichText(ScintillaNET.Scintilla text, string msg)
        {
            if (text.InvokeRequired)
            {
                DAL.MQClient._AppendRichText _set = new DAL.MQClient._AppendRichText(delegate (ScintillaNET.Scintilla _text, string _msg)
                {
                    _text.Text += (_msg + "\n");
                });
                this.Invoke(_set, text, msg);
            }
            else
            {
                text.Text += (msg + "\n");
            }
        }
        #endregion
    }
}
