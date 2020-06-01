using MyUILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MyUIGenerator.UIControlHelper
{
    public class CommandHelper
    {
        public static I_CommandManager GenerateCommand()
        {
            CommandManager result = new UIControlHelper.CommandManager();
            return result;
        }
    }
    public class CommandManager : I_CommandManager
    {
        public Button Button;
        Image image;
        TextBlock textBlock;
        public event EventHandler Clicked;
        public event EventHandler EnabledChanged;
        public CommandManager()
        {
            Button = new Button();
            Button.Click += Button_Click;
            Button.Width = 100;
            StackPanel content = new StackPanel();
            content.Orientation = Orientation.Horizontal;

            image = new System.Windows.Controls.Image();
            //image.Width = 20;
            //image.Height = 15;
            //image.Source = UIHelper.GetImageFromByte(command.ImagePath);
            // Button.ImageAlign = ContentAlignment.MiddleLeft;
            content.Children.Add(image);
            // Button.BackgroundImageLayout = ImageLayout.Stretch;
            textBlock = new TextBlock();
            textBlock.Margin = new System.Windows.Thickness(5, 0, 2, 0);
            content.Children.Add(textBlock);
            Button.Content = content;
            Button.Margin = new System.Windows.Thickness(2);
            Button.Height = 22;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Clicked != null)
                Clicked(this, null);
        }

        string _ImagePath;
        public string ImagePath
        {
            set
            {
                _ImagePath = value;
                image.Source = UIHelper.GetImageFromByte(_ImagePath);
            }
            get
            {
                return _ImagePath;
            }

        }



        public void OnEnabledChanged()
        {
            throw new NotImplementedException();
        }

        public void SetEnabled(bool enabled)
        {
            Button.IsEnabled = enabled;
        }

        public void SetTitle(string title)
        {
            textBlock.Text = title;
        }
    }
}
