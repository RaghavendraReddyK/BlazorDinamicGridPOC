using System.ComponentModel.DataAnnotations;
using DynamicGridGeneration.Validation;

namespace DynamicGridGeneration.Model
{
    public class FormInputModel
    {
        [Required(ErrorMessage = "Table name is required.")]
        public string TableName { get; set; }

        [Required(ErrorMessage = "Column name is required.")]
        [NoSpaces(ErrorMessage = "Spaces are not allowed in the column name.")]
        public string ColumnName { get; set; }

        [Required(ErrorMessage = "Data type is required.")]
        public string DataType { get; set; }
    }
}
