namespace ExamTask.ViewModels
{
    public class PaginateVM<T> where T : class,new()
    {
        public decimal TotalPage {  get; set; }
        public int CurrentPage { get; set; }    
        public int Take {  get; set; }  
        public List<T> Items { get; set; }
    }
}
