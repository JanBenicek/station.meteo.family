namespace station.meteo.family.Models
{
    public class V1Uri
    {
        /// <summary>
        /// Year-month-day_Hour-Minute-Second
        /// </summary>
        public string DateTime { get; set; }

        /// <summary>
        /// Air Temperature at 5cm above the ground in °C
        /// </summary>
        public decimal Temp5 { get; set; }

        /// <summary>
        /// Air Temperature at 200cm/2m above the ground in °C
        /// </summary>
        public decimal Temp200 { get; set; }

        /// <summary>
        /// Air Humidity percentage
        /// </summary>
        public decimal Humidity { get; set; }

        /// <summary>
        /// Air Pressure
        /// </summary>
        public decimal Pressure { get; set; }

        /// <summary>
        /// Wind Speed in m/s
        /// </summary>
        public decimal WindSpeed { get; set; }

        /// <summary>
        /// Wind Direction in °
        /// </summary>
        public float WindDirection { get; set; }

        /// <summary>
        /// Rain Quantity in mm/m2
        /// </summary>
        public float RainQuantity { get; set; }





    }
}
