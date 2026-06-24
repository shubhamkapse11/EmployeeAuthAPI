using Microsoft.EntityFrameworkCore;
using webApp.Data;
using webApp.Dto;
using webApp.Iservices;

namespace webApp.Services
{
    public class EmployeeService(AppDbContext _context) : IEmployeeService
    {
        public async Task<Tuple<int, List<EmployeeDto>>> GetAllEmployeeAsync()
        {
            try
            {
                var employees = await _context.Employees.AsNoTracking().Select(x => new EmployeeDto
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    LastModifiedDate = x.LastModifiedDate,
                    Department = x.Department,
                    DOB = x.DOB,
                    EmailAddress = x.EmailAddress,
                    Name = x.Name,
                    Position = x.Position
                }).ToListAsync();

                return new Tuple<int, List<EmployeeDto>>(1, employees);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching employees: " + ex.Message);
            }
        }

        public async Task<Tuple<int, EmployeeDto?>> GetEmployeeByIdAsync(Guid id)
        {
            try
            {
                var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null)
                {
                    return new Tuple<int, EmployeeDto?>(0, null);
                }

                var dto = new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    CreatedDate = employee.CreatedDate,
                    LastModifiedDate = employee.LastModifiedDate,
                    DOB = employee.DOB,
                    Position = employee.Position,
                    Department = employee.Department,
                    EmailAddress = employee.EmailAddress
                };

                return new Tuple<int, EmployeeDto?>(1, dto);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the employee: " + ex.Message);
            }
        }

        public async Task<Tuple<int, string>> CreateEmployee(EmployeeDto employee)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.EmailAddress == employee.EmailAddress);
            if (existing != null)
            {
                return new Tuple<int, string>(0, "Employee with the same email already exists.");
            }

            await _context.Employees.AddAsync(new Entities.Employee
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                Department = employee.Department,
                DOB = employee.DOB,
                EmailAddress = employee.EmailAddress,
                Name = employee.Name,
                Position = employee.Position
            });

            await _context.SaveChangesAsync();
            return new Tuple<int, string>(1, "Employee created successfully.");
        }

        public async Task<Tuple<int, string>> UpdateEmployee(EmployeeDto employee)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
            if (existing == null)
            {
                return new Tuple<int, string>(0, "Employee not found.");
            }

            if (!string.IsNullOrWhiteSpace(employee.EmailAddress) && employee.EmailAddress != existing.EmailAddress)
            {
                var emailConflict = await _context.Employees.FirstOrDefaultAsync(e => e.EmailAddress == employee.EmailAddress && e.Id != employee.Id);
                if (emailConflict != null)
                {
                    return new Tuple<int, string>(0, "Another employee with the same email already exists.");
                }
            }

            existing.Name = employee.Name;
            existing.Department = employee.Department;
            existing.DOB = employee.DOB;
            existing.EmailAddress = employee.EmailAddress;
            existing.Position = employee.Position;
            existing.LastModifiedDate = DateTime.UtcNow;

            _context.Employees.Update(existing);
            await _context.SaveChangesAsync();

            return new Tuple<int, string>(1, "Employee updated successfully.");
        }

        public async Task<Tuple<int, string>> DeleteEmployeeAsync(Guid id)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (existing == null)
            {
                return new Tuple<int, string>(0, "Employee not found.");
            }

            _context.Employees.Remove(existing);
            await _context.SaveChangesAsync();
            return new Tuple<int, string>(1, "Employee deleted successfully.");
        }
    }
}
