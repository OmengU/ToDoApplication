using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace frontend_WPF
{
    public partial class MainWindow : Window
    {
        
        
        private FlowDocument flowdoc;
        private ToDoList TodoList = new ToDoList();
        private SolidColorBrush background = new SolidColorBrush(Color.FromRgb(13, 17, 23));
        private SolidColorBrush secondary = new SolidColorBrush(Color.FromRgb(22, 27, 34));
        private SolidColorBrush border = new SolidColorBrush(Color.FromRgb(145, 126, 126));
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
        public void addRow(string title,  string content, bool isCompleted, TableRowGroup rowGroup)
        {
            TableRow newRow = new TableRow();
            rowGroup.Rows.Add(newRow);

            String add = isCompleted ? "✅ " : "";

            TableCell titleCell = new TableCell(new Paragraph(new Run(add+title)));
            TableCell contentCell = new TableCell(new Paragraph(new Run(content)));

            titleCell.Padding = new Thickness(8);
            contentCell.Padding = new Thickness(8);
            titleCell.BorderThickness= new Thickness(1);
            titleCell.BorderBrush = border;
            contentCell.BorderThickness= new Thickness(1);
            contentCell.BorderBrush= border;

            if (isCompleted)
            {
                titleCell.Foreground = new SolidColorBrush(Colors.White);
                contentCell.Foreground = new SolidColorBrush(Colors.White);
            }

            newRow.Cells.Add(titleCell);
            newRow.Cells.Add(contentCell);

            if (isCompleted) newRow.Background = secondary;
        }
        public async void drawTable()
        {
            List<ToDo> todos = await getTodos();
            TodoList.Todos = todos;

            Table todoTable = flowdoc.FindName("todotable") as Table;
            TableRowGroup rowGroup = todoTable.FindName("group") as TableRowGroup;
            rowGroup.Rows.Clear();
            foreach(ToDo todo in TodoList.Todos)
            {
                addRow(todo.Title, todo.Content, todo.IsCompleted, rowGroup);
            }

        }
        public MainWindow()
        {
            InitializeComponent();
            drawTable();

            flowdoc = flowDocumentReader.Document;
            flowdoc.FontFamily = new FontFamily("Segoe UI");
            flowDocumentReader.Zoom = 130;

            Style windowStyle = new Style(typeof(Window));

            windowStyle.Setters.Add(new Setter(Window.BackgroundProperty, background));
            windowStyle.Setters.Add(new Setter(Window.FontFamilyProperty, new FontFamily("Segoe UI")));
            windowStyle.Setters.Add(new Setter(Window.ForegroundProperty, Brushes.White));
            windowStyle.Setters.Add(new Setter(Window.FontSizeProperty, (double)15));

            Style = windowStyle;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await TodoList.add(txtTitle.Text, txtContent.Text);
            drawTable();
        }
        private void Delete_Checked(object sender, RoutedEventArgs e)
        {
            txtIndex.IsEnabled = true;
            txtNewContent.IsEnabled = false;
            txtNewTitle.IsEnabled = false;
            txtNewTitle.Text = string.Empty;
            txtNewContent.Text = string.Empty;
        }
        private void Completed_Checked(object sender, RoutedEventArgs e)
        {
            txtIndex.IsEnabled = true;
            txtNewContent.IsEnabled = false;
            txtNewTitle.IsEnabled = false;
            txtNewTitle.Text = string.Empty;
            txtNewContent.Text = string.Empty;
        }
        private void Change_Checked(object sender, RoutedEventArgs e)
        {
            txtIndex.IsEnabled = true;
            txtNewContent.IsEnabled = true;
            txtNewTitle.IsEnabled = true;
        }
        private async void Execute_Click(object sender, RoutedEventArgs e)
        {
            if (Delete.IsChecked == true)
            {
                await TodoList.remove(int.Parse(txtIndex.Text));
                drawTable();
            }
            else if (Complete.IsChecked == true)
            {
                await TodoList.markComplete(int.Parse(txtIndex.Text));
                drawTable();
            }
            else if (Change.IsChecked == true)
            {
                await TodoList.alterData(int.Parse(txtIndex.Text), txtNewTitle.Text, txtNewContent.Text);
                drawTable();
            }
        }
    }
    
}
