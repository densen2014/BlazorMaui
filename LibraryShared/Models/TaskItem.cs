using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LibraryShared
{
    [Index("Idu001", "Idu", true)]
    public class TaskItem : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null, bool force = false)
        {
            if (!force && EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    
        [Column(IsIdentity = true, IsPrimary = true)]
        [DisplayName("序号")]
        public int Id { get; set; }

        [DisplayName("名称")]
        public string Text { get => text; set { SetProperty(ref text, value); } }
        string text;

        [DisplayName("描述")]
        public string Description { get; set; }

        [Column(IsPrimary = true)]
        [DisplayName("序号U")]
        public Guid Idu { get; set; }

        public override string ToString() => $"[{Id}] {Text} ({Description})";

        public static List<TaskItem> GenerateDatas()
        {
            var r = new Random();

            var ItemList = new List<TaskItem>()
            {
                new TaskItem {  Text = "假装 First item" , Description="This is an item description." ,Idu=Guid.NewGuid()},
                new TaskItem {  Text = "的哥 Second item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new TaskItem { Text = "四风 Third item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new TaskItem {  Text = "加州 Fourth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new TaskItem { Text = "阳光 Fifth item", Description="This is an item description." ,Idu=Guid.NewGuid()},
                new TaskItem {  Text = "孔雀 Sixth item - "+ r.Next(11000).ToString(), Description="This is an item description." ,Idu=Guid.NewGuid()}
            };

            return ItemList;
        }
    }

    [Table(DisableSyncStructure = true, Name = "Item")]
    public class ItemDto : TaskItem
    {
        new public int? Id { get; set; }

    }
}