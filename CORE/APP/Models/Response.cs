namespace CORE.APP.Models
{
    public abstract class Response
    {
        public int Id { get; set; }
        public string Guid { get; set; }

        protected Response()
        {
        }

        protected Response(int id)
        {
            Id = id;
        }
    }
}