namespace Assignment.API.Model.Dto.Response
{
    public class ResponseDto<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
