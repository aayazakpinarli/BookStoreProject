using CORE.APP.Models;

namespace CORE.APP.Services
{
    public interface IService<TRequest, TResponse> where TRequest : Request, new() where TResponse : Response, new()
    {
        public List<TResponse> List();

        public TResponse Item(int id);

        public TRequest Edit(int id);

        public CommandResponse Create(TRequest request);

        public CommandResponse Update(TRequest request);

        public CommandResponse Delete(int id);
    }
}