using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ProjectFlareon.Utils
{
    public class ItemClickEventArgsToClickedItemConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ItemClickEventArgs args = (ItemClickEventArgs)value;
            return args.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        #endregion
    }
}
