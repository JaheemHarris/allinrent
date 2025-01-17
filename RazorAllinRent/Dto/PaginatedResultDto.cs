namespace RazorAllinRent.Dto
{
	public class PaginatedResultDto<T>
	{
		public IList<T> Items { get; set; } = new List<T>();
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public string? SearchCriteria { get; set; }
		public int TotalCount { get; set; }
	}
}
