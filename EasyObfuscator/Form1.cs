using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

        // 修改按鈕點擊事件以選擇單一檔案
        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            // 使用 OpenFileDialog 來選擇單一檔案
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "C# 檔案 (*.cs)|*.cs|ASPX 檔案 (*.aspx)|*.aspx|所有檔案 (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = openFileDialog.FileName;
                }
            }
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolderPath.Text))
            {
                MessageBox.Show("請選擇一個檔案。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnAnalyze.Enabled = false;
            try
            {
                // 在背景執行緒中進行分析
                var customIdentifiers = await Task.Run(() => _codeAnalyzer.AnalyzeFile(txtFolderPath.Text));

                if (customIdentifiers.Count > 0)
                {
                    // 獲取原檔案的目錄和檔名（不含副檔名）
                    string directory = Path.GetDirectoryName(txtFolderPath.Text);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(txtFolderPath.Text);

                    // 構建新的輸出檔案路徑
                    string outputFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_List.txt");

                    // 異步寫入結果到檔案
                    await Task.Run(() => File.WriteAllLines(outputFilePath, GenerateWordTxtContent(customIdentifiers)));
                    MessageBox.Show($"分析完成。結果已儲存到 {outputFilePath} 檔案中。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// 生成輸出檔案的內容
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

        private async void btnObfuscator_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolderPath.Text))
            {
                MessageBox.Show("請選擇一個檔案。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string originalFilePath = txtFolderPath.Text;
            string directory = Path.GetDirectoryName(originalFilePath);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFilePath);
            string extension = Path.GetExtension(originalFilePath);
            string listFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_List.txt");
            string outputFilePath = Path.Combine(directory, $"{fileNameWithoutExtension}_Obfuscator{extension}");

            if (!File.Exists(listFilePath))
            {
                MessageBox.Show($"找不到對應的 List 檔案：{listFilePath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string[] deleteWords = tbDelete.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> replacements = await Task.Run(() => ReadReplacementList(listFilePath, deleteWords));
                string originalContent = await Task.Run(() => File.ReadAllText(originalFilePath));
                string obfuscatedContent = await Task.Run(() => ObfuscateContent(originalContent, replacements));
                obfuscatedContent = RemoveSpecificComments(obfuscatedContent);
                await Task.Run(() => File.WriteAllText(outputFilePath, obfuscatedContent));

                MessageBox.Show($"混淆完成。結果已儲存到 {outputFilePath} 檔案中。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"混淆過程中發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, string> ReadReplacementList(string listFilePath, string[] deleteWords)
        {
            var replacements = new Dictionary<string, string>();
            foreach (var line in File.ReadLines(listFilePath))
            {
                var parts = line.Split(':');
                if (parts.Length == 2)
                {
                    string oldName = parts[0].Trim();
                    string newName = RemoveDeleteWords(parts[1].Trim(), deleteWords);
                    replacements[oldName] = newName;
                }
            }
            return replacements;
        }

        private string RemoveDeleteWords(string input, string[] deleteWords)
        {
            foreach (var word in deleteWords)
            {
                string reversedWord = new string(word.Reverse().ToArray());
                string pattern = $@"(^|[^a-zA-Z0-9])({Regex.Escape(word)})([^a-zA-Z0-9]|$)";
                input = Regex.Replace(input, pattern, $"$1{reversedWord}$3", RegexOptions.IgnoreCase);
            }
            return input;
        }

        private string RemoveSpecificComments(string content)
        {
            string pattern = @"//.*\d{3}-[A-Z]-\d{5}-\d{5}.*$";
            return Regex.Replace(content, pattern, "", RegexOptions.Multiline);
        }

        private string ObfuscateContent(string content, Dictionary<string, string> replacements)
        {
            foreach (var kvp in replacements)
            {
                content = Regex.Replace(content, $@"\b{Regex.Escape(kvp.Key)}\b", kvp.Value);
            }
            content = RemoveSpecificComments(content);
            return content;
        }

    }
}