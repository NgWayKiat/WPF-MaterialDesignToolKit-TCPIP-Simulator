using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfTemp.Model
{
    public partial class MessageData : ObservableObject
    {
        [ObservableProperty] 
        DateTime _Time;

        [ObservableProperty] 
        Global.MessageType _Type;

        [ObservableProperty] 
        string _Title = string.Empty;

        [ObservableProperty] 
        string _Content = string.Empty;

        [ObservableProperty]
        Brush _FColor = Brushes.Black;

        public MessageData(string Content, DateTime dateTime, Global.MessageType Type = Global.MessageType.INFO, string Title = "")
        {
            this.Type = Type;
            this.Content = Content;
            this.Time = dateTime;
            this.Title = Title;

            if(Type == Global.MessageType.INFO)
            {
                this.FColor = Brushes.Black;
            }
            else if (Type == Global.MessageType.WARN)
            {
                this.FColor = Brushes.Orange;
            }
            else if (Type == Global.MessageType.ERRS)
            {
                this.FColor = Brushes.Red;
            }
        }

    }
}
