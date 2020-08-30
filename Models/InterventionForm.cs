namespace Rocket_Elevators_Customer_Portal.Models
{
    public class InterventionFormModel
    {
        public int buildingId { get; set; }

        public int batteryId { get; set; }

        public int elevatorId { get; set; }

        public int customerId { get; set;}

        public int authorId { get; set;}

        public int employeeId { get; set;}

        public string description { get; set;}
        

    }
}