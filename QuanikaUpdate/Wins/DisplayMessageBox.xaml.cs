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
using System.Windows.Shapes;

namespace QuanikaUpdate.Wins
{
    /// <summary>
    /// Interaction logic for DisplayMessageBox.xaml
    /// </summary>
    public partial class DisplayMessageBox : Window
    {
        public DisplayMessageBox()
        {
            InitializeComponent();
        }


        static DisplayMessageBox _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;
        public static MessageBoxResult Show(string caption, string msg, MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.ConfirmationWithYesNo:
                    return Show(caption, msg, MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                case MessageBoxType.ConfirmationWithYesNoCancel:
                    return Show(caption, msg, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                case MessageBoxType.Information:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Information);
                case MessageBoxType.Error:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                case MessageBoxType.Warning:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                default:
                    return MessageBoxResult.No;
            }
        }
        public static MessageBoxResult Show(string msg, MessageBoxType type)
        {
            return Show(string.Empty, msg, type);
        }
        public static MessageBoxResult Show(string msg)
        {
            return Show(string.Empty, msg, MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult Show(string caption, string text)
        {
            return Show(caption, text, MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult Show(string caption, string text, MessageBoxButton button)
        {
            return Show(caption, text, button, MessageBoxImage.None);
        }
        public static MessageBoxResult Show( string caption, string text, MessageBoxButton button, MessageBoxImage image)
        {

            _messageBox = new DisplayMessageBox { txtMsg = { Text = text }, MessageTitle = { Text = caption } };
            _messageBox.Owner = Application.Current.MainWindow;
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            _messageBox.ShowDialog();
            return _result;
        }
        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnAbort.Visibility = Visibility.Collapsed;
                    _messageBox.btnRetry.Visibility = Visibility.Collapsed;
                    _messageBox.btnOk.Visibility = Visibility.Visible;
                    _messageBox.btnOk.Focus();
                    //_messageBox.ShowDialog();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnAbort.Visibility = Visibility.Collapsed;
                    _messageBox.btnRetry.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnAbort.Visibility = Visibility.Collapsed;
                    _messageBox.btnRetry.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Focus();
                    break;

                default:
                    break;
            }
        }
        private static void SetImageOfMessageBox(MessageBoxImage image)
        {
            //ico_q_title.png
            //ico_question.png
            switch (image)
            {
                case MessageBoxImage.Warning:
                    _messageBox.SetImage("ico_question.png"); //Warning.png
                    _messageBox.SetImageTitle("ico_q_title.png");//TWarning.png
                    break;
                case MessageBoxImage.Question:
                    _messageBox.SetImage("ico_question.png");//Question.png
                    _messageBox.SetImageTitle("ico_q_title.png");//TQuestion.png
                    break;
                case MessageBoxImage.Information:
                    _messageBox.SetImage("Information.png");//Information.png
                    _messageBox.SetImageTitle("TInformation.png"); //TInformation.png
                    break;
                case MessageBoxImage.Error:
                    _messageBox.SetImage("ico_question.png");//Error.png
                    _messageBox.SetImageTitle("ico_q_title.png");//TError.png
                    break;
                default:
                    _messageBox.img.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;
            _messageBox.Close();
            _messageBox = null;
        }
        private void SetImage(string imageName)
        {
            string uri = string.Format("/ImageResources/MessageBoxImages/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(uriSource);
        }

        private void SetImageTitle(string imageName)
        {
            string uri = string.Format("/ImageResources/MessageBoxImages/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
            imgTitle.Source = new BitmapImage(uriSource);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.DragMove();
        }

        private void PackIcon_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void btnCloseMsg_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_Moveable(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Retry_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnRetry)
                _result = MessageBoxResult.OK;
            else if (sender == btnAbort)
                _result = MessageBoxResult.No;
            else
                _result = MessageBoxResult.None;

            _messageBox.Close();
            _messageBox = null;
        }
    }

    public enum MessageBoxType
    {
        ConfirmationWithYesNo = 0,
        ConfirmationWithYesNoCancel,
        Information,
        Error,
        Warning
    }

    public enum MessageBoxImage
    {
        Warning = 0,
        Question,
        Information,
        Error,
        None
    }
}
