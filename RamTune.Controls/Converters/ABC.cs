/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 6/6/2011
 * Time: 9:05 PM
 * 
 * $
{
res:XML.StandardHeader.HowToChangeTemplateInformation
}
 */
using System;
using System.Diagnostics;
using System.Windows.Data;

namespace RamTune.Controls.Converters
{
	/// <summary>
	/// Description of DataArrayBindingConverter.
	/// </summary>
	public class ABC : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (value as string[,])[0,0];
		}
		
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debugger.Break();
			return null;
		}
	}
}
