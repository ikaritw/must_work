using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using LINQtoCSV;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        private string modifiedDate = "2014/2/16";
        Boolean flag = false;
        int maxFileSize = 0;
        int currentProgressSize = 0;
        String songfilename = string.Empty;
        FileInfo fileinfo;
        Dictionary<string, string> existSong = new Dictionary<string, string>();

        //宣告delegate
        delegate void MyInvokeUpdateLab(int stepSize, string title);
        private void UpdateLab(int stepSize, string title) {
            if (stepSize <= maxFileSize) {
                progressOfFile.Value = stepSize;
            }
            txtCurrentStatus.Text = title;
        }

        delegate void MyInvokeUpdateResult(CsvContext cc, Dictionary<string, List<songdata>> result, int totalCount);
        private void updateResult(CsvContext cc, Dictionary<string, List<songdata>> result, int totalCount) {
            string simplefilename = fileinfo.Name.Replace(fileinfo.Extension, "");
            string datestamp = DateTime.Now.ToString("yyyyMMdd");
            simplefilename = string.Format("{0}-{1}", simplefilename, datestamp);
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("chinese", "華語作品");
            dict.Add("other", "其它作品");
            dict.Add("english", "英語作品");
            dict.Add("uncategory", "未分類作品");
            dict.Add("exist", "已存在作品");

            txtResult.Clear();
            txtResult.Text += string.Format("{0}" + Environment.NewLine, DateTime.Now.ToString("yyyy/MM/dd hh:MM:ss"));
            foreach (KeyValuePair<string, List<songdata>> item in result) {
                string outputFilename = string.Format("{0}-{1}.csv", simplefilename, item.Key);
                outputFilename = Path.Combine(fileinfo.DirectoryName, outputFilename);//取得完整名稱，會與來源檔同一路徑
                txtResult.Text += string.Format("{0}共{1}筆→{2}" + Environment.NewLine, dict[item.Key], item.Value.Count, outputFilename);
                cc.Write<songdata>(item.Value, outputFilename);
            }
            txtResult.Text += string.Format("總數共{0}筆", totalCount);
            MessageBox.Show("分析完成");

            string logFilename = "SongListConvert_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".log";
            logFilename = Path.Combine(fileinfo.DirectoryName, logFilename);
            using (StreamWriter w = File.AppendText(logFilename)) {
                w.WriteLine(txtResult.Text);
            }
        }

        public Form1() {
            InitializeComponent();
        }

        /// <summary>
        /// 刪除指定陣列中的字元
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private string deleteChar(string p1, string[] p2, string replaceStr) {
            foreach (var s in p2) {
                p1 = p1.Replace(s, replaceStr);
            }
            return p1;
        }

        string replaceTargetPattern(Regex regx, string sourceStr, string replacement) {
            sourceStr = regx.Replace(sourceStr, replacement);
            return sourceStr;
        }

        int getSongSize(songdata song) {
            //1,1,667817471,"Gwiyomi Song (Intro)","","","Hari"
            const int fixedCharLength = 14 + 1;//14個標點跟斷行符號
            return fixedCharLength + song.BatchNo.Length
                + song.BatchSeqNo.Length
                + song.AdamId.Length
                + song.Title.Length
                + song.CIp.Length
                + song.AIp.Length
                + song.Arts.Length;
        }

        /// <summary>
        /// 清理指定的檔案並另存新檔
        /// </summary>
        /// <param name="filename"></param>
        private void clearData1(object arg) {
            if (!flag) {
                return;
            }

            MyInvokeUpdateLab invokeLabel = new MyInvokeUpdateLab(UpdateLab);
            MyInvokeUpdateResult invodeTextarea = new MyInvokeUpdateResult(updateResult);

            string filename = string.Empty;
            if (arg != null) {
                filename = (string)arg;
            }

            CsvFileDescription cv = new CsvFileDescription {
                SeparatorChar = ',',
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<songdata> songlist = cc.Read<songdata>(filename, cv);

            //用空白置換
            string pattern1 = "[!@#\\$%\\^\\*…~'\"`‘’”\\-:：_]";
            string replacement1 = " ";
            Regex rgx1 = new Regex(pattern1, RegexOptions.IgnoreCase);

            string pattern3_1 = @"(&|(\s&\s))";
            string pattern3_2 = @"(,|&|(\s&\s))";
            string replacement3_1 = " and ";
            string replacement3_2 = "/";
            Regex rgx3_1 = new Regex(pattern3_1, RegexOptions.IgnoreCase);
            Regex rgx3_2 = new Regex(pattern3_2, RegexOptions.IgnoreCase);

            string pattern4 = @"(\d{1,}[\.\-_]).*";
            Regex rgx4 = new Regex(pattern4);

            //title中要刪除的
            string pattern6 = @"[{\[(](feat.*|bonus track|instrumental|(acoustic|piano|guitar|main|demonstration|karaoke)\sversion|remastered|live|demo|radio edit|audio mastertone|without overdubs|with the band|ringtone|remixed)[}\[)]";
            Regex rgx6 = new Regex(pattern6, RegexOptions.IgnoreCase);

            string pattern7_KOR = @"[{\[(](korean ver(sion)?|韓(文|國?語)?)[}\])]";//→KOR
            string pattern7_JPN = @"[{\[(](japanese ver(sion)?|日(文|本?語)?)[}\])]";//→JPN
            string pattern7_ZHO = @"[{\[(](chinese ver(sion)?|中(文|國)?|國語?)[}\])]";//→ZHO
            string pattern7_ENG = @"[{\[(](english ver(sion)?|英(文|語)?)[}\])]";//→ENG
            Dictionary<string, Regex> lang_dic = new Dictionary<string, Regex>();
            lang_dic.Add("KOR", new Regex(pattern7_KOR, RegexOptions.IgnoreCase));
            lang_dic.Add("JPN", new Regex(pattern7_JPN, RegexOptions.IgnoreCase));
            lang_dic.Add("ZHO", new Regex(pattern7_ZHO, RegexOptions.IgnoreCase));
            lang_dic.Add("ENG", new Regex(pattern7_ENG, RegexOptions.IgnoreCase));

            /*
            3400～4DFFh：中日韓認同表意文字擴充A區，總計收容6,582個中日韓漢字。
            4E00～9FFFh：中日韓認同表意文字區，總計收容20,902個中日韓漢字。
            A000～A4FFh：彝族文字區，收容中國南方彝族文字和字根。
            韓AC00～D7FFh：韓文拼音組合字區，收容以韓文音符拼成的文字。
            F900～FAFFh：中日韓兼容表意文字區，總計收容302個中日韓漢字。
            FB00～FFFDh：文字表現形式區，收容組合拉丁文字、希伯來文、阿拉伯文、中日韓直式標點、小符號、半角符號、全角符號等。
            判断日语^[\u3040-\u30ff]
            判断韩语[\uac00-\ud7ff]
            判断中文[\u4e00-\u9fa5]
            */
            Regex isJapan = new Regex(@"[\u3040-\u30ff？]", RegexOptions.IgnoreCase);
            Regex isKorean = new Regex(@"[\uac00-\ud7ff]", RegexOptions.IgnoreCase);
            Regex isChniese = new Regex(@"[\u4e00-\u9fff]", RegexOptions.IgnoreCase);
            Regex isEnglish = new Regex(@"[a-zA-Z]", RegexOptions.IgnoreCase);

            Dictionary<string, List<songdata>> result = new Dictionary<string, List<songdata>>();
            result.Add("exist", new List<songdata>());
            result.Add("other", new List<songdata>());
            result.Add("chinese", new List<songdata>());
            result.Add("english", new List<songdata>());
            result.Add("uncategory", new List<songdata>());

            //華語作品
            Regex zho_rule1 = new Regex(@"[\u4e00-\u9fff\s]+[/\+][\u4e00-\u9fff\s]+", RegexOptions.IgnoreCase);
            Regex zho_rule1_1 = new Regex(@"\([\u4e00-\u9fff\s]+[/\+][\u4e00-\u9fff\s]+\)", RegexOptions.IgnoreCase);
            Regex cht_title_rule1 = new Regex(@"(.*)\((.*)\)");
            Match cht_title_rule1_match = null;

            Regex cht_arts_rule1 = new Regex(@"(.*)\((.*)\)");
            Regex cht_arts_rule2 = new Regex(@"^([\u4e00-\u9fa5\s]+)[\s]?([A-Za-z\s]+)$");
            Match cht_arts_rule1_match = null;
            Match cht_arts_rule2_match = null;
            string[] artsArray;
            List<string> ListArts = new List<string>();
            List<string> ListArts1 = new List<string>();
            List<string> ListArts2 = new List<string>();
            Regex cht_title_rule2 = new Regex(@"[\.\,]");//[.,]置換為空白
            Regex cht_title_rule3 = new Regex(@"[\.]");//[.]置換為空白

            //英語作品
            Regex eng_rule1 = new Regex(@"^(the|a|an)\s", RegexOptions.IgnoreCase);
            //Regex eng_rule2 = new Regex(@"performed\sby(.*)\)", RegexOptions.IgnoreCase);
            Regex eng_rule2 = new Regex(@"(\(|\[|\{)(karaoke.*)?(Originally)?\sPerformed\sBy\s(.*)(\)|\]|\})", RegexOptions.IgnoreCase);
            Match eng_rule2_match = null;
            Regex eng_rule3_1 = new Regex(@"[:\.,]", RegexOptions.IgnoreCase);
            Regex eng_rule3_2 = new Regex(@"^symphony.*", RegexOptions.IgnoreCase);
            Match eng_rule3_1_match = null;
            Match eng_rule3_2_match = null;
            Regex eng_rule4 = new Regex(@"[a-zA-Z\s]+/[a-zA-Z\s]+", RegexOptions.IgnoreCase);
            Regex eng_rule4_1 = new Regex(@"\([a-zA-Z\s]+/[a-zA-Z\s]+\)", RegexOptions.IgnoreCase);

            //其它作品
            Regex other_rule1 = new Regex(@".*\((.*)\).*", RegexOptions.IgnoreCase);
            Match other_rule1_match = null;

            int totalCount = 0;
            int stepSize = 0;//目前處理的長度
            bool isHasKey = (this.existSong.Count > 0);
            /*----------------迴圈開始------------------*/
            foreach (var song in songlist) {
                stepSize = getSongSize(song);
                currentProgressSize += stepSize;

                //使用主緒更新Label
                Invoke(invokeLabel, currentProgressSize, string.Format("{0}:{1}", totalCount, song.Title));
                if ("Batch No" == song.BatchNo) {
                    continue;//略過第一筆
                }

                if (isHasKey) {
                    if (existSong.ContainsKey(song.AdamId)) {
                        song.isExist = existSong[song.AdamId];
                    } else {
                        song.isExist = "";
                    }
                }

                //把title,arts複製一份
                song.OriginalTitle = song.Title;
                song.OriginalArts = song.Arts;

                //1 將檔案內指定符號用空白置換
                song.Title = rgx1.Replace(song.Title, replacement1);
                song.Arts = rgx1.Replace(song.Arts, replacement1);

                //3 Title中含有&用and取代，ARTIST中含有,&用/取代
                song.Title = rgx3_1.Replace(song.Title, replacement3_1);
                song.Arts = rgx3_2.Replace(song.Arts, replacement3_2);

                //4 Title開頭為數字連接符號(如20.)則刪除 
                //--此次略過
                //if (rgx4.Match(song.Title).Success) {
                //    Console.WriteLine(string.Format("rule4,{0} removed from {1}"
                //        , song.adamid, song.Title));
                //    song.Title = rgx4.Replace(song.Title, String.Empty);
                //}

                //5 Title開頭為全型以及半型羅馬數字連接符號(如 I.II.V.VI.X.)則刪除
                //--此次略過

                //7 替換指定字段，並指定語言欄位
                foreach (KeyValuePair<string, Regex> item in lang_dic) {
                    if (item.Value.Match(song.Title).Success) {
                        song.Title = item.Value.Replace(song.Title, String.Empty);
                        song.Language = item.Key;
                        break;
                    }
                }

                //6 刪除指定字段
                if (rgx6.Match(song.Title).Success) {
                    //Console.WriteLine(song.Title);
                }
                song.Title = replaceTargetPattern(rgx6, song.Title, String.Empty);

                //2 將Title前後空白刪除
                song.Title = song.Title.Trim();//2

                song.Category = "";//預設

                //英文的performer大家一起用
                eng_rule2_match = eng_rule2.Match(song.Title);
                if (eng_rule2_match != null && eng_rule2_match.Success) {
                    song.performer = eng_rule2_match.Groups[4].Value;//依pattern不同，是第4欄
                    song.Title = replaceTargetPattern(eng_rule2, song.Title, String.Empty);
                }

                //區分檔案
                /*
                 * 先分出日文/韓文到其它當中，
                 * 再分出中文
                 * 最後分出英文
                 * 如以上皆無則列入無法辨識
                 */
                if (!string.IsNullOrEmpty(song.isExist)) {
                    //已存在就另外放
                    result["exist"].Add(song);//在isExist欄位裡會有對應的資料
                } else if (isJapan.Match(song.Title).Success || isKorean.Match(song.Title).Success) {
                    /*
                    *其他作品：
                    *1.Title內含有()通常為其翻譯曲名，擷取()內文字另為一欄為sub-title，
                     * 但如()内文字開頭為Originally Performed By…則為另一欄為performer
                     */
                    result["other"].Add(song);//日韓文 其它作品

                    other_rule1_match = other_rule1.Match(song.Title);
                    if (other_rule1_match != null && other_rule1_match.Success) {
                        song.subTitle = other_rule1_match.Groups[1].Value;
                    }
                    
                } else if (isChniese.Match(song.Title).Success) {
                    result["chinese"].Add(song);//華語作品

                    //title有括號要分2欄
                    cht_title_rule1_match = cht_title_rule1.Match(song.Title);
                    if (cht_title_rule1_match.Success) {
                        song.Title = cht_title_rule1_match.Groups[1].Value;
                        song.Title1 = cht_title_rule1_match.Groups[2].Value;
                    }

                    //title中含有/且前後都是中文字時且不在括號中，另列為組曲類型作品
                    if (zho_rule1.Match(song.Title).Success && !zho_rule1_1.Match(song.Title).Success) {
                        song.Category += "Group,";
                    }

                    //title將.,置換成空白
                    song.Title = cht_title_rule2.Replace(song.Title, replacement1);

                    //arts將.置換成空白
                    song.Arts = cht_title_rule3.Replace(song.Arts, replacement1);

                    //ARTS→ARTS1,ARTS2
                    artsArray = song.Arts.Split('/');//有些Arts會用/區隔
                    ListArts.Clear();
                    ListArts1.Clear();
                    ListArts2.Clear();
                    for (int i = 0; i < artsArray.Length; i++) {
                        string artStr = artsArray[i];
                        //1.含有括號
                        cht_arts_rule1_match = cht_arts_rule1.Match(artStr);// 中中中AAA(BBBB)
                        if (cht_arts_rule1_match.Success) {
                            ListArts1.Add(cht_arts_rule1_match.Groups[2].Value);//取出括號
                            artStr = cht_arts_rule1_match.Groups[1].Value;//保留括號前的資料
                        }

                        //2.中英文要拆2欄
                        cht_arts_rule2_match = cht_arts_rule2.Match(artStr);// 中中中AAA
                        if (cht_arts_rule2_match.Success) {
                            ListArts2.Add(cht_arts_rule2_match.Groups[2].Value);//取出英文
                            artStr = cht_arts_rule2_match.Groups[1].Value;//保留中文
                        }

                        ListArts.Add(artStr);
                    }
                    song.Arts = string.Join("/", ListArts.ToArray());
                    song.Arts1 = string.Join("/", ListArts1.ToArray());
                    song.Arts2 = string.Join("/", ListArts2.ToArray());

                } else if (isEnglish.Match(song.Title).Success) {
                    /*
                    英文作品：(以下不分大小寫)
                    */
                    result["english"].Add(song);//英文作品

                    //Title字首為the、a、an則刪除
                    song.Title = eng_rule1.Replace(song.Title, string.Empty);

                    //是否為DP
                    // Title中同時含有三個以上的標點符號(冒號或逗號或句點)的時候，
                    //或是Title 開頭為Symphony另列為演奏(DP)作品
                    eng_rule3_1_match = eng_rule3_1.Match(song.Title);
                    eng_rule3_2_match = eng_rule3_2.Match(song.Title);
                    if ((eng_rule3_1_match.Success && eng_rule3_1_match.Groups.Count > 3)
                        || (eng_rule3_2_match.Success)) {
                        song.Category += "DP,";
                    } else {
                        //title將.,置換成空白
                        song.Title = cht_title_rule2.Replace(song.Title, replacement1);

                        //arts將.置換成空白
                        song.Arts = cht_title_rule3.Replace(song.Arts, replacement1);
                    }

                    //title將括號內另置title1並移除括號及其內容
                    cht_title_rule1_match = cht_title_rule1.Match(song.Title);
                    if (cht_title_rule1_match.Success) {
                        song.Title = cht_title_rule1_match.Groups[1].Value;
                        song.Title1 = cht_title_rule1_match.Groups[2].Value;
                    }

                    //組曲 Title中含有/且前後都是英文字時且不在括號內，另列為組曲類型作品
                    if (eng_rule4.Match(song.Title).Success && !eng_rule4_1.Match(song.Title).Success) {
                        song.Category += "Group,";
                    }

                    //arts將括號內另置arts1並移除括號及其內容
                    artsArray = song.Arts.Split('/');//有些Arts會用/區隔
                    ListArts.Clear();
                    ListArts1.Clear();
                    ListArts2.Clear();
                    for (int i = 0; i < artsArray.Length; i++) {
                        string artStr = artsArray[i];
                        //1.含有括號
                        cht_arts_rule1_match = cht_arts_rule1.Match(artStr);// 中中中AAA(BBBB)
                        if (cht_arts_rule1_match.Success) {
                            ListArts1.Add(cht_arts_rule1_match.Groups[2].Value);//取出括號
                            artStr = cht_arts_rule1_match.Groups[1].Value;//保留括號前的資料
                        }
                        ListArts.Add(artStr);
                    }
                    song.Arts = string.Join("/", ListArts.ToArray());
                    song.Arts1 = string.Join("/", ListArts1.ToArray());
                    song.Arts2 = string.Join("/", ListArts2.ToArray());
                    
                } else {
                    result["uncategory"].Add(song);//無法分類
                }

                totalCount++;//處理筆數加1
            }
            Invoke(invodeTextarea, cc, result, totalCount);//用invoke
        }

        private string deleteChar(string p1, string[] p2) {
            foreach (var s in p2) {
                p1 = p1.Replace(s, string.Empty);
            }
            return p1;
        }

        private void btnClearData_Click(object sender, EventArgs e) {
            if (isCheckExist.Checked && string.IsNullOrEmpty(txtExistFile.Text)) {
                MessageBox.Show("請選擇已存在的csv檔，\n需轉換成utf8編碼\n需含有Admin ID,Title");
            } else {
                string filename = fileinfo.FullName;
                if (!String.IsNullOrEmpty(filename)) {
                    songfilename = filename;
                    //clearData1(filename);
                    flag = true;
                    //單一執行緒
                    //AddRow();
                    //將job丟入ThreadPool Queue
                    //主緒繼續處理畫面
                    ThreadPool.QueueUserWorkItem(new WaitCallback(clearData1), filename);
                }
            }
        }

        private void 說明LToolStripButton_Click(object sender, EventArgs e) {
            MessageBox.Show("updated at " + modifiedDate);
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void btnGetExistFile_Click(object sender, EventArgs e) {
            using (OpenFileDialog od = new OpenFileDialog()) {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    string filename = od.FileName;
                    CsvFileDescription cv = new CsvFileDescription {
                        SeparatorChar = ',',
                        FirstLineHasColumnNames = false,
                        EnforceCsvColumnAttribute = true
                    };
                    CsvContext cc = new CsvContext();
                    IEnumerable<existSong> songlist = cc.Read<existSong>(filename, cv);
                    foreach (existSong song in songlist) {
                        if (!this.existSong.ContainsKey(song.AdamId)) {
                            this.existSong.Add(song.AdamId, string.Format("{0},{1}", song.AdamId, song.Title));
                        }
                    }
                    MessageBox.Show(string.Format("已載入{0}筆已存在的資料", this.existSong.Count.ToString("#,00")));
                    txtExistFile.Text = filename;
                }
            }
        }

        private void isCheckExist_CheckedChanged(object sender, EventArgs e) {
            txtExistFile.Clear();
            if (isCheckExist.Checked) {
                btnGetExistFile.Enabled = true;
                MessageBox.Show("請選擇已存在的csv檔，\n需轉換成utf8編碼\n需含有Admin ID,Title");
            } else {
                btnGetExistFile.Enabled = false;
                existSong.Clear();
            }
        }

        private void btnLoadUnreconizeFile_Click(object sender, EventArgs e) {
            using (OpenFileDialog od = new OpenFileDialog()) {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    string filename = od.FileName;
                    txtFilename.Text = filename;

                    fileinfo = new FileInfo(filename);
                    lblFIlesize.Text = fileinfo.Length.ToString("#,00");
                    maxFileSize = int.Parse(fileinfo.Length.ToString());
                    progressOfFile.Maximum = maxFileSize;
                    btnClearData.Focus();
                }
            }
        }
    }
}
