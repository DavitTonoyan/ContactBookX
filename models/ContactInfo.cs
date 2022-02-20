using System.Collections.Generic;
using System.Text;

namespace ContactBookX.models
{
    class ContactInfo
    {
        public int IdContact { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Emails { get; set; } = new List<string>();
        public List<string> Phones { get; set; } = new List<string>();


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($" ID:   {IdContact} \n");
            sb.Append($" FirstName:   {FirstName} \n");
            sb.Append($" LastName:    {LastName} \n");

            sb.Append("   Phone numbers:  \n");
            int index = 1;

            foreach (var phone in Phones)
            {
                sb.Append($"     {index}.  {phone} \n");
                index++;
            }

            sb.Append("   Emails:  \n");
            index = 1;

            foreach (var email in Emails)
            {
                sb.Append($"     {index}.  {email} \n");
                index++;
            }


            return sb.ToString();
        }
    }
}
