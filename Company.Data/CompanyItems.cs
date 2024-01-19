using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Data
{
    public class CompanyItems
    {
        public int Id { get; set; }
        public string NameOfCompany { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }

        public CompanyItems(int id, string nameOfCompany, string description, string owner)
        {
            Id = id;
            NameOfCompany = nameOfCompany;
            Description = description;
            Owner = owner;
        }
        public override bool Equals(object obj)
        {
            try
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                CompanyItems other = (CompanyItems)obj;
                return Id == other.Id && NameOfCompany == other.NameOfCompany && Description == other.Description && Owner == other.Owner;
            }
            catch (Exception)
            {
                throw new Exception("Didn't pass the test for Equals");
            }
        }

        public override int GetHashCode()
        {
            //Id doesn't repeat!!!!! it isn't mistake 
            unchecked
            {
                int hash = 17 * 23 + Id.GetHashCode();
                return hash;
            }
        }
    }
}
