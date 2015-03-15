using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

using log4net;
using log4net.Config;

namespace checkExcelFiles {
    public partial class mainForm : Form {
        private static readonly ILog TxtLog = LogManager.GetLogger(typeof(mainForm));
        string[] filenames;

        public mainForm() {
            InitializeComponent();
            string configFilePath = Path.Combine(Application.StartupPath, "log4net.config");
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(configFilePath));
            AddLog("載入完成");
        }

        public void AddLog(object obj) {
            TxtLog.Info(obj.ToString());
            //Console.WriteLine(obj.ToString());
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = "Excel 2003(*.xls)|*.xls|Excel 2007(*.xlsx)|*.xlsx";//過濾
                ofd.Multiselect = true;//多選
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    filenames = ofd.FileNames;
                    if (filenames.Length > 0) {
                        lstFileNames.Items.Clear();
                    }
                    foreach (string filename in filenames) {
                        lstFileNames.Items.Add(filename);
                    }
                }
            }
        }

        private bool checkMusicTypeIsEmpty(IRow currentRow, int cellIndex) {
            //throw new NotImplementedException();
            bool isAllEmpty = false;
            if (currentRow.LastCellNum >= cellIndex) {
                string normalMusic = currentRow.GetCell(cellIndex).ToString();//一般音樂
                string backMusic = currentRow.GetCell(cellIndex + 1).ToString();//背景音樂
                string adMusic = currentRow.GetCell(cellIndex + 2).ToString();//廣告音樂
                isAllEmpty = string.IsNullOrEmpty(normalMusic + backMusic + adMusic);
            }
            return isAllEmpty;
        }

        private int getSongNameIndex(IRow currentRow, string columnName) {
            //throw new NotImplementedException();
            int songnameindex = -1;
            int lastCellNumber = currentRow.LastCellNum;
            for (int cellIndex = 0; cellIndex < lastCellNumber; cellIndex++) {
                if (currentRow.GetCell(cellIndex).ToString().Trim() == columnName) {
                    songnameindex = cellIndex;
                    break;
                }
            }
            return songnameindex;
        }

        private int[] getFirstRowIndex(HSSFSheet hst) {
            //throw new NotImplementedException();
            int FirstRowIndex = -1;
            int maxRowIndex = hst.LastRowNum;
            int maxCellIndex = 18;
            for (int rowIndex = 0; rowIndex < maxRowIndex; rowIndex++) {
                IRow row = hst.GetRow(rowIndex);
                for (int cellIndex = 0; cellIndex <= maxCellIndex && cellIndex < row.LastCellNum; cellIndex++) {
                    ICell cell = row.GetCell(cellIndex);
                    if (cell == null) {
                        return null;
                    }
                    string musicType = cell.ToString();//音樂類型
                    if (musicType == "音樂類型") {
                        string normalMusic = hst.GetRow(rowIndex + 1).GetCell(cellIndex).ToString();//一般音樂
                        string backMusic = hst.GetRow(rowIndex + 1).GetCell(cellIndex + 1).ToString();//背景音樂
                        string adMusic = hst.GetRow(rowIndex + 1).GetCell(cellIndex + 2).ToString();//廣告音樂
                        if (normalMusic == "一般音樂"
                            && backMusic == "背景音樂"
                            && adMusic == "廣告音樂") {
                            FirstRowIndex = rowIndex + 2;
                            return new int[] { FirstRowIndex, cellIndex };
                        }
                    }
                }
            }
            return new int[] { -1, -1 };
        }

        private void btnExeute_Click(object sender, EventArgs e) {
            string checkMark = cmbCheckmark.Text;
            if (!string.IsNullOrEmpty(checkMark)) {
                //convertFiles(filenames, checkMark);
                convertFiles(lstFileNames.Items, checkMark);
                AddLog("作業完成");
            } else {
                MessageBox.Show("需輸入要填充的字串");
                cmbCheckmark.Focus();
                AddLog("查無要填充的字串");
            }
        }


        private void convertFile(string filename, string defaultCheckMark) {
            AddLog(filename);
            HSSFWorkbook hssfWorkbook;
            hssfWorkbook = null;
            IRow currentRow = null;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite)) {
                hssfWorkbook = new HSSFWorkbook(file);
            }
            if (hssfWorkbook != null) {
                bool isChanged = false;
                int sheetCount = hssfWorkbook.NumberOfSheets;
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++) {
                    HSSFSheet hst = (HSSFSheet)hssfWorkbook.GetSheetAt(sheetIndex);
                    int[] RowCell = getFirstRowIndex(hst);//取得資料第一列的位置
                    if (RowCell[0] > -1) {
                        //currentRow = hst.GetRow(RowCell[0]);

                        //取歌曲名稱的index
                        int songNameIndex = getSongNameIndex(hst.GetRow(2), "歌曲名稱");//songNameIndex

                        int maxRowIndex = hst.LastRowNum;
                        int firstCellIndex = RowCell[1];//一般音樂的cellIndex
                        for (int rowIndex = RowCell[0]; rowIndex < maxRowIndex; rowIndex++) {
                            //目前處理的行
                            currentRow = hst.GetRow(rowIndex);
                            if (currentRow.LastCellNum > songNameIndex) {
                                //取出歌曲名稱
                                string songname = currentRow.GetCell(songNameIndex).ToString();
                                if (!string.IsNullOrEmpty(songname)) {
                                    if (currentRow.LastCellNum > firstCellIndex) {
                                        //檢查三個欄位是否都為空
                                        bool isMusicTypeEmpty = checkMusicTypeIsEmpty(currentRow, firstCellIndex);
                                        if (isMusicTypeEmpty) {
                                            //歌曲名稱不為空且都沒有記號時，把背景音樂做記號
                                            currentRow.GetCell(firstCellIndex + 1).SetCellValue(defaultCheckMark);
                                            isChanged = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (hssfWorkbook != null && isChanged) {
                    using (FileStream fs = new FileStream(filename, FileMode.Create)) {
                        hssfWorkbook.Write(fs);
                        AddLog("寫入:" + filename);
                    }
                } else {
                    AddLog("略過:" + filename);
                }
            }
        }
        private void convertFiles(string[] objectCollection, string defaultCheckMark) {
            //throw new NotImplementedException();
            foreach (string filename in objectCollection) {
                convertFile(filename, defaultCheckMark);
            }
        }

        private void convertFiles(ListBox.ObjectCollection objectCollection, string defaultCheckMark) {
            //throw new NotImplementedException();
            foreach (var filename in objectCollection) {
                convertFile(filename.ToString(), defaultCheckMark);
            }
        }
    }
}
