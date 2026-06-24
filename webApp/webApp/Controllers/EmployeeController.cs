using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApp.Dto;
using webApp.GenericResponse;
using webApp.Iservices;

namespace webApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            try
            {
                var result = await _employeeService.GetAllEmployeeAsync();
                if (!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDto>>.Failure(null, "No employees found"));
                }

                return Ok(ResponseResult<List<EmployeeDto>>.Success(result.Item2, "Employees fetched successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var result = await _employeeService.GetEmployeeByIdAsync(id);
                if (result.Item1 == 0 || result.Item2 == null)
                {
                    return NotFound(ResponseResult<EmployeeDto>.Failure(null, "Employee not found."));
                }

                return Ok(ResponseResult<EmployeeDto>.Success(result.Item2, "Employee fetched successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employee)
        {
            try
            {
                var result = await _employeeService.CreateEmployee(employee);
                if (result.Item1 == 0)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeDto employee)
        {
            try
            {
                if (id == Guid.Empty || id != employee.Id)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, "Employee ID is invalid or does not match request body."));
                }

                var result = await _employeeService.UpdateEmployee(employee);
                if (result.Item1 == 0)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(id);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
