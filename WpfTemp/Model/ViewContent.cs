using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WpfTemp.Model
{
    public class ViewContent : ViewModelBase
    {
        private readonly Type _contentType;
        private readonly object? _dataContext;

        public string Name { get; }
        private object? _content;
        public object? Content => _content ??= CreateContent();

        public ViewContent(string name, Type contentType, object? dataContext = null)
        {
            Name = name;
            _contentType = contentType;
            _dataContext = dataContext;
        }

        private object? CreateContent()
        {      
            var content = Activator.CreateInstance(_contentType);
            if (_dataContext != null && content is FrameworkElement element)
            {
                element.DataContext = _dataContext;
            }

            return content;
        }
    }
}
