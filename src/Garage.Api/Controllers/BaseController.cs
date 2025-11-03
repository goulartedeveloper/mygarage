using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Garage.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Garage.Api.Controllers
{
    public class BaseController<TModel> : ControllerBase where TModel : class
    {
        private readonly IValidator<TModel> _validator;

        public BaseController(IValidator<TModel> validator)
        {
            _validator = validator;
        }

        public async Task<IActionResult> ValidateAsync(TModel model)
        {
            var result = await _validator.ValidateAsync(model);

            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e =>
                    new ValidationResponse
                    {
                        Field = e.PropertyName,
                        Message = e.ErrorMessage
                    }
                ));

            return null;
        }
    }
}
