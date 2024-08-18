using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace EasyObfuscator
{
    public partial class Form1 : Form
    {
        private readonly CodeAnalyzer _codeAnalyzer;

        public Form1()
        {
            InitializeComponent();
            _codeAnalyzer = new CodeAnalyzer();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = folderBrowser.SelectedPath;
                }
            }
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolderPath.Text))
            {
                MessageBox.Show("請選擇一個資料夾。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnAnalyze.Enabled = false;
            try
            {
                // 在背景執行緒中進行分析
                var customIdentifiers = await Task.Run(() => _codeAnalyzer.AnalyzeDirectory(txtFolderPath.Text));

                if (customIdentifiers.Count > 0)
                {
                    // 異步寫入結果到文件
                    await Task.Run(() => File.WriteAllLines("word.txt", GenerateWordTxtContent(customIdentifiers)));
                    MessageBox.Show("分析完成。結果已儲存到 word.txt 檔案中。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("沒有找到自定義識別符。", "資訊", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"分析過程中發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnAnalyze.Enabled = true;
            }
        }

        /// <summary>
        /// 生成 word.txt 檔案的內容
        /// </summary>
        /// <param name="customIdentifiers">自定義識別符集合</param>
        /// <returns>檔案內容的可列舉集合</returns>
        private IEnumerable<string> GenerateWordTxtContent(HashSet<string> customIdentifiers)
        {
            yield return @"Please take the following words as C# variable or function names. Understand their original naming meaning, and rewrite them into new variable or function names with the same meaning. The new names must comply with C# standard naming conventions (PascalCase for classes, methods, and properties; camelCase for local variables and method parameters). The new names should meet the following criteria:
The new name must not be the same as the old name when ignoring case sensitivity.
The new name must not be the same as the old name when ignoring case sensitivity.
The new name must not be the same as the old name when ignoring case sensitivity.
There must be no leading or trailing spaces around the new name.
There must be no leading or trailing spaces around the new name.
There must be no leading or trailing spaces around the new name.
Output each name on a new line in the following format:
OldName:NewName";

            foreach (var identifier in customIdentifiers)
            {
                yield return $"{identifier}";
            }
        }
    }
}
