namespace DynamicGridGeneration.Model
{
    public class ColumnVisibility
    {
        public  int Id {  get; set; }

        public string TableName {  get; set; }

        public string ColumnName {  get; set; }

        public bool IsVisible { get; set; }
    }
}
