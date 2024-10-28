using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace _28._10_Wpf_ImgViewer
{



    public partial class MainWindow : Window
    {
        Client client;

        public MainWindow()
        {
            InitializeComponent();

            client = new Client();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Words.Add(TextBoxWord.Text);
            TextBoxWord.Text = string.Empty;
            WordsList.Content = $"Поиск по словам: \n{String.Join("\n", client.Words)}";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           var imgs =client.Find();
            foreach(var im in imgs)
            {
                Img.Children.Add(new Image()
                {
                    Source = new BitmapImage(new Uri(im.URL))
                });
            }
        }
    }


    public class Client()
    {
        public List<string>  Words = new List<string>();

        public List<ImageInfo> Find()
        {
            var API = "https://pixabay.com/api/?key=46772558-52030dc4424fabcc6be9d61de&q";
            var tags = string.Join("+", Words);

            var RequestAPI = API + $"=" + tags;

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://pixabay.com/api/?key=46772558-52030dc4424fabcc6be9d61de&q=yellow+flowers&image_type=photo&pretty=true");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequestAPI);
            request.Method = "GET";

            List<ImageInfo> imgInfo = new List<ImageInfo>();


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string responseText = reader.ReadToEnd();

                var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                if (jsonResponse.ContainsKey("hits"))
                {
                    var hits = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse["hits"].ToString());
                    
                    foreach (var hit in hits)
                    {
                        var img = new ImageInfo(hit["webformatURL"], hit["user"], hit["likes"]);
                        imgInfo.Add(img);
                      
                    }
                }


            }
            response.Close();
            return imgInfo;
        }        
    }


    public record ImageInfo(string URL, string autor, string countLikes);





    //Напишите программу на C#, которая отправляет запрос к API Pixabay для поиска изображений по ключевому слову, введенному пользователем. 
    //В ответе программа должна получать список изображений (URL). Программа также должна выводить данные о количестве лайков и авторе каждого изображения.


}



