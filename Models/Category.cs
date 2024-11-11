namespace OopProject.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImage {  get; set; }
        public string CategoryDescription {  get; set; }

        public Category(int categoryId, string categoryName, string categoryImage, string categoryDescription)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            CategoryImage = categoryImage;
            CategoryDescription = categoryDescription;
        }
    }
}
