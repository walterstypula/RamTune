using System.Collections.Generic;
using System.ComponentModel;

namespace MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Members

        private readonly Dictionary<string, object> properties = new Dictionary<string, object>();

        protected void Set<T>(string property, T value)
        {
            T current = Get<T>(property);

            if (IsEqualOrNulls(current, value) == false)
            {
                properties[property] = value;
                OnPropertyChanged(property);
            }
        }

        protected T Get<T>(string property, T defaultValue = default(T))
        {
            if (properties.ContainsKey(property))
            {
                return (T)properties[property];
            }

            return defaultValue;
        }

        public bool IsEqualOrNulls(object objA, object objB)
        {
            if (objA == objB)
            {
                return true;
            }

            if (objA == null || objB == null)
            {
                return false;
            }

            return objA.Equals(objB);
        }
    }
}