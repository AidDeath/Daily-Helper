using MaterialDesignThemes.Wpf;
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

namespace Daily_Helper.Views.Dialogs
{
    public enum MessageType
    {
        Information,
        Error
    }

    /// <summary>
    /// Interaction logic for MaterialMessageBox.xaml
    /// </summary>
    public partial class MaterialMessageBox : UserControl
    {
        private MessageType MessageType;
        public MaterialMessageBox()
        {
            InitializeComponent();
        }
        public MaterialMessageBox(string message) : this()
        {
            PrimaryText.Text = message;
            Icon.Kind = PackIconKind.InformationOutline;
        }
        public MaterialMessageBox(string message, MessageType messageType) : this()
        {
            PrimaryText.Text = message;
            MessageType = messageType;

            switch (messageType)
            {
                case MessageType.Error:
                    Icon.Kind = PackIconKind.AlertOutline;
                    break;
                case MessageType.Information:

                    break;
                default:
                    break;
            }
            Icon.Kind = PackIconKind.InformationOutline;
        }

        public static MaterialMessageBox Create(string message)
        {
            return new MaterialMessageBox(message);
        }

        public static MaterialMessageBox Create(string message, MessageType messageType)
        {
            return new MaterialMessageBox(message, messageType);
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            
            DialogHost.Close("MaterialMessageBox", "here i return");
        }
    }
}
