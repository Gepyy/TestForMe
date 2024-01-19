using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Data
{
    public interface ICompanyRepository
    {
        CompanyItems GetByIdCompany(int getId);
        Task CreateCompany(int id, string nameOfCompany, string description, string owner);
        List<CompanyItems> ReadCompany();
        Task UpdateCompany(int id, string nameOfCompany, string description, string owner);
        Task DeleteCompany(int id);

    }
}
