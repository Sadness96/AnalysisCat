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
                        foreach (var itemCatDataItem in vAnalysis.CatDataItem.Where(o => o.CatAnalysisData != null))
                        {
                            dicData.Add(itemCatDataItem.DataItemInfo.DataItem, itemCatDataItem.CatAnalysisData);
                        }
                        vViewAsterixModel.AnalysisData = JsonConvert.SerializeObject(dicData);
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
