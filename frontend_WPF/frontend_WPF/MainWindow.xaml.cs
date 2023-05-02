using Microsoft.VisualBasic;
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
using System.Xml.Linq;

namespace frontend_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FlowDocument flowdoc;
        public MainWindow()
        {
            InitializeComponent();

            flowdoc = flowDocumentReader.Document;
            flowdoc.FontFamily = new FontFamily("Comic Sans MS");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Table todoTable = flowdoc.FindName("todotable") as Table;
            TableRowGroup rowGroup = todoTable.FindName("group") as TableRowGroup;

            TableRow newRow = new TableRow();
            rowGroup.Rows.Add(newRow);

            TableCell titleCell = new TableCell(new Paragraph(new Run(txtTitle.Text)));
            TableCell contentCell = new TableCell(new Paragraph(new Run(txtContent.Text)));

            titleCell.BorderBrush = Brushes.Black;
            titleCell.BorderThickness = new Thickness(0, 0, 0, 1);
            contentCell.BorderBrush = Brushes.Black;
            contentCell.BorderThickness = new Thickness(0, 0, 0, 1);

            newRow.Cells.Add(titleCell);
            newRow.Cells.Add(contentCell);
        }
    }
    
}
