namespace ContactBookX.models
{
    class Email
    {
        public int IdContact { get; set; }
        public string email { get; set; }


        public override string ToString()
        {
            return $" {email} ";
        }
    }
}
