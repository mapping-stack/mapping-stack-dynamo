using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using MappingStack.Dynamo.Testing.TestFactory;
using MappingStack.Dynamo.Testing.TestFactory.DtoModel;

namespace MappingStack.Dynamo.Testing.WebSelfHosted.Controllers
{   using TDto = SampleDto;
    [EnableQuery]
    public class SampleDtoController : ODataController
    {
        readonly IQueryable<TDto> list = new List<TDto>
            { new TDto{title = "test 1", @enum = EnumScalar.EnumScalarSecond, }}
            .AsQueryable();

        [HttpGet]
        public async Task<IQueryable<TDto>> Get(ODataQueryOptions<TDto> options)
        {
            ODataQueryOptions adjustedOptions = DynamoAux.AdjustOptions(options, new List<DynamoContext>());
            IQueryable<TDto> result = adjustedOptions
                .ApplyTo(list)
                .OfType<TDto>();
            return await Task.FromResult(result);
        }
        public async Task<TDto> Get(ODataQueryOptions<TDto> options, [FromODataUri] int key)
            => await Task.FromResult(list.SingleOrDefault(_ => _.id == key) ?? throw new HttpResponseException(HttpStatusCode.NotFound));
    }
}
