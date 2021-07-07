using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FontAwesome
{
    public class FontAwesomeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string reuslt = "";
            if (value is string strValue)
            {
                reuslt = (char)System.Convert.ToInt32(FontAwesomeCssLoad.FontAwesomeCss.Convert(strValue), 16) + "";
            }
            if (parameter is string parm)
            {
                reuslt = (char)System.Convert.ToInt32(FontAwesomeCssLoad.FontAwesomeCss.Convert(parm), 16) + "";
            }
            return reuslt;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class FontAwesomeCssLoad
    {
        private CssDocument _cssDocument = new CssDocument();
        private static FontAwesomeCssLoad Instance;
        private static object _lock = new object();
        public static FontAwesomeCssLoad FontAwesomeCss
        {
            get
            {
                if (Instance == null)
                {
                    lock (_lock)
                    {
                        if (Instance == null)
                        {
                            Instance = new FontAwesomeCssLoad();
                        }
                    }
                }
                return Instance;
            }
        }


        FontAwesomeCssLoad()
        {
            _cssDocument.Load(Properties.Resources.font_awesome);
            _cssDocument.Elements.ForEach(c => c.Name = c.Name.Replace(".", "").Replace(":before", ""));
        }

        public string Convert(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "";
            var cd = _cssDocument[name];
            if (cd == null) return "";
            return cd.Attributes["content"].Replace("\"", "").Replace("\\", "");
        }
    }
}
