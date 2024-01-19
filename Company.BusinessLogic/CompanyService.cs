using Company.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.BusinessLogic
{
    public class CompanyService:ICompanyService
    {
        public readonly ICompanyRepository companyRepository;

        public CompanyService(ICompanyRepository _companyRepository)
        {
            companyRepository = _companyRepository; 
        }

        public CompanyDTO GetCompanyByID(int id)
        {
            CompanyItems company = companyRepository.GetByIdCompany(id);
            var companyDTO = new CompanyDTO()
            {
                Id = company.Id,
                NameOfCompany = company.NameOfCompany,
                Description = company.Description,
                Owner = company.Owner,
            };
            return companyDTO;
        }
        public List<CompanyDTO> ReadCompany()
        {
            List<CompanyItems> company = companyRepository.ReadCompany();
            List<CompanyDTO> companyDTO = new List<CompanyDTO>();
            foreach(CompanyItems item in company)
            {
                CompanyDTO a = new CompanyDTO()
                {
                    Id = item.Id,
                    NameOfCompany = item.NameOfCompany,
                    Description = item.Description,
                    Owner = item.Owner,
                };
                companyDTO.Add(a);
            }
            return companyDTO;
        }

        public void CreateCompany(int id, string nameOfCompany, string description, string owner)
        {
            companyRepository.CreateCompany(id, nameOfCompany, description, owner);
        }

        public void UpdateCompany(int id, string nameOfCompany, string description, string owner)
        {
            companyRepository.UpdateCompany(id, nameOfCompany, description, owner);
        }
        public void DeleteCompany(int id)
        {
            companyRepository.DeleteCompany(id);
        }
    }

    public interface ICompanyService
    {
        CompanyDTO GetCompanyByID(int id);
        void CreateCompany(int id, string nameOfCompany, string description, string owner);
        List<CompanyDTO> ReadCompany();
        void UpdateCompany(int id, string nameOfCompany, string description, string owner);
        void DeleteCompany(int id);
    }
}
