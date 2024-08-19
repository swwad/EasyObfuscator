using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq.Expressions;
using System.Net.Http;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyObfuscator
{
    public class CodeAnalyzer
    {
        /// <summary>
        /// C# 保留關鍵字、上下文關鍵字、預處理器指令和常見類型名稱集合
        /// </summary>
        private static readonly HashSet<string> ReservedKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            // C# 關鍵字
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else",
            "enum", "event", "explicit", "extern", "false", "finally", "fixed", "float", "for",
            "foreach", "goto", "if", "implicit", "in", "int", "interface", "internal", "is", "lock",
            "long", "namespace", "new", "null", "object", "operator", "out", "override", "params",
            "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed",
            "short", "sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw",
            "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort", "using",
            "virtual", "void", "volatile", "while",
            
            // 上下文關鍵字
            "add", "alias", "ascending", "async", "await", "by", "descending", "dynamic", "equals",
            "from", "get", "global", "group", "into", "join", "let", "nameof", "on", "orderby",
            "partial", "remove", "select", "set", "value", "var", "when", "where", "yield",
            
            // 預處理器指令
            "define", "elif", "else", "endif", "endregion", "error", "if", "line", "pragma",
            "region", "undef", "warning",

            // 常見的類型名稱和方法
            "object", "string", "int", "long", "float", "double", "decimal", "bool", "byte", "char",
            "void", "short", "ushort", "uint", "ulong", "sbyte", "DateTime", "TimeSpan", "Guid",
            "Uri", "Version", "Tuple", "Nullable", "Lazy", "Func", "Action", "Predicate", "Exception",
            "Type", "Enum", "Delegate", "MulticastDelegate", "Array", "IEnumerable", "ICollection",
            "IList", "IDictionary", "List", "Dictionary", "HashSet", "Queue", "Stack", "LinkedList",
            "ArrayList", "Hashtable", "Task", "CancellationToken", "IDisposable", "EventArgs",
            "EventHandler", "Attribute", "Comparer", "EqualityComparer", "StringComparer",
            "StringBuilder", "Regex", "Match", "MatchCollection", "Group", "GroupCollection",
            "Capture", "CaptureCollection", "Stream", "MemoryStream", "FileStream", "StreamReader",
            "StreamWriter", "BinaryReader", "BinaryWriter", "File", "Directory", "Path", "Random",
            "Math", "Convert", "BitConverter", "Encoding", "UTF8Encoding", "ASCIIEncoding",
            "UnicodeEncoding", "Interlocked", "Monitor", "Mutex", "Semaphore", "Timer", "Stopwatch",
            "Process", "AppDomain", "Assembly", "MethodInfo", "PropertyInfo", "FieldInfo",
            "ParameterInfo", "ConstructorInfo", "MemberInfo", "ReflectionTypeLoadException",
            "XmlReader", "XmlWriter", "XmlDocument", "XElement", "XAttribute", "JsonSerializer",
            "JsonConvert", "DataSet", "DataTable", "DataRow", "DataColumn", "DataView",
            "ObservableCollection", "ReadOnlyCollection", "Span", "Memory", "ImmutableArray",
            "ImmutableList", "ImmutableDictionary", "ImmutableHashSet", "ConcurrentDictionary",
            "ConcurrentQueue", "ConcurrentStack", "ConcurrentBag", "Lazy", "Tuple", "ValueTuple",

            // 新增的常見類別、方法和屬性名稱
            "SqlClient", "Specialized", "Sockets", "RegularExpressions", "Empty", "Substring",
            "Count", "Fatal", "Format", "StackTrace", "GetFrame", "GetMethod", "AddDays",
            "AddHours", "AddMonths", "Length", "ParseExact", "CultureInfo", "InvariantCulture",
            "Function", "AddSeconds", "NewGuid", "GetHashCode", "RandString", "Chars",
            "Append", "PadLeft", "DayOfYear", "PadRight", "AddDate", "ASCII", "GetString",
            "Split", "Replace", "Local", "GetSection", "Formatter", "Label", "GetHostEntry",
            "AddressList", "Sleep", "ToString", "ToLower", "ToUpper", "Trim", "TrimStart", "TrimEnd",
            "IndexOf", "LastIndexOf", "Contains", "StartsWith", "EndsWith", "Remove", "Insert",
            "ToArray", "ToList", "FirstOrDefault", "LastOrDefault", "Single", "SingleOrDefault",
            "Any", "All", "Where", "Select", "OrderBy", "OrderByDescending", "GroupBy",
            "Join", "Max", "Min", "Sum", "Average", "Distinct", "Concat", "Union", "Intersect",
            "Except", "Reverse", "Take", "Skip", "TakeWhile", "SkipWhile", "ElementAt",
            "ElementAtOrDefault", "First", "Last", "DefaultIfEmpty", "OfType", "Cast",
            "ToLookup", "ToDictionary", "Aggregate", "Zip", "Repeat", "Range", "Empty",
            "Equals", "GetType", "GetValue", "SetValue", "Invoke", "BeginInvoke", "EndInvoke",
            "Dispose", "Initialize", "Reset", "Clear", "Add", "AddRange", "RemoveAt",
            "RemoveAll", "Sort", "BinarySearch", "Find", "FindAll", "FindIndex",
            "ForEach", "GetEnumerator", "CopyTo", "Clone", "CompareTo", "GetBytes", "GetChars"
        };

        /// <summary>
        /// 常見命名空間和庫名稱前綴集合
        /// </summary>
        private static readonly HashSet<string> CommonNamespaces = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "System", "Microsoft", "Windows", "Newtonsoft", "EntityFramework", "Xunit", "NUnit", "Moq",
            "AutoMapper", "Dapper", "NHibernate", "FluentValidation", "Serilog", "NLog", "log4net",
            "Ninject", "Unity", "Autofac", "RestSharp", "Polly", "StackExchange", "IdentityServer",
            "AspNetCore", "EntityFrameworkCore", "Mvc", "WebApi", "Owin", "SignalR", "Blazor",
            "Json", "Xml", "Linq", "Threading", "Tasks", "Collections", "Reflection", "Diagnostics",
            "IO", "Text", "Security", "Cryptography", "Net", "Drawing", "ComponentModel", "Runtime",
            "Interop", "Globalization", "Configuration", "Data", "Messaging", "ServiceModel",
            "Abstractions", "Extensions", "Hosting", "Http", "Logging", "Options", "Primitives",
            "Providers", "Routing", "Serialization", "Services", "Swagger", "Utils", "Validation",
            "Azure", "AWS", "Google", "Firebase", "MongoDB", "Redis", "RabbitMQ", "Kafka", "Elastic",
            "GraphQL", "OpenApi", "Npgsql", "MySql", "SQLite", "Oracle", "SqlServer", "Xamarin",
            "Prism", "ReactiveUI", "MediatR", "FluentAssertions", "Shouldly", "Bogus", "Faker",
            "Hangfire", "Quartz", "HealthChecks", "Refit", "Scrutor", "LazyCache", "CsvHelper",
            "EPPlus", "iTextSharp", "PdfSharp", "Humanizer", "BenchmarkDotNet", "ClosedXML"
        };

        public CodeAnalyzer()
        {
            AddCommonTypeMembersToReservedKeywords();
        }

        /// <summary>
        /// 使用反射將常用類型的成員添加到保留關鍵字集合中
        /// </summary>
        private void AddCommonTypeMembersToReservedKeywords()
        {
            var commonTypes = new[]
            {
                // 基本類型
                typeof(object), typeof(string), typeof(int), typeof(long), typeof(float), typeof(double),
                typeof(decimal), typeof(bool), typeof(char), typeof(byte), typeof(sbyte), typeof(short),
                typeof(ushort), typeof(uint), typeof(ulong),

                // 日期和時間
                typeof(DateTime), typeof(DateTimeOffset), typeof(TimeSpan),

                // 系統類型
                typeof(Guid), typeof(Uri), typeof(Version), typeof(Enum), typeof(Array), typeof(Nullable<>),
                typeof(ValueTuple<>), typeof(Tuple<>), typeof(Lazy<>), typeof(WeakReference), typeof(WeakReference<>),

                // 集合
                typeof(List<>), typeof(Dictionary<,>), typeof(HashSet<>), typeof(Queue<>), typeof(Stack<>),
                typeof(LinkedList<>), typeof(SortedList<,>), typeof(SortedDictionary<,>), typeof(SortedSet<>),
                typeof(ObservableCollection<>), typeof(ReadOnlyCollection<>), typeof(IEnumerable<>),
                typeof(ICollection<>), typeof(IList<>), typeof(IDictionary<,>),

                // 併發集合
                typeof(ConcurrentDictionary<,>), typeof(ConcurrentQueue<>), typeof(ConcurrentStack<>),
                typeof(ConcurrentBag<>), typeof(BlockingCollection<>),


                // 文件和IO
                typeof(File), typeof(Directory), typeof(Path), typeof(Stream), typeof(MemoryStream),
                typeof(FileStream), typeof(StreamReader), typeof(StreamWriter), typeof(BinaryReader),
                typeof(BinaryWriter), typeof(TextReader), typeof(TextWriter), typeof(StringReader),
                typeof(StringWriter),

                // XML和JSON
                typeof(XmlReader), typeof(XmlWriter), typeof(XmlDocument), typeof(XDocument), typeof(XElement),
                typeof(XAttribute),

                // 網絡
                typeof(IPAddress), typeof(IPHostEntry), typeof(Dns), typeof(Socket), typeof(SocketException),
                typeof(AddressFamily), typeof(HttpClient), typeof(HttpResponseMessage), typeof(WebClient),
                typeof(WebRequest), typeof(WebResponse), typeof(FtpWebRequest), typeof(NetworkStream),

                // 反射
                typeof(Type), typeof(Attribute), typeof(MethodBase), typeof(MethodInfo), typeof(PropertyInfo),
                typeof(FieldInfo), typeof(ConstructorInfo), typeof(ParameterInfo), typeof(Assembly),
                typeof(AppDomain),

                // 任務和多線程
                typeof(Task), typeof(Task<>), typeof(Thread), typeof(ThreadPool), typeof(Parallel),
                typeof(Interlocked), typeof(Monitor), typeof(Mutex), typeof(Semaphore), typeof(AutoResetEvent),
                typeof(ManualResetEvent), typeof(CancellationToken), typeof(CancellationTokenSource),

                // 異常處理
                typeof(Exception), typeof(SystemException), typeof(ArgumentException), typeof(NullReferenceException),
                typeof(InvalidOperationException), typeof(NotImplementedException), typeof(NotSupportedException),

                // 其他常用類型
                typeof(Console), typeof(Math), typeof(Random), typeof(Convert), typeof(BitConverter),
                typeof(StringBuilder), typeof(Regex), typeof(Match), typeof(Stopwatch), typeof(Process),
                typeof(Environment), typeof(Activator), typeof(EventArgs), typeof(IDisposable),
                typeof(IComparable), typeof(IComparable<>), typeof(IEquatable<>), typeof(IConvertible),
                typeof(IFormattable), typeof(ICloneable),

                // 字符串處理
                typeof(string), typeof(StringComparer), typeof(StringComparison), typeof(Encoding),
                typeof(UTF8Encoding), typeof(ASCIIEncoding), typeof(UnicodeEncoding),

                // 數學和數字處理
                typeof(Math), typeof(Random),

                // 加密和安全
                typeof(RSA), typeof(DSA), typeof(SHA256), typeof(MD5), typeof(HMACSHA256),

                // 序列化
                typeof(BinaryFormatter), typeof(XmlSerializer),

                // 數據庫
                typeof(DbConnection), typeof(DbCommand), typeof(DbDataReader), typeof(DataSet), typeof(DataTable),
                typeof(DataRow), typeof(DataColumn),

                // Windows Forms (如果適用)
                typeof(Form), typeof(Control), typeof(Button), typeof(TextBox), typeof(Label),


                // LINQ
                typeof(Enumerable), typeof(Queryable), typeof(Expression), typeof(Func<>), typeof(Action<>),
                typeof(Predicate<>),

                // 診斷和跟踪
                typeof(Debug), typeof(Trace), typeof(EventLog), typeof(StackTrace), typeof(StackFrame),

                // 本地化
                typeof(CultureInfo), typeof(ResourceManager),

                // 繪圖 (如果適用)
                typeof(Graphics), typeof(Bitmap), typeof(Image), typeof(Font), typeof(Color),

                // 壓縮
                typeof(GZipStream), typeof(DeflateStream),

                // 其他
                typeof(Comparer<>), typeof(EqualityComparer<>), typeof(TimeZoneInfo), typeof(Uri),
                typeof(UriBuilder), typeof(Lazy<>), typeof(Volatile), typeof(Interlocked)
            };

            foreach (var type in commonTypes)
            {
                AddTypeMembersToReservedKeywords(type);
            }
        }

        /// <summary>
        /// 將指定類型的成員添加到保留關鍵字集合中
        /// </summary>
        /// <param name="type">要添加成員的類型</param>
        private void AddTypeMembersToReservedKeywords(Type type)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            foreach (var member in type.GetMembers(bindingFlags))
            {
                ReservedKeywords.Add(member.Name);
            }
        }

        /// <summary>
        /// 分析指定目錄中的所有 .cs 和 .aspx 檔案
        /// </summary>
        /// <param name="directoryPath">要分析的目錄路徑</param>
        /// <returns>自定義識別符集合</returns>
        public HashSet<string> AnalyzeDirectory(string directoryPath)
        {
            var customIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(file => file.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase));

            foreach (var file in files)
            {
                var fileContent = File.ReadAllText(file);
                AnalyzeCode(fileContent, customIdentifiers);
            }

            return customIdentifiers;
        }

        /// <summary>
        /// 分析指定的單一檔案
        /// </summary>
        /// <param name="filePath">要分析的檔案路徑</param>
        /// <returns>自定義識別符集合</returns>
        public HashSet<string> AnalyzeFile(string filePath)
        {
            var customIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (filePath.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) ||
                filePath.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                var fileContent = File.ReadAllText(filePath);
                AnalyzeCode(fileContent, customIdentifiers);
            }

            return customIdentifiers;
        }

        /// <summary>
        /// 分析代碼內容，提取自定義識別符
        /// </summary>
        /// <param name="code">代碼內容</param>
        /// <param name="customIdentifiers">自定義識別符集合</param>
        private void AnalyzeCode(string code, HashSet<string> customIdentifiers)
        {
            // 移除多行註釋
            code = Regex.Replace(code, @"/\*[\s\S]*?\*/", "");

            // 移除單行註釋，但保留 #region 和 #endregion
            code = Regex.Replace(code, @"(?<!#region\s|#endregion\s)//.*$", "", RegexOptions.Multiline);

            // 移除字符串字面量
            code = Regex.Replace(code, @"""(?:\\.|[^""\\])*""", "");

            // 移除字元字面量
            code = Regex.Replace(code, @"'\\?.'", "");

            // 移除數字字面量（包括十六進制）
            code = Regex.Replace(code, @"\b(0x[0-9a-fA-F]+|\d+(\.\d+)?([eE][+-]?\d+)?[dDfFmM]?)\b", "");

            // 移除 #region 和 #endregion 行
            code = Regex.Replace(code, @"^\s*#(region|endregion).*$", "", RegexOptions.Multiline);

            // 匹配所有可能的識別符
            var matches = Regex.Matches(code, @"\b[a-zA-Z_]\w*\b");

            foreach (Match match in matches)
            {
                var identifier = match.Value;
                // 檢查識別符是否符合我們的條件
                if (!ReservedKeywords.Contains(identifier, StringComparer.OrdinalIgnoreCase) &&
                    !IsCommonLibraryName(identifier) &&
                    !IsGenericTypeName(identifier) &&
                    identifier.Length >= 5) // 排除長度小於5的識別符
                {
                    customIdentifiers.Add(identifier);
                }
            }
        }

        /// <summary>
        /// 檢查識別符是否屬於常見庫名稱
        /// </summary>
        /// <param name="identifier">要檢查的識別符</param>
        /// <returns>是否屬於常見庫名稱</returns>
        private bool IsCommonLibraryName(string identifier)
        {
            return CommonNamespaces.Any(ns => identifier.StartsWith(ns, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 檢查識別符是否為泛型類型名稱
        /// </summary>
        /// <param name="identifier">要檢查的識別符</param>
        /// <returns>是否為泛型類型名稱</returns>
        private bool IsGenericTypeName(string identifier)
        {
            return Regex.IsMatch(identifier, @"^T[A-Z]?\w*$", RegexOptions.IgnoreCase);
        }
    }
}