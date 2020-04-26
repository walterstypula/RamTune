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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Data;

namespace RamTune.Controls.Converters
{
	/// <summary>
	/// Description of DataArrayBindingConverter.
	/// </summary>
	public class DataArrayBindingConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var dataArray = value as List<List<string>>;
			var indexData = parameter as int[];

			if(dataArray == null)
			{
				return null;
			}

			return dataArray[indexData[0] - 1][indexData[1] - 1];
		}
		
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Debugger.Break();
			return null;
		}
	}
}
