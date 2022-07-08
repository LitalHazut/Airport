namespace AirportBusinessLogic.Dtos
{
    public class Station
    {
        public int StationId { get; set; }
        public string? Name { get; set; }
        public Station[] NextStations { get; set; }

        public ReadFlightDto? OccupingPlane = null;
        
        public Station(Airport.Data.Model.Station data)
        {
            StationId = data.StationId;

        }

        public void SetPlane(ReadFlightDto? p)
        {
            this.OccupingPlane = p;
        }

        public bool IsOccupied()
        {
            return this.OccupingPlane != null;

        }
        private void SendPlaneToNextStation(ReadFlightDto p)
        {
            var station = this.NextStations?.First(station => station.IsOccupied());

            if (station !=null)
            {
                station.SetPlane(p);
                this.SetPlane(null);
            }
        }

        private bool IsLastStation()
        {
            return this.NextStations.Length == 0;
        }

        private void HandlePlaneOnFinalStation(ReadFlightDto p)
        {
            this.SetPlane(null);
        }
        public void HandlePlane(ReadFlightDto p)
        {
            if(this.IsLastStation())
            {
                this.HandlePlaneOnFinalStation(p);   
            }
            else
            {
                this.SendPlaneToNextStation(p);
            }
            //Update 
        }
       
    }
}