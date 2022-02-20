namespace ContactBookX.models
{
    class Phone
    {
        public int IdContact { get; set; }
        public string phone { get; set; }

        public override string ToString()
        {
            return $" {phone}  ";
        }
    }
}
