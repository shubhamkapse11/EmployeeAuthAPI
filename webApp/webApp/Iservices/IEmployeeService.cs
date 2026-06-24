using webApp.Dto;

using webApp.Dto;

namespace webApp.Iservices
{
    public interface IEmployeeService
    {
        Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync();
        Task<Tuple<int, EmployeeDto?>> GetEmployeeByIdAsync(Guid id);
        Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee);
        Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee);
        Task<Tuple<int, string>> DeleteEmployeeAsync(Guid id);
    }
}
