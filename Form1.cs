namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private delegate void setTxtContent(string msg);
        Dictionary<int, List<int>> dicAll;
        Random ran;
        object obj = new object();  
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dicAll = new Dictionary<int, List<int>>();
            ran = new Random();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetAllData();
            GetTakePoker();
        }
        /// <summary>
        /// ��ʼ��357��Ϸ���ݣ�Ҳ����ͨ��������ӽӿڵķ�ʽȥ��ȡ���ݣ��������ݾͿ�����չ��
        /// </summary>
        /// <returns></returns>
        private void GetAllData()
        {
            dicAll?.Clear();
            dicAll.Add(1, new List<int>() { 1, 2, 3 });
            dicAll.Add(2, new List<int>() { 4, 5, 6, 7, 8 });
            dicAll.Add(3, new List<int>() { 9, 10, 11, 12, 12, 14, 15 });
        }

        /// <summary>
        /// �û��
        /// </summary>
        private async void GetTakePoker()
        {
            int userSelectedRow = ran.Next(1, dicAll.Count + 1);
            if (dicAll[userSelectedRow] == null || dicAll[userSelectedRow].Count == 0)
            {
                ShowLog($"���ṩ��Դ�����쳣");
                return;
            }
            while (dicAll[userSelectedRow].Count > 0)
            {
                var taskList = new List<Task<bool>> { userOneTake(userSelectedRow), userTwoTake(userSelectedRow) }; //���߳� ����ʱ������
                await Task.WhenAll(taskList);
            }
        }
        /// <summary>
        /// �û�һ�û��
        /// </summary>
        /// <param name="userOneSelectedRow"></param>
        /// <returns></returns>
        private Task<bool> userOneTake(int userOneSelectedRow)
        {
            return Task.Run(() =>
            {
                try
                {
                    GetPokerCaculate(userOneSelectedRow, Operation.userOne);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }
        /// <summary>
        /// �û����û��
        /// </summary>
        /// <param name="userOneSelectedRow"></param>
        /// <returns></returns>
        private Task<bool> userTwoTake(int userOneSelectedRow)
        {
            return Task.Run(() =>
            {
                try
                {
                    GetPokerCaculate(userOneSelectedRow, Operation.userTwo);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="operation"></param>
        private void GetPokerCaculate(int rowIndex, Operation operation)
        {
            lock (obj)
            {
                List<int> lstSelectedRowData = dicAll[rowIndex];
                if (lstSelectedRowData.Count == 0)
                {
                    return;
                }
                int selectCount = ran.Next(1, lstSelectedRowData.Count + 1);
                dicAll[rowIndex].RemoveRange(0, selectCount);
                if (dicAll[rowIndex].Count == 0)//���һ��ȡ���б��еĻ���ж�Ϊ��
                {
                    string content = string.Empty;
                    if (operation == Operation.userOne)
                    {
                        content = "userOne lose";
                    }
                    else
                    {
                        content = "userTwo lose";
                    }
                    this.Invoke(new setTxtContent(ShowLog), new object[] { content });
                }
            }
        }

        /// <summary>
        /// ������ʾ���
        /// </summary>
        /// <param name="msg"></param>
        private void ShowLog(string msg)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.AppendText("\r\n");
                textBox1.AppendText("\r\n");
            }
            textBox1.AppendText(msg);
        }

    }
    /// <summary>
    /// ������ö��
    /// </summary>
    public enum Operation
    {
        userOne,
        userTwo
    }


   
}