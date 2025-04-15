namespace PKT.Models
{
    public class NavBar
    {


        public int Id { get; set; }
        public string LinkText { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public int MenuId { get; set; }
        public string Access { get; set; }
        public List<NavBar> SubMenus { get; set; } // Add this property
    }
}
