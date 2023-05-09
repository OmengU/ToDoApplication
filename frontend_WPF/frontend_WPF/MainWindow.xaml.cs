using Microsoft.VisualBasic;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Net;

namespace frontend_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        
        private FlowDocument flowdoc;
        private ToDoList TodoList = new ToDoList();
        static async Task<List<ToDo>> getTodos()
        {
            using var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("http://localhost:5161/api/todo");

            if (response.IsSuccessStatusCode)
            {
                string jsonString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonString);

                List<ToDo> data = JsonConvert.DeserializeObject<List<ToDo>>(jsonString);
                return data;
            }
            else
            {
                Console.WriteLine("Error: " + response.StatusCode);
                throw new Exception("Failed to retrieve todos from API. Status code: " + response.StatusCode);
            }
        }
        public void addRow(string title,  string content, TableRowGroup rowGroup)
        {
            TableRow newRow = new TableRow();
            rowGroup.Rows.Insert(0, newRow);

            TableCell titleCell = new TableCell(new Paragraph(new Run(title)));
            TableCell contentCell = new TableCell(new Paragraph(new Run(content)));

            titleCell.BorderBrush = Brushes.Black;
            titleCell.BorderThickness = new Thickness(0, 0, 0, 1);
            contentCell.BorderBrush = Brushes.Black;
            contentCell.BorderThickness = new Thickness(0, 0, 0, 1);

            newRow.Cells.Add(titleCell);
            newRow.Cells.Add(contentCell);
        }
        public async void drawTable()
        {
            List<ToDo> todos = await getTodos();
            TodoList.Todos = todos;

            Table todoTable = flowdoc.FindName("todotable") as Table;
            TableRowGroup rowGroup = todoTable.FindName("group") as TableRowGroup;
            foreach(ToDo todo in TodoList.Todos)
            {
                addRow(todo.Title, todo.Content, rowGroup);
            }

        }
        public MainWindow()
        {
            InitializeComponent();
            drawTable();

            flowdoc = flowDocumentReader.Document;
            flowdoc.FontFamily = new FontFamily("Comic Sans MS");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TodoList.add(txtTitle.Text, txtContent.Text);

            Table todoTable = flowdoc.FindName("todotable") as Table;
            TableRowGroup rowGroup = todoTable.FindName("group") as TableRowGroup;
            addRow(txtTitle.Text, txtContent.Text, rowGroup);
        }
    }
    
}
