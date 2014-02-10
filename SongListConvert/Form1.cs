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
        Boolean flag = false;
        int maxFileSize = 0;
        int currentProgressSize = 0;
        String songfilename = string.Empty;
        FileInfo fileinfo;

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


            string pattern1 = "[!@#\\$%\\^\\*…~'\"`‘’”\\.\\-]";
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

            string pattern6 = @"[{\[(](feat.*|bonus track|instrumental|(acoustic|piano|guitar|main|demonstration|karaoke)\sversion|remastered|live|demo|radio edit|audio mastertone)[}\[)]";
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
             */

            /*
            // 判断中文
            if (System.Text.RegularExpressions.Regex.IsMatch(content, @"^[\u4e00-\u9fa5]+$"))
            // 判断日语
            if (System.Text.RegularExpressions.Regex.IsMatch(content, @"^[\u3040-\u30ff]+$"))
            // 判断韩语
            if  (System.Text.RegularExpressions.Regex.IsMatch(content, @"^[\uac00-\ud7ff]+$"))
            */
            Regex isChniese = new Regex(@"[\u4e00-\u9fff]", RegexOptions.IgnoreCase);
            Regex isJapan = new Regex(@"[\u3040-\u30ff]", RegexOptions.IgnoreCase);
            Regex isKorean = new Regex(@"[\uac00-\ud7ff]", RegexOptions.IgnoreCase);
            Regex isEnglish = new Regex(@"[a-zA-Z]", RegexOptions.IgnoreCase);

            Dictionary<string, List<songdata>> result = new Dictionary<string, List<songdata>>();
            result.Add("chinese", new List<songdata>());
            result.Add("other", new List<songdata>());
            result.Add("english", new List<songdata>());
            result.Add("uncategory", new List<songdata>());

            //華語作品
            Regex zho_rule1 = new Regex(@"[\u4e00-\u9fff]+/[\u4e00-\u9fff]+", RegexOptions.IgnoreCase);
            Regex cht_title_rule1 = new Regex(@"\((.*)\)");
            Match cht_title_rule1_match = null;

            Regex cht_arts_rule1 = new Regex(@"(.*)\((.*)\)");
            Regex cht_arts_rule2 = new Regex(@"^([\u4e00-\u9fa5\s]+)[\s]?([A-Za-z\s]+)$");
            Match cht_arts_rule1_match = null;
            Match cht_arts_rule2_match = null;
            string[] artsArray;
            List<string> subart1 = new List<string>();
            List<string> subart2 = new List<string>();

            //英語作品
            Regex eng_rule1 = new Regex(@"^(the|a|an)\s", RegexOptions.IgnoreCase);
            //Regex eng_rule2 = new Regex(@"performed\sby(.*)\)", RegexOptions.IgnoreCase);
            Regex eng_rule2 = new Regex(@"(\(|\[|\{)(karaoke.*)?(Originally)?\sPerformed\sBy\s(.*)(\)|\]|\})", RegexOptions.IgnoreCase);
            Match eng_rule2_match = null;
            Regex eng_rule3_1 = new Regex(@"[:\.,]", RegexOptions.IgnoreCase);
            Regex eng_rule3_2 = new Regex(@"symphony.*", RegexOptions.IgnoreCase);
            Match eng_rule3_1_match = null;
            Match eng_rule3_2_match = null;
            Regex eng_rule4 = new Regex(@"[a-zA-Z]+/[a-zA-Z]+", RegexOptions.IgnoreCase);

            //其它作品
            Regex other_rule1 = new Regex(@".*\((.*)\).*", RegexOptions.IgnoreCase);
            Match other_rule1_match = null;

            int totalCount = 0;
            int stepSize = 0;//目前處理的長度
            /*----------------迴圈開始------------------*/
            foreach (var song in songlist) {
                stepSize = getSongSize(song);
                currentProgressSize += stepSize;

                //使用主緒更新Label
                Invoke(invokeLabel, currentProgressSize, string.Format("{0}:{1}", totalCount, song.Title));

                //把title複製為OriginalTitle
                song.OriginalTitle = song.Title;

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
                if (isJapan.Match(song.Title).Success || isKorean.Match(song.Title).Success) {
                    /*
                    *其他作品：
                    *1.Title內含有()通常為其翻譯曲名，擷取()內文字另為一欄為sub-title，
                     * 但如()内文字開頭為Originally Performed By…則為另一欄為performer
                     */
                    other_rule1_match = other_rule1.Match(song.Title);
                    if (other_rule1_match != null && other_rule1_match.Success) {
                        song.subTitle = other_rule1_match.Groups[1].Value;
                    }
                    result["other"].Add(song);//日韓文 其它作品
                } else if (isChniese.Match(song.Title).Success) {
                    //華語作品：Title中含有/且前後都是中文字時，另列為組曲類型作品
                    if (zho_rule1.Match(song.Title).Success) {
                        song.Category += "Group,";
                    }
                    result["chinese"].Add(song);//華語作品

                    //TODO title需增加判斷
                    //TITLE→TITLE,TITLE1
                    //rule1:有括號要分2欄
                    cht_title_rule1_match = cht_title_rule1.Match(song.Title);
                    if (cht_title_rule1_match.Success) {
                        song.Title1 = cht_title_rule1_match.Groups[1].Value;
                        song.Title = cht_title_rule1.Replace(song.Title, string.Empty);
                    }
                    //TODO arts需增加判斷
                    //ARTS→SUBARTS1,SUBARTS2
                    //rule1:有括號要分2欄
                    if (song.Arts.IndexOf('/') > -1) {
                        artsArray = song.Arts.Split('/');
                        subart1.Clear();
                        subart2.Clear();

                        foreach (string art in artsArray) {
                            cht_arts_rule1_match = cht_arts_rule1.Match(art);// AAAA(BBBB)
                            cht_arts_rule2_match = cht_arts_rule2.Match(art);// 中中中EEE
                            if (cht_arts_rule1_match.Success) {
                                subart1.Add(cht_arts_rule1_match.Groups[1].Value);
                                subart2.Add(cht_arts_rule1_match.Groups[2].Value);
                            } else if (cht_arts_rule2_match.Success) {
                                subart1.Add(cht_arts_rule2_match.Groups[1].Value);
                                subart2.Add(cht_arts_rule2_match.Groups[2].Value);
                            } else {
                                subart1.Add(art);
                            }
                        }
                        song.subArts1 = string.Join("/", subart1.ToArray());
                        song.subArts2 = string.Join("/", subart2.ToArray());
                    } else {
                        cht_arts_rule1_match = cht_arts_rule1.Match(song.Arts);
                        if (cht_arts_rule1_match.Success) {
                            song.subArts1 = cht_arts_rule1_match.Groups[1].Value;
                            song.subArts2 = cht_arts_rule1_match.Groups[2].Value;
                        }
                        //rule2:subArts1中英文要拆2欄
                        if (string.IsNullOrEmpty(song.subArts1)) {
                            //art1無值時，由art來判斷
                            cht_arts_rule2_match = cht_arts_rule2.Match(song.Arts);
                        } else {
                            cht_arts_rule2_match = cht_arts_rule2.Match(song.subArts1);
                        }
                        if (cht_arts_rule2_match.Success) {
                            song.subArts1 = cht_arts_rule2_match.Groups[1].Value;
                            if (string.IsNullOrEmpty(song.subArts2)) {
                                song.subArts2 = cht_arts_rule2_match.Groups[2].Value;
                            } else {
                                song.subArts2 = cht_arts_rule2_match.Groups[2].Value + song.subArts2;
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(song.subArts1)) {
                        song.subArts1 = song.Arts;
                    }
                } else if (isEnglish.Match(song.Title).Success) {
                    /*
                    英文作品：(以下不分大小寫)
                    1. Title字首為the、a、an則刪除
                    2. Title含有()且其内文字開頭為Originally Performed By…則為另一欄為performer
                    3. Title中同時含有三個以上的標點符號(冒號或逗號或句點)的時候，
                       或是Title 開頭為Symphony另列為演奏(DP)作品
                    4. Title中含有/且前後都是英文字時，另列為組曲類型作品
                     */
                    song.Title = eng_rule1.Replace(song.Title, string.Empty);

                    eng_rule3_1_match = eng_rule3_1.Match(song.Title);
                    eng_rule3_2_match = eng_rule3_2.Match(song.Title);
                    if ((eng_rule3_1_match.Success && eng_rule3_1_match.Groups.Count > 3)
                        || (eng_rule3_2_match.Success)) {
                        song.Category += "DP,";
                    }

                    if (eng_rule4.Match(song.Title).Success) {
                        song.Category += "Group,";
                    }

                    result["english"].Add(song);//英文作品
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

        private void btnOpenfile_Click(object sender, EventArgs e) {
            using (OpenFileDialog od = new OpenFileDialog()) {
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    string filename = od.FileName;
                    txtFilename.Text = filename;

                    fileinfo = new FileInfo(filename);
                    lblFIlesize.Text = fileinfo.Length.ToString("#,00");
                    maxFileSize = int.Parse(fileinfo.Length.ToString());
                    progressOfFile.Maximum = maxFileSize;
                }
            }
        }

        private void 說明LToolStripButton_Click(object sender, EventArgs e) {
            MessageBox.Show("updated at 2014/2/10");
        }

        private void 開啟OToolStripButton_Click(object sender, EventArgs e) {
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

        private void Form1_Load(object sender, EventArgs e) {
            
        }
    }
}
