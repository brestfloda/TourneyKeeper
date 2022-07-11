using TourneyKeeper.DTO.App;
using System;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Web.WebAPI.DTO;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web.WebAPI
{
    public class BaseController<TEntity, TManager> : ApiController where TEntity : class where TManager : IManager<TEntity>, new()
    {
        protected readonly TManager manager = new TManager();

        [HttpPost]
        public HttpResponseMessage Update(EntityUpdateDTO entityUpdate)
        {
            try
            {
                var data = manager.Get(entityUpdate.Id);

                PropertyInfo propertyInfo = data.GetType().GetProperty(entityUpdate.FieldToUpdate);

                Type t = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                object safeValue = (entityUpdate.Value == null) ? null : (entityUpdate.Value.ToString() == "null") ? null : Convert.ChangeType(entityUpdate.Value, t);
                propertyInfo.SetValue(data, safeValue, null);

                manager.Update(data, entityUpdate.Token);

                return new JsonMessage(true, "");
            }
            catch (Exception e)
            {
                LogManager.LogError(e.Message);
                return new JsonMessage(false, e.Message);
            }
        }
    }
}