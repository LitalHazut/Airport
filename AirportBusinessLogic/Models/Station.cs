﻿using Airport.Data.Contexts;

namespace AirportBusinessLogic.Models
{
    public class Station
    {
        public int StationId { get; set; }
        public string? Name { get; set; }
        public Station[] NextStations { get; set; }

        public Plane? OccupingPlane = null;

        public Station(Airport.Data.Model.Station data)
        {
            StationId = data.StationId;
            Name = data.Name;            
        }

        public void SetPlane(Plane? p)
        {
            this.OccupingPlane = p;
        }

        public bool IsOccupied()
        {
            return this.OccupingPlane != null;

        }
        private void SendPlaneToNextStation(Plane p)
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

        private void HandlePlaneOnFinalStation(Plane p)
        {
            this.SetPlane(null);
        }
        public void HandlePlane(Plane p)
        {
            if(this.IsLastStation())
            {
                this.HandlePlaneOnFinalStation(p);   
            }
            else
            {
                this.SendPlaneToNextStation(p);
            }
        }
        //public Airport.Data.Model.Station? UpdateStation(Airport.Data.Model.Station newStation)
        //{
        //    if (newStation == null) return null;
        //    _context.Stations.Update(newStation);
        //    _context.SaveChanges();
        //    return newStation;
        //}
    }
}