using AnalysisCat.Helper;
using AnalysisCat.Helper.Models;
using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnalysisCat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("14 00 46 FF 0F 01 84 16 07 41 10 A1 A0 BB 00 57 8B 48 01 44 DC F6 00 17 06 00 1F AD 0E F2 02 78 10 45 80 0C 54 F2 DB 3C 60 00 02 20 40 19 98 D0 00 00 00 00 00 01 00 0C 00 0C 00 03 00 06 00 05 00 05 A1 A0 C2 00");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("15002efba1df80000100302327660055a0b60144ae0a7802610006080388000a077e043e0d33b3c72de000800002");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("3E0034BB7D25040203000E584F003806E501460641FD2601B70D4A000D33B3C37E2080780CCB000601000550000028002A003E04");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("15 00 5b ff 9f bb 5b c3 22 16 82 00 0e 55 02 a8 b0 d7 13 f1 b1 54 fa 1b 09 f8 d8 f2 2a 7d 0d cd a8 b0 5d 78 09 e3 a8 b0 d7 2b a2 5e d1 a8 b0 5d 2e c8 41 f6 0f 01 01 a0 02 07 fd 03 6c 00 01 3d 06 9a 9e fd a8 b0 d8 0d 33 b3 e3 7d 60 00 00 aa 7d 81 c0 00 00 00 00 00 00 00 00");
            stringBuilder.Append("\r\n");
            stringBuilder.Append("0A 00 33 FB 3F 31 80 16 00 01 25 03 00 07 07 F4 10 19 CD F4 50 EF 12 EF 0C 4A 15 DC 0C AA 00 04 00 78 08 3C 00 0D 36 B9 D7 0D 60 FF FB 00 02 00 00 00 00");
            TBAsterixData.Text = stringBuilder.ToString();
        }

        /// <summary>
        /// 文本框数据发生变化触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBAsterixData_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<ViewAsterixModel> listViewAsterix = new List<ViewAsterixModel>();
            foreach (var itemAsterixDataText in TBAsterixData.Text.Split(new char[] { '\r', '\n' }).Where(o => !string.IsNullOrEmpty(o)))
            {
                var vViewAsterixModel = new ViewAsterixModel();
                vViewAsterixModel.AsterixData = itemAsterixDataText;
                //解析数据
                var vAnalysis = DataParser.Analysis(itemAsterixDataText);
                if (vAnalysis != null)
                {
                    if (vAnalysis.CatDataType == CatType.unknown)
                    {
                        vViewAsterixModel.AsterixState = AsterixState.Error;
                        vViewAsterixModel.CatDataType = vAnalysis.CatDataType;
                    }
                    else
                    {
                        vViewAsterixModel.AsterixState = AsterixState.Success;
                        vViewAsterixModel.CatDataType = vAnalysis.CatDataType;
                        vViewAsterixModel.StartLength = vAnalysis.DataStartLength;
                        vViewAsterixModel.StopLength = vAnalysis.DataStopLength;
                        //拼接识别到的数据
                        Dictionary<string, object> dicData = new Dictionary<string, object>();
                        if (vAnalysis.CatDataItem != null)
                        {
                            foreach (var itemCatDataItem in vAnalysis.CatDataItem.Where(o => o.CatAnalysisData != null))
                            {
                                dicData.Add(itemCatDataItem.DataItemInfo.DataItem, itemCatDataItem.CatAnalysisData);
                            }
                            vViewAsterixModel.AnalysisData = JsonConvert.SerializeObject(dicData);
                        }
                    }
                }
                else
                {
                    vViewAsterixModel.AsterixState = AsterixState.Unrecognized;
                    vViewAsterixModel.CatDataType = CatType.unknown;
                }
                listViewAsterix.Add(vViewAsterixModel);
            }
            DGAsterixData.ItemsSource = listViewAsterix;
        }
    }
}
