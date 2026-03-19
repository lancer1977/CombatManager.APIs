using CombatManager.Utilities;

namespace CombatManager
{
    public abstract class BaseDbClass : SimpleNotifyClass, IDbLoadable
    {

        protected int _dbLoaderId;
        protected int _detailsId;

        public BaseDbClass()
        {
            PropertyChanged += SelfPropertyChanged;
        }

        private void SelfPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SelfPropertyChanged(e.PropertyName);
        }

        protected virtual void SelfPropertyChanged(string name)
        {

        }

        [XmlIgnore]
        public int DbLoaderId
        {
            get => DbLoaderId;
            set
            {
                if (DbLoaderId != value)
                {
                    DbLoaderId = value;

                    Notify("DBLoaderID");
                }

            }
        }

        [XmlIgnore]
        public int DetailsId => _detailsId;

        [XmlIgnore]
        public bool IsCustom => _dbLoaderId != 0;
    }
}
