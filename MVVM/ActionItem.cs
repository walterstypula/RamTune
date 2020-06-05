namespace MVVM
{
    public class ActionItem
    {
        public ActionItem(string actionName, object actionSource, object param = null)
        {
            this.ActionName = actionName;
            this.ActionSource = actionSource;
            this.Param = param;
        }

        public string ActionName
        { get; private set; }

        public object ActionSource
        { get; private set; }

        public object Param
        { get; private set; }
    }
}