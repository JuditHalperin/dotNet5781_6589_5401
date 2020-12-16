﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;

namespace DL
{
    public class DalObject : IDAL
    {
        #region Singelton

        static readonly DalObject instance = new DalObject();
        public static DalObject Instance => instance;

        static DalObject() { }
        DalObject() { }

        #endregion

        #region Buses

        void addBus(Bus bus)
        {
            Bus clonedBus = bus.Clone();
            if (DS.DataSource.Buses.Exists(item => item.LicensePlate == clonedBus.LicensePlate))
                throw new BusException("The bus already exists.");
            DS.DataSource.Buses.Add(clonedBus);
        }
        void removeBus(Bus bus)
        {
            if (!DS.DataSource.Buses.Remove(bus))
                throw new BusException("The bus does not exist.");
        }
        void updateBus(Bus bus)
        {
            Bus clonedBus = bus.Clone();
            Bus busT;
            try
            {
                busT = (from item in DS.DataSource.Buses
                        where item.LicensePlate == clonedBus.LicensePlate
                        select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new BusException("The bus does not exist.");
            }
            DS.DataSource.Buses.Remove(busT); // remove the old bus
            DS.DataSource.Buses.Add(clonedBus); // add the updated bus           
        }
        Bus getBus(string licensePlate)
        {
            Bus bus;
            try
            {
                bus = (from item in DS.DataSource.Buses
                       where item.LicensePlate == licensePlate
                       select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new BusException("The bus does not exist.");
            }
            return bus;
        }
        IEnumerable<Bus> GetBuses()
        {
            IEnumerable<Bus> buses = from item in DS.DataSource.Buses
                                     select item.Clone();
            if (buses.Count() == 0)
                throw new BusException("No buses exist.");
            return buses;
        }
        IEnumerable<Bus> GetBuses(Predicate<Bus> condition)
        {
            IEnumerable<Bus> buses = from item in DS.DataSource.Buses
                                     where condition(item)
                                     select item.Clone();
            if (buses.Count() == 0)
                throw new BusException("No buses exist.");
            return buses;
        }

        #endregion

        #region Lines

        void addLine(Line line)
        {
            Line clonedLine = line.Clone();
            if (DS.DataSource.Lines.Exists(item => item.ThisSerial == clonedLine.ThisSerial))
                throw new LineException("The line already exists.");
            DS.DataSource.Lines.Add(clonedLine);
        }
        void removeLine(Line line)
        {
            if (!DS.DataSource.Lines.Remove(line))
                throw new LineException("The line does not exist.");
        }
        void updateLine(Line line)
        {
            Line clonedLine = line.Clone();
            Line lineT;
            try
            {
                lineT = (from item in DS.DataSource.Lines
                         where item.ThisSerial == clonedLine.ThisSerial
                         select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new LineException("The line does not exist.");
            }
            DS.DataSource.Lines.Remove(lineT); // remove the old line
            DS.DataSource.Lines.Add(clonedLine); // add the updated line    
        }
        Line getLine(int serial)
        {
            Line line;
            try
            {
                line = (from item in DS.DataSource.Lines
                        where item.ThisSerial == serial
                        select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new LineException("The line does not exist.");
            }
            return line;
        }
        IEnumerable<Line> GetLines()
        {
            IEnumerable<Line> lines = from item in DS.DataSource.Lines
                                     select item.Clone();
            if (lines.Count() == 0)
                throw new LineException("No lines exist.");
            return lines;
        }
        IEnumerable<Line> GetLines(Predicate<Line> condition)
        {
            IEnumerable<Line> lines = from item in DS.DataSource.Lines
                                      where condition(item)
                                      select item.Clone();
            if (lines.Count() == 0)
                throw new LineException("No lines exist.");
            return lines;
        }

        #endregion

        #region Stations

        void addStation(Station station)
        {
            Station clonedStation = station.Clone();
            if (DS.DataSource.Stations.Exists(item => item.ID == clonedStation.ID))
                throw new StationException("The station already exists.");
            DS.DataSource.Stations.Add(clonedStation);
        }
        void removeStation(Station station)
        {
            if (!DS.DataSource.Stations.Remove(station))
                throw new StationException("The station does not exist.");
        }
        void updateStation(Station station)
        {
            Station clonedStation = station.Clone();
            Station stationT;
            try
            {
                stationT = (from item in DS.DataSource.Stations
                            where item.ID == clonedStation.ID
                            select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new LineException("The station does not exist.");
            }
            DS.DataSource.Stations.Remove(stationT); // remove the old station
            DS.DataSource.Stations.Add(clonedStation); // add the updated station
        }
        Station getStation(int id)
        {
            Station station;
            try
            {
                station = (from item in DS.DataSource.Stations
                           where item.ID == id
                           select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new LineException("The station does not exist.");
            }
            return station;
        }
        IEnumerable<Station> GetStations()
        {
            IEnumerable<Station> stations =  from item in DS.DataSource.Stations
                                             select item.Clone();
            if (stations.Count() == 0)
                throw new StationException("No stations exist.");
            return stations;
        }
        IEnumerable<Station> GetStations(Predicate<Station> condition)
        {
            IEnumerable<Station> stations = from item in DS.DataSource.Stations
                                            where condition(item)
                                            select item.Clone();
            if (stations.Count() == 0)
                throw new StationException("No stations exist.");
            return stations;
        }

        #endregion

        #region LineStations

        void addLineStation(LineStation lineStation)
        {
            LineStation clonedLineStation = lineStation.Clone();
            if (DS.DataSource.LineStations.Exists(item => item.NumberLine == clonedLineStation.NumberLine && item.ID == clonedLineStation.ID))
                throw new StationException("The line station already exists.");
            DS.DataSource.LineStations.Add(clonedLineStation);
        }
        void removeLineStation(LineStation lineStation)
        {
            if (!DS.DataSource.LineStations.Remove(lineStation))
                throw new StationException("The line station does not exist.");
        }
        void updateLineStation(LineStation lineStation)
        {
            LineStation clonedLineStation = lineStation.Clone();
            LineStation lineStationT;
            try
            {
                lineStationT = (from item in DS.DataSource.LineStations
                                where item.NumberLine == clonedLineStation.NumberLine && item.ID == clonedLineStation.ID
                                select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new StationException("The line station does not exist.");
            }
            DS.DataSource.LineStations.Remove(lineStationT); // remove the old line station
            DS.DataSource.LineStations.Add(clonedLineStation); // add the updated line station
        }
        LineStation getLineStation(int numberLine, int id)
        {
            LineStation lineStation;
            try
            {
                lineStation = (from item in DS.DataSource.LineStations
                               where item.NumberLine == numberLine && item.ID == id
                               select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new StationException("The line station does not exist.");
            }
            return lineStation;
        }
        IEnumerable<LineStation> GetLineStations()
        {
            IEnumerable<LineStation> lineStations = from item in DS.DataSource.LineStations
                                                    select item.Clone();
            if (lineStations.Count() == 0)
                throw new StationException("No line stations exist.");
            return lineStations;
        }
        IEnumerable<LineStation> GetLineStations(Predicate<LineStation> condition)
        {
            IEnumerable<LineStation> lineStations = from item in DS.DataSource.LineStations
                                                    where condition(item)
                                                    select item.Clone();
            if (lineStations.Count() == 0)
                throw new StationException("No line stations exist.");
            return lineStations;
        }

        #endregion

        #region FollowingStations

        void addTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            TwoFollowingStations clonedTwoFollowingStations = twoFollowingStations.Clone();
            if (DS.DataSource.FollowingStations.Exists(item => item.FirstStationID == clonedTwoFollowingStations.FirstStationID && item.SecondStationID == clonedTwoFollowingStations.SecondStationID))
                throw new StationException("The two following stations already exist.");
            DS.DataSource.FollowingStations.Add(clonedTwoFollowingStations);
        }
        void removeTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            if (!DS.DataSource.FollowingStations.Remove(twoFollowingStations))
                throw new StationException("The two following stations do not exist.");
        }
        void updateTwoFollowingStations(TwoFollowingStations twoFollowingStations)
        {
            TwoFollowingStations clonedTwoFollowingStations = twoFollowingStations.Clone();
            TwoFollowingStations twoFollowingStationsT;
            try
            {
                twoFollowingStationsT = (from item in DS.DataSource.FollowingStations
                                         where item.FirstStationID == clonedTwoFollowingStations.FirstStationID && item.SecondStationID == clonedTwoFollowingStations.SecondStationID
                                         select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new StationException("The two following stations do not exist.");
            }
            DS.DataSource.FollowingStations.Remove(twoFollowingStationsT); // remove the old two following stations
            DS.DataSource.FollowingStations.Add(clonedTwoFollowingStations); // add the updated two following stations
        }
        TwoFollowingStations getTwoFollowingStations(int firstStationID, int secondStationID)
        {
            TwoFollowingStations twoFollowingStations;
            try
            {
                twoFollowingStations = (from item in DS.DataSource.FollowingStations
                                        where item.FirstStationID == firstStationID && item.SecondStationID == secondStationID
                                        select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new StationException("The two following stations do not exist.");
            }
            return twoFollowingStations;
        }
        IEnumerable<TwoFollowingStations> GetFollowingStations()
        {
            IEnumerable<TwoFollowingStations> followingStations = from item in DS.DataSource.FollowingStations
                                                                  select item.Clone();
            if (followingStations.Count() == 0)
                throw new StationException("No following stations exist.");
            return followingStations;
        }
        IEnumerable<TwoFollowingStations> GetFollowingStations(Predicate<TwoFollowingStations> condition)
        {
            IEnumerable<TwoFollowingStations> followingStations = from item in DS.DataSource.FollowingStations
                                                                  where condition(item)
                                                                  select item.Clone();
            if (followingStations.Count() == 0)
                throw new StationException("No following stations exist.");
            return followingStations;
        }

        #endregion

        #region DrivingBuses

        void addDrivingBus(DrivingBus drivingBus)
        {
            DrivingBus clonedDrivingBus = drivingBus.Clone();
            if (DS.DataSource.DrivingBuses.Exists(item => item.ThisSerial == clonedDrivingBus.ThisSerial && item.LicensePlate == clonedDrivingBus.LicensePlate && item.Line == clonedDrivingBus.Line && item.Start == clonedDrivingBus.Start))
                throw new BusException("The driving bus already exists.");
            DS.DataSource.DrivingBuses.Add(clonedDrivingBus);
        }
        void removeDrivingBus(DrivingBus drivingBus)
        {
            if (!DS.DataSource.DrivingBuses.Remove(drivingBus))
                throw new BusException("The driving bus does not exist.");
        }
        DrivingBus getDrivingBus(int thisSerial, string licensePlate, int line, DateTime start)
        {
            DrivingBus drivingBus;
            try
            {
                drivingBus = (from item in DS.DataSource.DrivingBuses
                              where item.ThisSerial == thisSerial && item.LicensePlate == licensePlate && item.Line == line && item.Start == start
                              select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new BusException("The driving bus does not exist.");
            }
            return drivingBus;
        }
        IEnumerable<DrivingBus> GetDrivingBuses()
        {
            IEnumerable<DrivingBus> drivingBuses = from item in DS.DataSource.DrivingBuses
                                                   select item.Clone();
            if (drivingBuses.Count() == 0)
                throw new BusException("No driving buses exist.");
            return drivingBuses;
        }
        IEnumerable<DrivingBus> GetDrivingBuses(Predicate<DrivingBus> condition)
        {
            IEnumerable<DrivingBus> drivingBuses = from item in DS.DataSource.DrivingBuses
                                                   where condition(item)
                                                   select item.Clone();
            if (drivingBuses.Count() == 0)
                throw new BusException("No driving buses exist.");
            return drivingBuses;
        }

        #endregion

        #region DrivingLines

        void addDrivingLine(DrivingLine drivingLine)
        {
            DrivingLine clonedDrivingLine = drivingLine.Clone();
            if (DS.DataSource.DrivingLines.Exists(item => item.NumberLine == clonedDrivingLine.NumberLine && item.Start == clonedDrivingLine.Start))
                throw new LineException("The driving line already exists.");
            DS.DataSource.DrivingLines.Add(clonedDrivingLine);
        }
        void removeDrivingLine(DrivingLine drivingLine)
        {
            if (!DS.DataSource.DrivingLines.Remove(drivingLine))
                throw new LineException("The driving line does not exist.");
        }
        DrivingLine getDrivingLine(int numberLine, DateTime start)
        {
            DrivingLine drivingLine;
            try
            {
                drivingLine = (from item in DS.DataSource.DrivingLines
                               where item.NumberLine == numberLine && item.Start == start
                               select item).First();
            }
            catch (ArgumentNullException)
            {
                throw new LineException("The driving line does not exist.");
            }
            return drivingLine;
        }
        IEnumerable<DrivingLine> GetDrivingLines()
        {
            IEnumerable<DrivingLine> drivingLines = from item in DS.DataSource.DrivingLines
                                                    select item.Clone();
            if (drivingLines.Count() == 0)
                throw new LineException("No driving lines exist.");
            return drivingLines;
        }
        IEnumerable<DrivingLine> GetDrivingLines(Predicate<DrivingLine> condition)
        {
            IEnumerable<DrivingLine> drivingLines = from item in DS.DataSource.DrivingLines
                                                    where condition(item)
                                                    select item.Clone();
            if (drivingLines.Count() == 0)
                throw new LineException("No driving lines exist.");
            return drivingLines;
        }

        #endregion
    }
}
